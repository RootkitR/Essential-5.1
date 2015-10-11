using System;
using System.Data;
using Essential.HabboHotel.Users.UserDataManagement;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Users;
namespace Essential.HabboHotel.Users.Authenticator
{
	internal sealed class Authenticator
	{
		internal static Habbo CreateHabbo(string ssoTicket, GameClient Session, UserDataFactory userData, UserDataFactory otherData)
		{
			return Authenticator.CreateHabbo(userData.GetUserData(), ssoTicket, Session, otherData);
		}

		private static Habbo CreateHabbo(DataRow habboData, string ssoTicket, GameClient session, UserDataFactory otherData)
		{
			uint Id = (uint)habboData["Id"];
			string Username = (string)habboData["username"];
            string Name = (string)habboData["real_name"];
			uint Rank = (uint)habboData["rank"];
			string Motto = (string)habboData["motto"];
			string ip_last = (string)habboData["ip_last"];
			string look = (string)habboData["look"];
			string gender = (string)habboData["gender"];
			int credits = (int)habboData["credits"];
			int pixels = (int)habboData["activity_points"];
            string account_created = (string)habboData["account_created"];
			double activity_points_lastupdate = (double)habboData["activity_points_lastupdate"];
            string last_loggedin = (string)habboData["last_loggedin"];
            int daily_respect_points = (int)habboData["daily_respect_points"];
            int daily_pet_respect_points = (int)habboData["daily_pet_respect_points"];
            int unmutetime = (int)habboData["unmute_timestamp"];
            double vipha_last = (double)habboData["vipha_last"];
            double viphal_last = (double)habboData["viphal_last"];
            bool passed_quiz = Essential.StringToBoolean(habboData["quiz_passed"].ToString());
            bool passed_safetyquiz = Essential.StringToBoolean(habboData["passed_safety_quiz"].ToString());
            double eventwin_last = (double)habboData["eventwin_last"];
            return new Habbo(Id, Username, Name, ssoTicket, Rank, Motto, look, gender, credits, pixels, activity_points_lastupdate, account_created, Essential.StringToBoolean(habboData["is_muted"].ToString()), (uint)habboData["home_room"], (int)habboData["newbie_status"], Essential.StringToBoolean(habboData["block_newfriends"].ToString()), Essential.StringToBoolean(habboData["hide_inroom"].ToString()), Essential.StringToBoolean(habboData["hide_online"].ToString()), Essential.StringToBoolean(habboData["vip"].ToString()), (int)habboData["volume"], (int)habboData["vip_points"], Essential.StringToBoolean(habboData["accept_trading"].ToString()), ip_last, session, otherData, last_loggedin, daily_respect_points, daily_pet_respect_points, vipha_last, viphal_last, Essential.StringToBoolean(habboData["friend_stream_enabled"].ToString()), passed_quiz, passed_safetyquiz, unmutetime, eventwin_last);
		}

		internal static Habbo CreateHabbo(string username)
		{
			UserDataFactory userdata = new UserDataFactory(username, false);
			return Authenticator.CreateHabbo(userdata.GetUserData(), "", null, userdata);
		}
	}
}
