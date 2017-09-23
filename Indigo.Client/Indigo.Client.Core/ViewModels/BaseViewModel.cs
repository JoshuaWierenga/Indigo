using Indigo.Client.Core.Rest;
using MvvmHelpers;

namespace Indigo.Client.Core.ViewModels
{
    public class BaseViewModel
	{
		public IIndigoApi Api = Refit.RestService.For<IIndigoApi>("http://192.168.0.2/api");
	}
}