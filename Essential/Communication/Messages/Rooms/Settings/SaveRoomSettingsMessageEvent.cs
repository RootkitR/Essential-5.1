using System;
using System.Collections.Generic;
using System.Text;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.HabboHotel.Navigators;
using Essential.Storage;
namespace Essential.Communication.Messages.Rooms.Settings
{
	internal sealed class SaveRoomSettingsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Room room = Session.GetHabbo().CurrentRoom;
            RoomData roomData = room.RoomData;
            if ((room != null) && room.CheckRights(Session, true))
            {
                int num = Event.PopWiredInt32();
                string str = Essential.FilterString(Event.PopFixedString());
                string str2 = Essential.FilterString(Event.PopFixedString());
                int num2 = Event.PopWiredInt32();
                string str3 = Essential.FilterString(Event.PopFixedString());
                int num3 = Event.PopWiredInt32();
                int id = Event.PopWiredInt32();
                int num5 = Event.PopWiredInt32();
                List<string> tags = new List<string>();
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < num5; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(",");
                    }
                    string item = Essential.FilterString(Event.PopFixedString().ToLower());
                    tags.Add(item);
                    builder.Append(item);
                }
                int num7 = Event.PopWiredInt32();
                bool k = Event.PopWiredBoolean();
                bool flag2 = Event.PopWiredBoolean();
                bool flag3 = Event.PopWiredBoolean();
                bool flag4 = Event.PopWiredBoolean();
                int num8 = Event.PopWiredInt32();
                int num9 = Event.PopWiredInt32();
                if ((num8 < -2) || (num8 > 1))
                {
                    num8 = 0;
                }
                if ((num9 < -2) || (num9 > 1))
                {
                    num9 = 0;
                }
                if (((str.Length >= 1) && ((num2 >= 0) && (num2 <= 2))) && ((num3 >= 10) && (num3 <= 500)))
                {
                    FlatCat flatCat = Essential.GetGame().GetNavigator().GetFlatCat(id);
                    if (flatCat != null)
                    {
                        if (flatCat.MinRank > Session.GetHabbo().Rank)
                        {
                            Session.SendNotification("Du hast nicht die dafür vorgegebene Rechte!");
                            id = 0;
                        }
                            room.AllowPet = k;
                            room.AllowPetsEating = flag2;
                            room.AllowWalkthrough = flag3;
                            room.Hidewall = flag4;
                            room.RoomData.AllowPet = k;
                            room.RoomData.AllowPetsEating = flag2;
                            room.RoomData.AllowWalkthrough = flag3;
                            room.RoomData.Hidewall = flag4;
                            room.Name = str;
                            room.State = num2;
                            room.Description = str2;
                            room.Category = id;
                            room.Password = str3;
                            room.RoomData.Name = str;
                            room.RoomData.State = num2;
                            room.RoomData.Description = str2;
                            room.RoomData.Category = id;
                            room.RoomData.Password = str3;
                            room.Tags.Clear();
                            room.Tags.AddRange(tags);
                            room.UsersMax = num3;
                            room.RoomData.Tags.Clear();
                            room.RoomData.Tags.AddRange(tags);
                            room.RoomData.UsersMax = num3;
                            room.Wallthick = num8;
                            room.Floorthick = num9;
                            room.RoomData.Wallthick = num8;
                            room.RoomData.Floorthick = num9;
                            string str5 = "open";
                            if (room.State == 1)
                            {
                                str5 = "locked";
                            }
                            else if (room.State > 1)
                            {
                                str5 = "password";
                            }
                            using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
                            {
                                adapter.AddParamWithValue("caption", room.Name);
                                adapter.AddParamWithValue("description", room.Description);
                                adapter.AddParamWithValue("password", room.Password);
                                adapter.AddParamWithValue("tags", builder.ToString());
                                adapter.ExecuteQuery(string.Concat(new object[] { 
                                    "UPDATE rooms SET caption = @caption, description = @description, password = @password, category = '", id, "', state = '", str5, "', tags = @tags, users_max = '", num3, "', allow_pets = '", (k ? 1 : 0), "', allow_pets_eat = '", (flag2 ? 1 : 0), "', allow_walkthrough = '", (flag3 ? 1 : 0), "', allow_hidewall = '", (room.Hidewall ? 1 : 0), "', floorthick = '", room.Floorthick, 
                                    "', wallthick = '", room.Wallthick, "' WHERE id = ", room.Id
                                 }));
                            }

                            ServerMessage UpdateRoomOne = new ServerMessage(Outgoing.UpdateRoomOne);
                            UpdateRoomOne.AppendInt32(room.Id);
                            Session.SendMessage(UpdateRoomOne);

                            ServerMessage WallAndFloor = new ServerMessage(Outgoing.ConfigureWallandFloor);
                            WallAndFloor.AppendBoolean(room.Hidewall);
                            WallAndFloor.AppendInt32(room.Wallthick);
                            WallAndFloor.AppendInt32(room.Floorthick);
                            Session.GetHabbo().CurrentRoom.SendMessage(WallAndFloor,null);

                            RoomData data2 = room.RoomData;

                            ServerMessage RoomDataa = new ServerMessage(Outgoing.RoomData);
                            RoomDataa.AppendBoolean(false);
                            RoomDataa.AppendInt32(room.Id);
                            RoomDataa.AppendString(room.Name);
                            RoomDataa.AppendBoolean(true);
                            RoomDataa.AppendInt32(room.OwnerId);
                            RoomDataa.AppendString(room.Owner);
                            RoomDataa.AppendInt32(room.State);
                            RoomDataa.AppendInt32(room.UsersNow);
                            RoomDataa.AppendInt32(room.UsersMax);
                            RoomDataa.AppendString(room.Description);
                            RoomDataa.AppendInt32(0);
                            RoomDataa.AppendInt32((room.Category == 0x34) ? 2 : 0);
                            RoomDataa.AppendInt32(room.Score);
                            RoomDataa.AppendInt32(0);
                            RoomDataa.AppendInt32(room.Category);

                            if (room.RoomData.GuildId == 0)
                            {
                                RoomDataa.AppendInt32(0);
                                RoomDataa.AppendInt32(0);
                            }
                            else
                            {
                                GroupsManager guild = Groups.GetGroupById(room.RoomData.GuildId);
                                RoomDataa.AppendInt32(guild.Id);
                                RoomDataa.AppendString(guild.Name);
                                RoomDataa.AppendString(guild.Badge);
                            }

                            RoomDataa.AppendString("");
                            RoomDataa.AppendInt32(room.Tags.Count);

                            foreach (string str6 in room.Tags)
                            {
                                RoomDataa.AppendString(str6);
                            }

                            RoomDataa.AppendInt32(0);
                            RoomDataa.AppendInt32(0);
                            RoomDataa.AppendInt32(0);
                            RoomDataa.AppendBoolean(true);
                            RoomDataa.AppendBoolean(true);
                            RoomDataa.AppendInt32(0);
                            RoomDataa.AppendInt32(0);
                            RoomDataa.AppendBoolean(false);
                            RoomDataa.AppendBoolean(false);
                            RoomDataa.AppendBoolean(false);

                            RoomDataa.AppendInt32(0);
                            RoomDataa.AppendInt32(0);
                            RoomDataa.AppendInt32(0);
                            RoomDataa.AppendBoolean(false);
                            RoomDataa.AppendBoolean(true);
                            Session.GetHabbo().CurrentRoom.SendMessage(RoomDataa,null);
                    }
                }
            }
        }
		
	}
}
