using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System.Collections.Generic;
namespace Essential.Communication.Messages.TalentTrack
{
    internal sealed class GetTalentTrack : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            #region "citizenship"
            bool flag = false;
            Dictionary<uint, int> dictionary1 = new Dictionary<uint, int>();
            List<string> badge = new List<string>();
            badge.Add("ACH_AvatarLooks1");
            dictionary1.Add(1,1);
            badge.Add("ACH_RespectGiven1");
            dictionary1.Add(4,1);
            badge.Add("ACH_AllTimeHotelPresence1");
            dictionary1.Add(15,2);
            badge.Add("ACH_RoomEntry1");
            dictionary1.Add(8,1);
            if (Essential.GetGame().GetAchievementManager().HasAchievements(dictionary1,Session) || Session.GetHabbo().GetBadgeComponent().HasBadgeList(badge))
                flag = true;
            bool flag2 = false;
            Dictionary<uint,int> dictionary2 = new Dictionary<uint, int>();
            List<string> list2 = new List<string>();
            list2.Add("ACH_RegistrationDuration1");
            dictionary2.Add(17, 1);
            list2.Add("ACH_AllTimeHotelPresence2");
            dictionary2.Add(15,2);
            list2.Add("ACH_RoomEntry2");
            dictionary2.Add(8,2);
            if (Essential.GetGame().GetAchievementManager().HasAchievements(dictionary2,Session) || Session.GetHabbo().GetBadgeComponent().HasBadgeList(list2))
                flag2 = true;
            bool flag3 = false;
            Dictionary<uint, int> dictionary3 = new Dictionary<uint, int>();
            List<string> list3 = new List<string>();
            list3.Add("ACH_RegistrationDuration2");
            dictionary3.Add(17,2);
            list3.Add("ACH_AllTimeHotelPresence3");
            dictionary3.Add(15,3);
            list3.Add("ACH_RoomEntry3");
            dictionary3.Add(8,3);
            if (Essential.GetGame().GetAchievementManager().HasAchievements(dictionary3,Session) || Session.GetHabbo().GetBadgeComponent().HasBadgeList(list3))
                flag3 = true;
            //if (!flag3 || !flag2 || !flag || !Session.GetHabbo().PassedSafetyQuiz || !Session.GetHabbo().PassedHabboWayQuiz)
            //{
                ServerMessage message = new ServerMessage(Outgoing.TalentTrack);
                message.AppendString(Event.PopFixedString());
                message.AppendInt32(5);
                message.AppendInt32(0);
                message.AppendInt32(Session.GetHabbo().PassedHabboWayQuiz ? 2 : 1);
                message.AppendInt32(1);
                message.AppendInt32(0x7d);
                message.AppendInt32(1);
                message.AppendString("ACH_SafetyQuizGraduate1");
                message.AppendInt32(Session.GetHabbo().PassedSafetyQuiz ? 2 : 1);
                message.AppendInt32(Session.GetHabbo().PassedSafetyQuiz ? 1 : 0);
                message.AppendInt32(1);
                message.AppendInt32(0);
                message.AppendInt32(1);
                message.AppendString("A1 KUMIANKKA");
                message.AppendInt32(0);
                message.AppendInt32(1);
                message.AppendInt32(flag ? 2 : (Session.GetHabbo().PassedSafetyQuiz ? 1 : 0));
                message.AppendInt32(4);
                message.AppendInt32(6);
                message.AppendInt32(1);
                message.AppendString("ACH_AvatarLooks1");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 1, 1) || Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AvatarLooks1")) ? 2 : (Session.GetHabbo().PassedSafetyQuiz ? 1 : 0));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevel(1, Session));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevels(1));
                message.AppendInt32(0x12);
                message.AppendInt32(1);
                message.AppendString("ACH_RespectGiven1");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 4, 1) || Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RespectGiven1")) ? 2 : (Session.GetHabbo().PassedSafetyQuiz ? 1 : 0));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevel(4, Session));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevels(4));
                message.AppendInt32(0x13);
                message.AppendInt32(1);
                message.AppendString("ACH_AllTimeHotelPresence1");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 15, 1) || Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AllTimeHotelPresence1")) ? 2 : (Session.GetHabbo().PassedSafetyQuiz ? 1 : 0));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevel(15, Session));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevels(15));
                message.AppendInt32(8);
                message.AppendInt32(1);
                message.AppendString("ACH_RoomEntry1");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 8, 1) || Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RoomEntry1")) ? 2 : (Session.GetHabbo().PassedSafetyQuiz ? 1 : 0));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevel(8, Session));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevels(8));
                message.AppendInt32(0);
                message.AppendInt32(1);
                message.AppendString("A1 KUMIANKKA");
                message.AppendInt32(flag2 ? 2 : (flag ? 1 : 0));
                message.AppendInt32(2);
                message.AppendInt32(0);
                message.AppendInt32(3);
                message.AppendInt32(11);
                message.AppendInt32(1);
                message.AppendString("ACH_RegistrationDuration1");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 17, 1) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RegistrationDuration1") && flag)) ? 2 : (flag ? 1 : 0));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevel(17, Session));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevels(17));
                message.AppendInt32(0x13);
                message.AppendInt32(2);
                message.AppendString("ACH_AllTimeHotelPresence2");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 15, 2) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AllTimeHotelPresence2") && flag)) ? 2 : (flag ? 1 : 0));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevel(15, Session));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevels(15));
                message.AppendInt32(8);
                message.AppendInt32(2);
                message.AppendString("ACH_RoomEntry2");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 8, 2) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RoomEntry2") && flag)) ? 2 : (flag ? 1 : 0));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevel(8, Session));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevels(8));
                message.AppendInt32(0);
                message.AppendInt32(1);
                message.AppendString("A1 KUMIANKKA");
                message.AppendInt32(flag3 ? 2 : (flag2 ? 1 : 0));
                message.AppendInt32(3);
                message.AppendInt32(0);
                message.AppendInt32(4);
                message.AppendInt32(11);
                message.AppendInt32(2);
                message.AppendString("ACH_RegistrationDuration2");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 17, 2) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RegistrationDuration2") && flag2)) ? 2 : (flag2 ? 1 : 0));
                message.AppendInt32(0);
                message.AppendInt32(3);
                message.AppendInt32(0x5e);
                message.AppendInt32(1);
                message.AppendString("ACH_HabboWayGraduate1");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 23, 1) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_HabboWayGraduate1") && flag2)) ? 2 : (flag2 ? 1 : 0));
                message.AppendInt32(0);
                message.AppendInt32(1);
                message.AppendInt32(0x13);
                message.AppendInt32(3);
                message.AppendString("ACH_AllTimeHotelPresence3");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 15, 3) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AllTimeHotelPresence3") && flag)) ? 2 : (flag ? 1 : 0));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevel(15, Session));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevels(15));
                message.AppendInt32(0x8e);
                message.AppendInt32(1);
                message.AppendString("ACH_FriendListSize1");
                message.AppendInt32((Essential.GetGame().GetAchievementManager().HasAchievement(Session, 24, 1) || (Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_FriendListSize1") && flag2)) ? 2 : (flag2 ? 1 : 0));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevel(24,Session));
                message.AppendInt32(Essential.GetGame().GetAchievementManager().GetLevels(24));
                message.AppendInt32(1);
                message.AppendString("TRADE");
                message.AppendInt32(1);
                message.AppendString("A1 KUMIANKKA");
                message.AppendInt32(flag3 ? 1 : 0);
                message.AppendInt32(4);
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendInt32(1);
                message.AppendString("CITIZEN");
                message.AppendInt32(2);
                message.AppendString("A1 KUMIANKKA");
                message.AppendInt32(0);
                message.AppendString("HABBO_CLUB_CITIZENSHIP_VIP_REWARD");
                message.AppendInt32(7);
                Session.SendMessage(message);
            //}
                #endregion
            #region "helper"
            /*ServerMessage HelperTrack = new ServerMessage(Outgoing.TalentTrack);
            HelperTrack.AppendString("helper");
            HelperTrack.AppendInt32(0);
            Session.SendMessage(HelperTrack);*/
            #endregion
        }
    }
}
