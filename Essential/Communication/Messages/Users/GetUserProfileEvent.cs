using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using System.Data;
using System.Collections.Generic;
namespace Essential.Communication.Messages.Users
{
    internal sealed class GetUserProfileEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            try
            {
                int num = Event.PopWiredInt32();
                bool flag = Event.PopWiredBoolean();
                DataRow habbo = null;
                DataRow userStats = null;
                DataTable userGroups = null;
                int Friends = 0;
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    habbo = dbClient.ReadDataRow("SELECT * FROM users WHERE Id=" + num + " LIMIT 1");
                    userStats = dbClient.ReadDataRow("SELECT * FROM user_stats WHERE id=" + num + " LIMIT 1");
                    userGroups = dbClient.ReadDataTable("SELECT groupid FROM group_memberships WHERE userid=" + num);
                    Friends = dbClient.ReadInt32("SELECT COUNT(*) FROM messenger_friendships WHERE user_one_id=" + num);
                }

                if (habbo == null)
                {
                    Session.SendNotification("No Data found for User: " + num);
                }
                else
                {
                        DataRow Info;
                        TimeSpan span = (TimeSpan)(DateTime.Now - UnixTimeStampToDateTime(double.Parse((string)habbo["last_online"])));
                        List<GroupsManager> list = new List<GroupsManager>();
                        foreach(DataRow guild2 in userGroups.Rows)
                        {
                            if (!list.Contains(Groups.GetGroupById((int)guild2["groupid"])))
                                list.Add(Groups.GetGroupById((int)guild2["groupid"]));
                        }
                        ServerMessage Response = new ServerMessage(Outgoing.ProfileInformation);
                        Response.AppendUInt((uint)habbo["id"]);
                        Response.AppendString((string)habbo["username"]);
                        Response.AppendString((string)habbo["look"]);
                        Response.AppendString((string)habbo["motto"]);
                        Response.AppendString(UnixTimeStampToDateTime(double.Parse((string)habbo["account_created"])).ToShortDateString());
                        Response.AppendInt32((int)userStats["achievementScore"]);
                        Response.AppendInt32(Friends);
                        Response.AppendBoolean(num != Session.GetHabbo().Id);
                        Response.AppendBoolean(false);
                        Response.AppendBoolean(Essential.GetGame().GetClientManager().GetClient((uint)num) != null);
                        Response.AppendInt32(list.Count);
                        int idk = 0;
                        foreach (GroupsManager guild in list)
                        {
                            if (guild != null)
                            {
                                Response.AppendInt32(guild.Id);
                                Response.AppendString(guild.Name);
                                Response.AppendString(guild.Badge);
                                Response.AppendString(guild.ColorOne);
                                Response.AppendString(guild.ColorTwo);
                                Response.AppendBoolean((int)userStats["groupid"] == guild.Id);
                            }
                            else
                            {
                                Response.AppendInt32(idk);
                                Response.AppendString("Undefined");
                                Response.AppendString("");
                                Response.AppendString("bf2222");
                                Response.AppendString("bf2222");
                                Response.AppendBoolean(false);
                            }
                            idk++;
                        }
                        Response.AppendBoolean(false);
                        Response.AppendInt32((int)span.TotalSeconds);//(int)span.TotalSeconds);
                        Response.AppendBoolean(true);
                        Session.SendMessage(Response);
                    }
            }
            catch(Exception ex)
            {
               /* Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;*/
            }
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
