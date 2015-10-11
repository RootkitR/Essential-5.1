using System;
namespace Essential.HabboHotel.Users.UserDataManagement
{
	internal class UserDataNotFoundException : Exception
	{
		public UserDataNotFoundException(string reason) : base(reason)
		{
		}
	}
}
