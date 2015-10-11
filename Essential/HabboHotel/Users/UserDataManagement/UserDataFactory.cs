using System;
using System.Data;
using Essential.Util;
using Essential.Storage;
using Essential.Core;
using System.Collections.Generic;
using Essential.HabboHotel.Users.Relationship;
using System.Linq;
namespace Essential.HabboHotel.Users.UserDataManagement
{
    internal sealed class UserDataFactory
    {
        public bool Validated;

        private DataRow UserData;
        private DataTable Achievements;
        private DataTable Favorites;
        private DataTable Ignores;
        private DataTable Tags;
        private DataTable Subscriptions;
        private DataTable Badges;
        private DataTable Items;
        private DataTable Effects;
        private DataTable Friends;
        private DataTable FriendRequests;
        private DataTable Rooms;
        private DataTable Pets;
        private DataTable Bots;
        public double LoginTime;
        private List<Relationship.Relationship> relationships;
        public DataRow GetUserData()
        {
            return UserData;
        }

        public DataTable GetAchievements()
        {
            return Achievements;
        }

        public DataTable GetFavorites()
        {
            return Favorites;
        }

        public DataTable GetIgnores()
        {
            return Ignores;
        }

        public DataTable GetTags()
        {
            return Tags;
        }

        public DataTable GetSubscriptions()
        {
            return Subscriptions;
        }

        public DataTable GetBadges()
        {
            return Badges;
        }

        public DataTable GetItems()
        {
            return Items;
        }

        public DataTable GetEffects()
        {
            return Effects;
        }

        public DataTable GetFriends()
        {
            return Friends;
        }

        public DataTable GetFriendRequests()
        {
            return FriendRequests;
        }

        public DataTable GetRooms()
        {
            return Rooms;
        }

        public void SetRooms(DataTable table)
        {
            Rooms = table;
        }
       
        public DataTable GetBots()
        {
            return Bots;
        }
        
        public DataTable GetPets()
        {
            return Pets;
        }
        public List<Relationship.Relationship> GetRelationships()
        {
            return relationships;
        }

        public UserDataFactory(string ssoTicket, string ipAddress, bool getAllData)
        {
            if (string.IsNullOrEmpty(ssoTicket))
            {
                this.Validated = false;
            }
            else
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    dbClient.AddParamWithValue("auth_ticket", ssoTicket);
                    string str = "";
                    try
                    {
                        if (int.Parse(Essential.GetConfig().data["debug"]) == 1)
                        {
                            str = "";
                        }
                    }
                    catch { }

                    this.UserData = dbClient.ReadDataRow("SELECT * FROM users WHERE auth_ticket = @auth_ticket " + str + " LIMIT 1;");

                    if (this.UserData != null)
                    {
                        this.Validated = true;

                        uint id = (uint)this.UserData["Id"];

                        if (getAllData)
                        {
                            this.Achievements = dbClient.ReadDataTable("SELECT achievement_id,achievement_level FROM user_achievements WHERE user_id = '" + id + "'");
                            this.Favorites = dbClient.ReadDataTable("SELECT room_id FROM user_favorites WHERE user_id = '" + id + "'");
                            this.Ignores = dbClient.ReadDataTable("SELECT ignore_id FROM user_ignores WHERE user_id = '" + id + "'");
                            this.Tags = dbClient.ReadDataTable("SELECT tag FROM user_tags WHERE user_id = '" + id + "'");
                            this.Subscriptions = dbClient.ReadDataTable("SELECT subscription_id, timestamp_activated, timestamp_expire FROM user_subscriptions WHERE user_id = '" + id + "'");
                            this.Badges = dbClient.ReadDataTable("SELECT user_badges.badge_id,user_badges.badge_slot FROM user_badges WHERE user_id = " + id);
                            this.Items = dbClient.ReadDataTable("SELECT items.Id,items.base_item,items.ltd_id,items.ltd_cnt,items.guild_data,items_extra_data.extra_data FROM items LEFT JOIN items_extra_data ON items_extra_data.item_id = items.Id WHERE room_id = 0 AND user_id = " + id);
                            this.Effects = dbClient.ReadDataTable("SELECT user_effects.effect_id,user_effects.total_duration,user_effects.is_activated,user_effects.activated_stamp FROM user_effects WHERE user_id =  " + id);
                            this.Friends = dbClient.ReadDataTable("SELECT users.Id,users.username,users.motto,users.look,users.last_online FROM users JOIN messenger_friendships ON users.Id = messenger_friendships.user_two_id WHERE messenger_friendships.user_one_id = '" + id + "'");
                            this.FriendRequests = dbClient.ReadDataTable("SELECT messenger_requests.Id,messenger_requests.from_id,users.username,users.gender,users.look FROM users JOIN messenger_requests ON users.Id = messenger_requests.from_id WHERE messenger_requests.to_id = '" + id + "'");
                            
                            dbClient.AddParamWithValue("name", (string)this.UserData["username"]);

                            this.Rooms = dbClient.ReadDataTable("SELECT * FROM rooms WHERE owner = @name ORDER BY Id ASC LIMIT " + ServerConfiguration.RoomUserLimit);
                            this.Pets = dbClient.ReadDataTable("SELECT Id, user_id, room_id, name, type, race, color, expirience, energy, nutrition, respect, createstamp, x, y, z FROM user_pets WHERE user_id = " + id + " AND room_id = 0");
                            this.Bots = dbClient.ReadDataTable("SELECT * FROM user_bots WHERE owner_id = '" + id + "' AND room_id=0");

                            dbClient.ExecuteQuery(string.Concat(new object[]
                            {
                                "UPDATE users SET online = '1' WHERE Id = '",
                                id,
                                "' LIMIT 1; UPDATE user_info SET login_timestamp = '",
                                Essential.GetUnixTimestamp(),
                                "' WHERE user_id = '",
                                id,
                                "' LIMIT 1;"
                            }));
                            double loginStamp = Essential.GetUnixTimestamp();
                            this.LoginTime = loginStamp;
                            dbClient.ExecuteQuery("INSERT INTO user_clientsessions (`userId`,`login_timestamp`,`logout_timestamp`) VALUES ('" +  id + "','" + loginStamp + "','0');");
                            //    @class.ExecuteQuery(string.Concat(new object[]
                            //{
                            //    "UPDATE users SET online = '1'" + /*auth_ticket = ''*/ "WHERE Id = '",
                            //    num,
                            //    "' LIMIT 1; UPDATE user_info SET login_timestamp = '",
                            //    Essential.GetUnixTimestamp(),
                            //    "' WHERE user_id = '",
                            //    num,
                            //    "' LIMIT 1;"
                            //}));
                            DataTable table;
                            DataTable table2;
                            //Profile Initialization - Might don't need all friends information, just the count but I didn't want to brake the system, so let them go..
                            //Altrough this is not the best perfomance scenario
                            table = dbClient.ReadDataTable("SELECT * FROM user_relationships WHERE requester_id = " + id);
                            uint target, rstatus;
                            List<Relationship.Relationship> relationships = new List<Relationship.Relationship>();
                            Dictionary<uint, uint> tempRelationshipsDictionary = new Dictionary<uint, uint>();
                            
                            foreach (DataRow row3 in table.Rows)
                            {
                                target = Convert.ToUInt32(row3["target_id"]);
                                rstatus = Convert.ToUInt32(row3["relationshipstatus"]);
                                relationships.Add(new Relationship.Relationship(target, rstatus));
                            }
                            this.relationships = relationships;
                        }
                    }
                    else
                    {
                        this.Validated = false;
                    }
                }
            }
        }
        public Relationship.Relationship getRelation(uint id)
        {
            try
            {
                return GetRelationships().Where(x => x.targetID == id).ToArray()[0];
            }
            catch { }
            return null;
        }
        public UserDataFactory(string username, bool getAllData)
        {
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("username", username);
                this.UserData = dbClient.ReadDataRow("SELECT * FROM users WHERE username = @username LIMIT 1;");

                if (this.UserData != null)
                {
                    this.Validated = true;

                    uint id = (uint)this.UserData["Id"];

                    if (getAllData)
                    {
                        this.Achievements = dbClient.ReadDataTable("SELECT achievement_id,achievement_level FROM user_achievements WHERE user_id = '" + id + "'");
                        this.Favorites = dbClient.ReadDataTable("SELECT room_id FROM user_favorites WHERE user_id = '" + id + "'");
                        this.Ignores = dbClient.ReadDataTable("SELECT ignore_id FROM user_ignores WHERE user_id = '" + id + "'");
                        this.Tags = dbClient.ReadDataTable("SELECT tag FROM user_tags WHERE user_id = '" + id + "'");
                        this.Subscriptions = dbClient.ReadDataTable("SELECT subscription_id, timestamp_activated, timestamp_expire FROM user_subscriptions WHERE user_id = '" + id + "'");
                        this.Badges = dbClient.ReadDataTable("SELECT user_badges.badge_id,user_badges.badge_slot FROM user_badges WHERE user_id = " + id);
                        this.Items = dbClient.ReadDataTable("SELECT items.Id,items.base_item,,items.guild_data,items_extra_data.extra_data FROM items LEFT JOIN items_extra_data ON items_extra_data.item_id = items.Id WHERE room_id = 0 AND user_id = " + id);
                        this.Effects = dbClient.ReadDataTable("SELECT user_effects.effect_id,user_effects.total_duration,user_effects.is_activated,user_effects.activated_stamp FROM user_effects WHERE user_id =  " + id);
                        this.Friends = dbClient.ReadDataTable("SELECT users.Id,users.username,users.motto,users.look,users.last_online FROM users JOIN messenger_friendships ON users.Id = messenger_friendships.user_two_id WHERE messenger_friendships.user_one_id = '" + id + "'");
                        this.FriendRequests = dbClient.ReadDataTable("SELECT messenger_requests.Id,messenger_requests.from_id,users.username,users.gender,users.look FROM users JOIN messenger_requests ON users.Id = messenger_requests.from_id WHERE messenger_requests.to_id = '" + id + "'");

                        dbClient.AddParamWithValue("name", (string)this.UserData["username"]);

                        this.Rooms = dbClient.ReadDataTable("SELECT * FROM rooms WHERE owner = @name ORDER BY Id ASC LIMIT " + ServerConfiguration.RoomUserLimit);
                        this.Pets = dbClient.ReadDataTable("SELECT Id, user_id, room_id, name, type, race, color, expirience, energy, nutrition, respect, createstamp, x, y, z FROM user_pets WHERE user_id = " + id + " AND room_id = 0");
                        this.Bots = dbClient.ReadDataTable("SELECT * FROM user_bots WHERE owner_id = '" + id + "' AND room_id=0");
                        DataTable table;
                        DataTable table2;
                        //Profile Initialization - Might don't need all friends information, just the count but I didn't want to brake the system, so let them go..
                        //Altrough this is not the best perfomance scenario
                        table = dbClient.ReadDataTable("SELECT * FROM user_relationships WHERE requester_id = " + id);
                        uint target, rstatus;
                        List<Relationship.Relationship> relationships = new List<Relationship.Relationship>();
                        foreach (DataRow row3 in table.Rows)
                        {
                            target = Convert.ToUInt32(row3["target_id"]);
                            rstatus = Convert.ToUInt32(row3["relationshipstatus"]);
                            relationships.Add(new Relationship.Relationship(target, rstatus));
                        }
                        this.relationships = relationships;
                    }
                }
                else
                {
                    this.Validated = false;
                }
            }
        }
    }
}
