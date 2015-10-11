using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using System.Data;
using Essential.Storage;
using System.Collections.Generic;

namespace Essential.HabboHotel.Users.Stream
{

        internal sealed class Stream
        {
            private GameClient gc;
            private List<StreamEntry> ESEntry;
            public Stream()
            {
                ESEntry = new List<StreamEntry>();
            }
          /*  public ServerMessage GetEntries(DataTable EntriesTable, GameClient Session)
            {
                gc = Session;
                List<StreamEntry> Entries = new List<StreamEntry>();
                ServerMessage Message = new ServerMessage(Outgoing.EventStream);
                Dictionary<uint, int> ContainsID = new Dictionary<uint, int>();
                uint virtualidcounter = 0;
                foreach (DataRow Entry in EntriesTable.Rows)
                {
                    if (!ContainsID.ContainsKey((uint)Entry["id"]))
                    {
                        virtualidcounter = virtualidcounter + 1;
                        DataRow UserRow = GetUserRow(Entry["userid"].ToString());
                        Entries.Add(new StreamEntry((int)Entry["id"], (int)virtualidcounter, Entry["userid"].ToString(), (string)UserRow["username"], UserRow["gender"].ToString().ToLower(), (string)UserRow["look"], GetMinutes((double)Entry["time"]), Convert.ToInt32(Entry["data_extra"].ToString()), GetEntryLiked((int)Entry["id"], false), (string)Entry["data"]));
                        ContainsID.Add((uint)Entry["id"], 1);
                    }
                }
                Message.AppendInt32(Entries.Count);
                this.ESEntry = Entries;
                return AppendEntryOnServerMessage(Message, Entries);
            }*/
            public ServerMessage GetEntries(DataTable EntriesTable, GameClient Session)
            {
                gc = Session;
                List<StreamEntry> Entries = new List<StreamEntry>();
                ServerMessage Message = new ServerMessage(Outgoing.EventStream);
                Dictionary<int,int> ids = new Dictionary<int,int>();
                int virtualIdCounter = 0;
                foreach (DataRow Entry in EntriesTable.Rows)
                {
                    try
                    {
                        int id = int.Parse(Entry["id"].ToString());
                        if (!ids.ContainsKey(id))
                        {
                            virtualIdCounter++;
                            DataRow userRow = GetUserRow(Entry["userid"].ToString());

                            Entries.Add(new StreamEntry(id, virtualIdCounter, Entry["userid"].ToString(), (string)userRow["username"], userRow["gender"].ToString().ToLower(), (string)userRow["look"], GetMinutes((double)Entry["time"]), Convert.ToInt32(Entry["data_extra"].ToString()), GetEntryCanLike(id, false), (string)Entry["data"]));
                            ids.Add(id, 1);
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
                Message.AppendInt32(Entries.Count);
                this.ESEntry = Entries;
                return AppendEntryOnServerMessage(Message, Entries);
            }
            public ServerMessage AppendEntryOnServerMessage(ServerMessage Message, List<StreamEntry> Entries)
            {
                foreach (StreamEntry Entry in Entries)
                {
                    Message.AppendInt32(Entry.VirtualID);
                    Message.AppendInt32(5);
                    Message.AppendStringWithBreak(Entry.UserID);
                    Message.AppendStringWithBreak(Entry.Username);
                    Message.AppendStringWithBreak(Entry.Gender);
                    Message.AppendStringWithBreak(Essential.HeadImagerURL + Entry.Look);
                    Message.AppendInt32(Entry.Time);
                    Message.AppendInt32(0);
                    Message.AppendInt32(Entry.Likes);
                    Message.AppendBoolean(Entry.Liked);
                    Message.AppendBoolean(Entry.Liked);
                    Message.AppendBoolean(Entry.Liked);
                    Message.AppendStringWithBreak(Entry.Text);

                    //Console.WriteLine(Entry.VirtualID + " " + Entry.UserID + " " + Entry.Username + " " + Entry.Gender + " " + Entry.Look + " " + Entry.Time + " " + Entry.Likes + " " + Entry.Liked + " " + Entry.Text);
                }
                return Message;
            }

            public DataRow GetUserRow(string UserID)
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    return dbClient.ReadDataRow("SELECT username,look,gender FROM users WHERE id='" + UserID + "' LIMIT 1");
                }

            }
            public int GetMinutes(double TimeStamp)
            {
                return Convert.ToInt32(Math.Round((Essential.GetUnixTimestamp() - TimeStamp) / 60));
            }
            public int GetIdByVirtualId(int virtualID)
            {
                foreach (StreamEntry ese in ESEntry)
                {
                    if (ese.VirtualID == virtualID)
                    {
                        return ese.ID;
                    }
                }
                return 0;
            }
            
            public bool GetEntryCanLike(int EntryID, bool useVirtualID)
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    int id = EntryID;
                    if (useVirtualID)
                        id = GetIdByVirtualId(EntryID);
                    DataRow Liked = dbClient.ReadDataRow("SELECT userid FROM friend_stream_likes WHERE entryid='" + id + "' AND userid='" + gc.GetHabbo().Id + "'");
                    return Liked != null ? false : true;
                }
            }
            public void LikeStreamEntry(int id, int userid)
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    int entryid = GetIdByVirtualId((int)id);
                    if (GetEntryCanLike((int)id, true))
                    {
                        dbClient.ExecuteQuery("UPDATE friend_stream SET data_extra = data_extra + 1 WHERE id=" + entryid);
                        dbClient.ExecuteQuery("INSERT INTO friend_stream_likes (entryid, userid) VALUES ('" + entryid + "', '" + userid + "')");
                    }
                    else
                    {
                        gc.SendNotification("Du hast diesen Beitrag bereits mit \"Gefällt mir\" markiert.");
                    }
                }
            }
        }
    

}
