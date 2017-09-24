using Indigo.Client.Core.Rest;
using MvvmHelpers;

namespace Indigo.Client.Core.ViewModels
{
    public class BaseViewModel : ObservableObject
	{
		public ServerAccess Server = new ServerAccess();
	}
}