using Indigo.Core.Models;
using MvvmHelpers;
using System.Linq;
using System.Threading.Tasks;

namespace Indigo.Client.Core.ViewModels
{
    class ModifyConversationViewModel : ConversationsViewModel
    {
        Conversation _conversation;
        public Conversation Conversation
        {
            get => _conversation;
            set => SetProperty(ref _conversation, value);
        }

        ObservableRangeCollection<UserConversation> _partners;
        public ObservableRangeCollection<UserConversation> Partners
        {
            get => _partners;
            set => SetProperty(ref _partners, value);
        }

        bool _newConversation;
        public bool NewConversation
        {
            get => _newConversation;
            set => SetProperty(ref _newConversation, value);
        }

        bool _chatType;
        public bool ChatType
        {
            get => _chatType;
            set => SetProperty(ref _chatType, value);
        }

        string _saveText;
        public string SaveText
        {
            get => _saveText;
            set => SetProperty(ref _saveText, value);
        }

        public ModifyConversationViewModel(User existingUser, Conversation existingConversation = null) : base(existingUser)
        {
            NewConversation = existingConversation == null;
            Conversation = NewConversation ? new Conversation() : existingConversation;

            Partners = new ObservableRangeCollection<UserConversation>();

            if (NewConversation)
            {
                AddPartner();
            }
            else
            {
                Partners.AddRange(Conversation.UserConversations);
                ChatType = Conversation.isGroupChat;
            }
        }

        public async Task SaveConversation()
        {
            if (NewConversation)
            {
                Conversation.isGroupChat = ChatType;
                Conversation = await Server.CreateConversationAsync(User, Conversation);
                foreach (UserConversation userConversation in Partners)
                {
                    User Partner = await Server.GetUserAsync(User, userConversation.User.Username);
                    await Server.CreateUserConversationAsync(User, Conversation, Partner, userConversation.isAdmin);
                }

            }
            else
            {
                await Server.PutConversationAsync(User, Conversation);
                foreach (UserConversation userConversation in Conversation.UserConversations)
                {
                    UserConversation foundPartner = Partners.SingleOrDefault(uc => uc.User.Username == userConversation.User.Username);

                    if (foundPartner != null)
                    {
                        Partners.Remove(foundPartner);
                        if (foundPartner.isAdmin != userConversation.isAdmin)
                        {
                            //TODO Add putuserconversation
                            //User Partner = await Server.GetUserAsync(User, userConversation.User.Username);
                            //await Server.PutUserConversationAsync(User, Conversation, Partner, foundPartner.isAdmin);
                        }
                    }
                    else if (foundPartner == null)
                    {
                        User Partner = await Server.GetUserAsync(User, userConversation.User.Username);
                        //await Server.DeleteUserConversationAsync(User, Conversation, Partner);
                    }
                }
                foreach (UserConversation userConversation in Partners)
                {
                    User Partner = await Server.GetUserAsync(User, userConversation.User.Username);
                    await Server.CreateUserConversationAsync(User, Conversation, Partner, userConversation.isAdmin);
                }
            }
        }

        public bool CheckEntered()
        {
            if (Conversation.ConversationName == "")
            {
                return false;
            }

            foreach (UserConversation userConversation in Partners)
            {
                if (userConversation.User.Username == "")
                {
                    return false;
                }
            }
            return true;
        }

        async Task<User> GetPartnerUser(string partnerUsername)
        {
            return await Server.GetUserAsync(User, partnerUsername);
        }

        public void AddPartner()
        {
            Partners.Add(new UserConversation
            {
                User = new User
                {
                    Username = ""
                },
                isAdmin = false
            });
        }
    }
}