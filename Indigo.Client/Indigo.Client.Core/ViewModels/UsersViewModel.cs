using Indigo.Core.Models;
using MvvmHelpers;
using System;
using System.Threading.Tasks;

namespace Indigo.Client.Core.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
		public ObservableRangeCollection<User> Users { get; set; }
		
		public UsersViewModel()
		{
			Users = new ObservableRangeCollection<User>();	
		}

		public async Task UpdateAllUsersAsync()
		{
			try
			{
				Users.Clear();
				Users.AddRange(await Api.GetUsers());
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}