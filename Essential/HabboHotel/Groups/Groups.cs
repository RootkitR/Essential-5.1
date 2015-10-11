using System;
using System.Collections.Generic;
using System.Data;
using Essential.Core;
using Essential.Storage;
using Essential.Messages;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel;
using System.Text;
namespace Essential
{
	internal sealed class Groups
	{
		public static Dictionary<int, GroupsManager> GroupsManager;
		public Groups()
		{
			Groups.GroupsManager = new Dictionary<int, GroupsManager>();
		}
        public static GroupsManager GetRoomGroup(uint RoomId)
        {
            foreach (GroupsManager Managerr in Groups.GroupsManager.Values)
            {
                if (Managerr.RoomId == (int)RoomId)
                {
                    return Managerr;
                }
            }
            return null;
        }
		public static void Initialize(DatabaseClient class6_0)
		{
            Logging.Write("Loading groups...");
			Groups.ClearGroups();
			DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM groups;");
			foreach (DataRow dataRow in dataTable.Rows)
			{
				Groups.GroupsManager.Add((int)dataRow["Id"], new GroupsManager((int)dataRow["Id"], dataRow, class6_0));
			}
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}
		public static void ClearGroups()
		{
			Groups.GroupsManager.Clear();
		}
        public static void RemoveGroup(GroupsManager guild)
        {
            using(DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery("DELETE FROM groups WHERE id=" + guild.Id);
                dbClient.ExecuteQuery("DELETE FROM group_memberships WHERE groupid=" + guild.Id);
                dbClient.ExecuteQuery("DELETE FROM group_requests WHERE groupid=" + guild.Id);
                dbClient.ExecuteQuery("UPDATE user_stats SET groupid=0 WHERE groupid=" + guild.Id);
                //dbClient.ExecuteQuery("UPDATE items SET guild_data='0;0;0' WHERE guild_data LIKE '" + guild.Id + ";%'");
                foreach(GameClient gc in Essential.GetGame().GetClientManager().GetClients())
                {
                    if(gc != null && gc.GetHabbo() != null&& gc.GetHabbo().InGuild(guild.Id))
                    {
                        if (gc.GetHabbo().FavouriteGroup == guild.Id)
                            gc.GetHabbo().FavouriteGroup = 0;
                        gc.GetHabbo().RefreshGuilds();
                    }
                }
                Groups.GroupsManager.Remove(guild.Id);
            }
        }
		public static GroupsManager GetGroupById(int int_0)
		{
			if (Groups.GroupsManager.ContainsKey(int_0))
			{
				return Groups.GroupsManager[int_0];
			}
			else
			{
				return null;
			}
		}
		public static void UpdateGroup(DatabaseClient class6_0, int int_0)
		{
			GroupsManager @class = Groups.GetGroupById(int_0);
			if (@class != null)
			{
				DataRow Row = class6_0.ReadDataRow("SELECT * FROM groups WHERE Id = " + int_0 + " LIMIT 1");
				@class.Name = (string)Row["name"];
                @class.Badge = (string)Row["badge"];
                @class.RoomId = (uint)Row["roomid"];
				@class.Description = (string)Row["desc"];
                @class.Locked = (string)Row["locked"];
                @class.ColorOne = (string)Row["color_one"];
                @class.ColorTwo = (string)Row["color_two"];
                @class.Created = Essential.TimestampToDate(double.Parse((string)Row["created"])).ToShortDateString();
				@class.Members.Clear();
				DataTable dataTable = class6_0.ReadDataTable("SELECT userid FROM group_memberships WHERE groupid = " + int_0 + ";");
				foreach (DataRow dataRow2 in dataTable.Rows)
				{

                    @class.JoinGroup((int)dataRow2["userid"]);
				}
			}
		}
        public static string GenerateGuildImage(int GuildBase, int GuildBaseColor, List<int> GStates)
        {
            List<int> list = GStates;
            string str = "";
            int num = 0;
            string str2 = "b";
            if (GuildBase.ToString().Length >= 2)
            {
                str2 = str2 + GuildBase;
            }
            else
            {
                str2 = str2 + "0" + GuildBase;
            }
            str = GuildBaseColor.ToString();
            if (str.Length >= 2)
            {
                str2 = str2 + str;
            }
            else if (str.Length <= 1)
            {
                str2 = str2 + "0" + str;
            }
            int num2 = 0;
            if (list[9] != 0)
            {
                num2 = 4;
            }
            else if (list[6] != 0)
            {
                num2 = 3;
            }
            else if (list[3] != 0)
            {
                num2 = 2;
            }
            else if (list[0] != 0)
            {
                num2 = 1;
            }
            int num3 = 0;
            for (int i = 0; i < num2; i++)
            {
                str2 = str2 + "s";
                num = list[num3] - 20;
                if (num.ToString().Length >= 2)
                {
                    str2 = str2 + num;
                }
                else
                {
                    str2 = str2 + "0" + num;
                }
                int num5 = list[1 + num3];
                str = num5.ToString();
                if (str.Length >= 2)
                {
                    str2 = str2 + str;
                }
                else if (str.Length <= 1)
                {
                    str2 = str2 + "0" + str;
                }
                str2 = str2 + list[2 + num3].ToString();
                switch (num3)
                {
                    case 0:
                        num3 = 3;
                        break;

                    case 3:
                        num3 = 6;
                        break;

                    case 6:
                        num3 = 9;
                        break;
                }
            }
            return str2;
        }
        /*public static string GenerateGuildImage(int guildBase, int guildBaseColor, List<int> states)
        {
            var image = new StringBuilder(String.Format("b{0:00}{1:00}", guildBase, guildBaseColor));
            for (var i = 0; i < 3 * 4; i += 3)
                image.Append(i >= states.Count ? "s" : String.Format("s{0:00}{1:00}{2}", states[i], states[i + 1], states[i + 2]));
            return image.ToString();
        }*/
        public static string GetHtmlColor(int Color)
        {
            try
            {
                using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
                {
                    return adapter.ReadString("SELECT ExtraData1 FROM groups_elements WHERE id = '" + Color + "' AND Type = 'Color3'");
                }
            }
            catch
            {

            }
            return "";
        }
        public static int GetColorByHTMLColor(string htmlcolor)
        {
            try
            {
                using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
                {
                    return adapter.ReadInt32("SELECT Id FROM groups_elements WHERE ExtraData1 = '" + htmlcolor + "' AND Type = 'Color3' LIMIT 1");
                }
            }
            catch
            {

            }
            return 0;
        }
        public static GroupsManager AddGuild(int id, string name, int ownerid, string ownername, string description, int roomid, string image, int customcolor1, int customcolor2, int guildbase, int guildbasecolor, List<int> guildstates, string htmlcolor1, string htmlcolor2, string datecreated, Dictionary<int, string> members, List<int> petitions, int type, int rightstype)
        {
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                try
                {
                    try
                    {
                        id = dbClient.ReadInt32("SELECT ID FROM groups ORDER BY ID DESC LIMIT 1") + 1;
                    }
                    catch
                    {
                        id = 1;
                    }
                    string str = "";
                    foreach (int num in guildstates)
                    {
                        str = str + num + ";";
                    }
                    str = str.Substring(0, str.Length - 1);
                    dbClient.AddParamWithValue("id", id);
                    dbClient.AddParamWithValue("name", name);
                    dbClient.AddParamWithValue("ownerid", ownerid);
                    dbClient.AddParamWithValue("description", description);
                    dbClient.AddParamWithValue("roomid", roomid);
                    dbClient.AddParamWithValue("image", image);
                    dbClient.AddParamWithValue("htmlcolor1", htmlcolor1);
                    dbClient.AddParamWithValue("htmlcolor2", htmlcolor2);
                    dbClient.AddParamWithValue("datecreated", datecreated);
                    dbClient.AddParamWithValue("guildstates", str);
                    dbClient.AddParamWithValue("guildbase", guildbase);
                    dbClient.AddParamWithValue("guildbasecolor", guildbasecolor);
                    dbClient.ExecuteQuery("INSERT INTO groups (`id`,`name`,`ownerid`,`desc`,`roomid`,`badge`,`color_one`,`color_two`,`created`,`GuildStates`,`GuildBase`,`GuildBaseColor`) VALUES (@id, @name, @ownerid, @description, @roomid, @image, @htmlcolor1, @htmlcolor2, @datecreated,@guildstates,@guildbase,@guildbasecolor);");
                    dbClient.ExecuteQuery("INSERT INTO group_memberships (groupid, userid) VALUES (" + id + "," + ownerid + ")");
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                    GroupsManager newGuild = new GroupsManager(id, dbClient.ReadDataRow("SELECT * FROM groups WHERE id=" + id + " LIMIT 1"), dbClient);
                Groups.GroupsManager.Add(id, newGuild);
                return newGuild;
            }
        }
        public static string IntToType(int i)
        {
            switch(i)
            {
                case 0:
                    return "open";
                case 1:
                    return "locked";
                case 2:
                    return "closed";
                default:
                    return "open";
            }
        }
        public static int TypeToInt(string t)
        {
            switch(t.ToLower())
            {
                case "open":
                    return 1;
                case "locked":
                    return 0;
                case "closed":
                    return 2;
                default:
                    return 1;
            }
        }
	}
}
