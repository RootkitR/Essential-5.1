using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
using Essential.Util;
namespace Essential.Communication.Messages.Navigator
{
    internal sealed class ToggleStaffPickMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session.GetHabbo().Rank > 6)
            {
                Room Room = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

                int AlreadyStaffPicks;
                AlreadyStaffPicks = 0;

                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    if (dbClient.ReadDataRow("SELECT * FROM navigator_publics_new WHERE room_id = '" + Room.Id + "'") != null)
                    {
                        AlreadyStaffPicks = 1;
                    }
                }


                if (AlreadyStaffPicks == 0)
                {
                    string Owner;
                    int OwnerID;
                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        Owner = Room.Owner;
                        int ordernum = dbClient.ReadInt32("SELECT COUNT(*) FROM navigator_publics_new WHERE category_parent_id=6") + 1;
                        dbClient.AddParamWithValue("roomname", Room.Name);
                        //would be bad if this exploit still exists.. nah?
                        dbClient.ExecuteQuery("INSERT INTO `navigator_publics_new` (`ordernum`,`bannertype`, `caption`, `description`, `image`, `image_type`, `room_id`, `category_id`, `category_parent_id`, `enabled`, `typeofdata`) VALUES (" + ordernum + ",'0', @roomname, '','officalrooms_hq/staffempfehlungen.gif','internal',"+ Room.Id + ",6,6,'1',3)");
                    }

                    GameClient RoomOwner = Essential.GetGame().GetClientManager().GetClientByHabbo(Owner);
                    if (RoomOwner != null)
                    {
                        RoomOwner.GetHabbo().StaffPicks++;
                        RoomOwner.GetHabbo().CheckStaffPicksAchievement();
                    }
                    else
                    {
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            try
                            {
                                OwnerID = dbClient.ReadInt32("SELECT id FROM users WHERE username = '" + Owner + "'");
                                dbClient.ExecuteQuery("UPDATE user_stats SET staff_picks = staff_picks + 1 WHERE id = '" + OwnerID + "' LIMIT 1");
                            }
                            catch
                            {
                                Session.SendNotification("Es ist ein Fehler aufgetaucht: ToggleStaffPickMessageEvent:50!");
                            }
                        }
                    }

                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        Essential.GetGame().GetNavigator().Initialize(dbClient);
                    }

                    Session.SendNotification("Raum wurde erfolgreich zu den Staffpicks hinzugefügt.");

                }
                else
                {
                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {

                        dbClient.ExecuteQuery("DELETE FROM `navigator_publics_new` WHERE (`room_id`='" + Room.Id + "')");
                    }

                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        Essential.GetGame().GetNavigator().Initialize(dbClient);
                    }

                    Session.SendNotification("Raum wurde von den Staffpicks entfernt.");
                }
            }
        }
    }
}
