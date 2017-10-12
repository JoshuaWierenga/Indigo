using Indigo.Client.Rest;
using MvvmHelpers;

namespace Indigo.Client.ViewModels
{
    public class BaseViewModel : ObservableObject
	{
		public ServerAccess Server = new ServerAccess();
	}
}