using Essential.Collections;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Users.UserDataManagement;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
namespace Essential.HabboHotel.Users.Relationship
{
    class RelationshipComposer
    {
        private Dictionary<uint, uint> composedRelations = new Dictionary<uint, uint>();
        internal uint UserID;
        private uint[] relationshipStatusIndex = { 0, 0, 0 };  //love,friend,enemy
        GameClient client;


        internal RelationshipComposer(uint userID, UserDataFactory data)
        {
            this.UserID = userID;
            foreach (Relationship relation in data.GetRelationships())
            {
                if (!this.composedRelations.ContainsKey(relation.targetID))
                {
                    composedRelations.Add(relation.targetID, relation.relationshipStatus);

                    switch (relation.relationshipStatus)
                    {
                        case 1:
                            relationshipStatusIndex[0]++;
                            break;
                        case 2:
                            relationshipStatusIndex[1]++;
                            break;
                        case 3:
                            relationshipStatusIndex[2]++;
                            break;
                    }
                }
            }
        }
        public GameClient GetClient()
        {
            return Essential.GetGame().GetClientManager().GetClientById(this.UserID);
        }
        internal RelationshipComposer(uint userID)
        {
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                this.UserID = userID;
                DataTable table;
                //Profile Initialization - Might don't need all friends information, just the count but I didn't want to brake the system, so let them go..
                //Altrough this is not the best perfomance scenario
                table = dbClient.ReadDataTable("SELECT * FROM user_relationships WHERE requester_id = " + userID);
                uint target, rstatus;
                List<Relationship> relationships2 = new List<Relationship>();
                Dictionary<uint, uint> tempRelationshipsDictionary = new Dictionary<uint, uint>();

                foreach (DataRow row3 in table.Rows)
                {
                    target = Convert.ToUInt32(row3["target_id"]);
                    rstatus = Convert.ToUInt32(row3["relationshipstatus"]);
                    relationships2.Add(new Relationship(target, rstatus));
                }
                foreach (Relationship relation in relationships2)
                {
                    if (!this.composedRelations.ContainsKey(relation.targetID))
                    {
                        composedRelations.Add(relation.targetID, relation.relationshipStatus);

                        switch (relation.relationshipStatus)
                        {
                            case 1:
                                relationshipStatusIndex[0]++;
                                break;
                            case 2:
                                relationshipStatusIndex[1]++;
                                break;
                            case 3:
                                relationshipStatusIndex[2]++;
                                break;
                        }
                    }
                }
            }
        }
        internal void RelationshipStatusUpdate(uint targetID, uint newRelationshipStatus)
        {
            if (newRelationshipStatus != 0)
            {
                if (this.HasRelationshipWith(targetID))
                {
                    using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
                    {
                        adapter.AddParamWithValue("sender", this.UserID);
                        adapter.AddParamWithValue("target", targetID);
                        adapter.AddParamWithValue("newstatus", newRelationshipStatus);
                        adapter.ExecuteQuery("UPDATE user_relationships SET relationshipstatus=@newstatus WHERE requester_id = @sender AND target_id = @target LIMIT 1");
                    }

                    relationshipStatusIndex[composedRelations[targetID] - 1]--;
                    composedRelations[targetID] = newRelationshipStatus;
                    relationshipStatusIndex[newRelationshipStatus - 1]++;
                }
                else
                {
                    using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
                    {
                        adapter.AddParamWithValue("sender", this.UserID);
                        adapter.AddParamWithValue("target", targetID);
                        adapter.AddParamWithValue("newstatus", newRelationshipStatus);
                        adapter.ExecuteQuery("INSERT INTO user_relationships (requester_id,target_id,relationshipstatus) VALUES (@sender,@target,@newstatus)");
                    }
                    relationshipStatusIndex[newRelationshipStatus - 1]++;
                    this.composedRelations.Add(targetID, newRelationshipStatus);
                }

            }
            else
            {
                using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
                {
                    adapter.AddParamWithValue("sender", this.UserID);
                    adapter.AddParamWithValue("target", targetID);
                    adapter.ExecuteQuery("DELETE FROM user_relationships WHERE requester_id = @sender AND target_id = @target LIMIT 1");
                }
                relationshipStatusIndex[composedRelations[targetID] - 1]--;
                this.composedRelations.Remove(targetID);
            }
        }
        internal ServerMessage SerializeRelationshipsProfile()
        {
            Random random = new Random();
            int indexesCounter = 0;
            ServerMessage Packet = new ServerMessage(Outgoing.ProfileRelationships);
            Packet.AppendInt32(this.UserID);
            for (int i = 0; i < 3; i++)
            {
                if (relationshipStatusIndex[i] > 0)
                    indexesCounter++;
            }
            Packet.AppendInt32(indexesCounter);
            if (indexesCounter > 0)
            {
                for (uint i = 1; i <= 3; i++)
                {
                    if (composedRelations.ContainsValue(i))
                    {
                        List<uint> relationshipsList = new List<uint>();
                        Packet.AppendInt32(i);
                        Packet.AppendInt32(relationshipStatusIndex[i - 1]);
                        foreach (KeyValuePair<uint, uint> iterator in this.composedRelations)
                        {
                            if (iterator.Value == i)
                            {
                                relationshipsList.Add(iterator.Key);
                            }
                        }
                        uint target_id = relationshipsList[random.Next(0, relationshipsList.Count)];
                        Packet.AppendInt32(target_id);
                        using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
                        {
                            DataRow row = adapter.ReadDataRow("SELECT username FROM users where id = " + target_id);
                            try
                            {
                                Packet.AppendString((string)row["username"]);
                            }
                            catch
                            {
                                Packet.AppendString("Unknown User");
                            }
                            //Packet.AppendString((string)row["look"]); newer versions only
                        }
                    }
                }
            }
            return Packet;
        }

        internal bool HasRelationshipWith(uint targetID)
        {
            return this.composedRelations.ContainsKey(targetID);
        }

        internal uint RelationshipStatusWith(uint targetID)
        {
            if (!HasRelationshipWith(targetID))
            {
                return 0;
            }
            else
                return composedRelations[targetID];
        }

        internal Dictionary<uint, uint> GetRelationshipList
        {
            get
            {
                return this.composedRelations;
            }
        }

        internal uint GetRelationshipsLovesCount
        {
            get
            {
                return this.relationshipStatusIndex[0];
            }
        }
        internal uint GetRelationshipsFriendsCount
        {
            get
            {
                return this.relationshipStatusIndex[1];
            }
        }
        internal uint GetRelationshipsEnemysCount
        {
            get
            {
                return this.relationshipStatusIndex[2];
            }
        }

        internal bool IsOnline
        {
            get
            {
                return ((((this.client != null) && (this.client.GetHabbo() != null)) && (this.client.GetHabbo().GetMessenger() != null)) && !this.client.GetHabbo().HideOnline);
            }
        }
        internal ServerMessage SerializeRelationshipUpdate(uint targetId, uint newRelationshipState)
        {
            GameClient gc = Essential.GetGame().GetClientManager().GetClientByHabbo(Essential.GetGame().GetClientManager().GetNameById(targetId));
            if (gc != null && gc.GetHabbo() != null)
            {
                this.RelationshipStatusUpdate(targetId, newRelationshipState);
                ServerMessage reply = new ServerMessage(Outgoing.FriendUpdate);
                reply.AppendInt32(0);
                reply.AppendInt32(1);
                reply.AppendInt32(0);
                new Messenger.MessengerBuddy(gc.GetHabbo().Id, gc.GetHabbo().Username, gc.GetHabbo().Figure, gc.GetHabbo().Motto, gc.GetHabbo().LastOnline, (int)newRelationshipState).Serialize(reply, false);
                return reply;
            }
            else
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    DataRow dRow = dbClient.ReadDataRow("SELECT username,look,motto,last_online FROM users WHERE id=" + targetId);
                    this.RelationshipStatusUpdate(targetId, newRelationshipState);
                    ServerMessage reply = new ServerMessage(Outgoing.FriendUpdate);
                    reply.AppendInt32(0);
                    reply.AppendInt32(1);
                    reply.AppendInt32(0);
                    new Messenger.MessengerBuddy(targetId, (string)dRow["username"], (string)dRow["look"], (string)dRow["motto"], (string)dRow["last_online"], (int)newRelationshipState).Serialize(reply, false);
                    reply.AppendInt32(newRelationshipState);
                    return reply;
                }
            }
        }
    }
}
