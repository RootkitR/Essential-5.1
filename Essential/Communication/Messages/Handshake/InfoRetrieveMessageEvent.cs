using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Handshake
{
	internal sealed class InfoRetrieveMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            ServerMessage Message = new ServerMessage(Outgoing.HabboInfomation);
			Message.AppendUInt(Session.GetHabbo().Id);
			Message.AppendStringWithBreak(Session.GetHabbo().Username);
			Message.AppendStringWithBreak(Session.GetHabbo().Figure);
			Message.AppendStringWithBreak(Session.GetHabbo().Gender.ToUpper());
			Message.AppendStringWithBreak(Session.GetHabbo().Motto);
			Message.AppendStringWithBreak(Session.GetHabbo().RealName);
			Message.AppendBoolean(false);
			Message.AppendInt32(Session.GetHabbo().Respect);
			Message.AppendInt32(Session.GetHabbo().RespectPoints);
			Message.AppendInt32(Session.GetHabbo().PetRespectPoints);
            Message.AppendBoolean(true);
            Message.AppendStringWithBreak(Essential.TimestampToDate(double.Parse(Session.GetHabbo().LastOnline)).ToLongDateString());
            Message.AppendBoolean(Session.GetHabbo().HasFuse("cmd_flagme"));
            Message.AppendBoolean(false);
			Session.SendMessage(Message);


            ServerMessage Allowances = new ServerMessage(Outgoing.Allowances);
            Allowances.AppendInt32(7);//this.Session.GetHabbo().PassedQuiz ? 7 : 5);
            Allowances.AppendString("VOTE_IN_COMPETITIONS");
            Allowances.AppendBoolean(true);
            Allowances.AppendString("");
            Allowances.AppendString("TRADE");
            Allowances.AppendBoolean(true);
            Allowances.AppendString("");
            Allowances.AppendString("CITIZEN");
            Allowances.AppendBoolean(Session.GetHabbo().PassedHabboWayQuiz);
            Allowances.AppendString("");
            Allowances.AppendString("SAFE_CHAT");
            Allowances.AppendBoolean(Session.GetHabbo().PassedSafetyQuiz);
            Allowances.AppendString("");
            Allowances.AppendString("FULL_CHAT");
            Allowances.AppendBoolean(Session.GetHabbo().PassedSafetyQuiz);
            Allowances.AppendString("");
            Allowances.AppendString("CALL_ON_HELPERS");
            Allowances.AppendBoolean(true);
            Allowances.AppendString("");
            Allowances.AppendString("USE_GUIDE_TOOL");
            Allowances.AppendBoolean(Session.GetHabbo().IsGuide);
            Allowances.AppendString("requirement.unfulfilled.helper_level_4");
            Allowances.AppendString("JUDGE_CHAT_REVIEWS");
            Allowances.AppendBoolean(false);
            Allowances.AppendString("requirement.unfulfilled.helper_level_6");
            Session.SendMessage(Allowances);
            ServerMessage AchievementPoints = new ServerMessage(Outgoing.AchievementPoints);
            AchievementPoints.AppendInt32(Session.GetHabbo().AchievementScore);
            Session.SendMessage(AchievementPoints);
            Session.GetHabbo().method_12();
            ServerMessage SendAllowances = new ServerMessage(Outgoing.SendAllowances);
            SendAllowances.AppendString("2012-11-09 19:00,hstarsa;2012-11-30 12:00,");
            SendAllowances.AppendString("hstarsa");
            Session.SendMessage(SendAllowances);
            SendAllowances = new ServerMessage(Outgoing.SendAllowances);
            SendAllowances.AppendString("2012-11-09 15:00,hstarsbots;2012-11-16 18:00,diarare;2012-11-19 12:00,xmasghost1;2012-11-22 20:00,xmasghost2;2012-11-22 20:45,xmasghost1;2012-11-25 21:00,xmasghost2;2012-11-25 21:45,xmasghost1;2012-11-28 22:00,xmasghost2;2012-11-28 22:45,xmasghost1;2012-11-30 14:00,");
            SendAllowances.AppendString("xmasghost1");
            Session.SendMessage(SendAllowances);
            SendAllowances = new ServerMessage(Outgoing.SendAllowances);
            SendAllowances.AppendString("2012-11-23 18:00,hstarssubmit2;2012-11-26 11:00,;2012-11-26 14:00,hstarsvote2;2012-11-28 11:00,");
            SendAllowances.AppendString("hstarsvote2");
            Session.SendMessage(SendAllowances);
            SendAllowances = new ServerMessage(Outgoing.SendAllowances);
            SendAllowances.AppendString("2012-11-09 18:00,hspeedway;2012-11-15 15:00,hstarsdiamonds;2012-11-30 12:00,");
            SendAllowances.AppendString("hstarsdiamonds");
            Session.SendMessage(SendAllowances);
            SendAllowances = new ServerMessage(Outgoing.SendAllowances);
            SendAllowances.AppendString("");
            SendAllowances.AppendString("");
            Session.SendMessage(SendAllowances);
            if (!Session.GetHabbo().PassedHabboWayQuiz)
            {
                ServerMessage CitizenshipPanel = new ServerMessage(Outgoing.CitizenshipPanel);
                CitizenshipPanel.AppendString("citizenship");
                CitizenshipPanel.AppendInt32(1);
                CitizenshipPanel.AppendInt32(4);
                Session.SendMessage(CitizenshipPanel);
            }
                if(!Session.IsMobileUser)
                    Session.LoadRoom(Session.GetHabbo().HomeRoomId);
		}
	}
}
