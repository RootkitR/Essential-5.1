using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Users;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Essential.HabboHotel.Games.SnowWar
{
    internal class SnowStorm
    {
        internal int Kvalue = 0;
        internal int MaxUsers;
        internal SnowModel Model;
        internal int WarId;
        internal int WarLevel;
        internal Habbo WarOwner;
        internal int WarStarted;
        internal List<Habbo> WarUsers;
        internal int Countdown;
        internal Room SnowRoom;
        internal SnowStorm(Habbo warowner)
        {
            WarsData stormWars = Essential.GetGame().GetStormWars();
            stormWars.LastWarId++;
            this.WarId = Essential.GetGame().GetStormWars().LastWarId;
            this.WarLevel = 9;
            this.WarOwner = warowner;
            this.WarUsers = new List<Habbo>();
            this.WarStarted = 0;
            this.MaxUsers = 10;
            this.Model = Essential.GetGame().GetStormWars().RoomModel[this.WarLevel];
            this.Kvalue = 0;
            Essential.GetGame().GetStormWars().Wars.Add(this.WarId, this);
            this.Countdown = 3;
            RoomData class2 = new RoomData();
            class2.FillNull((uint)WarId);
            class2.Owner = "Rootkit";
            class2.Name = "SnowStorm Level " + WarLevel;
            this.SnowRoom = new Room(class2.Id, class2.Name, class2.Description, class2.Type, class2.Owner, class2.Category, class2.State, class2.UsersMax,"model_a", class2.CCTs, class2.Score, class2.Tags, class2.AllowPet, class2.AllowPetsEating, class2.AllowWalkthrough, class2.Hidewall, class2.Icon, class2.Password, class2.Wallpaper, class2.Floor, class2.Landscape, class2, class2.bool_3, class2.Wallthick, class2.Floorthick, class2.Achievement, this.Model.HeightMap, class2.HideOwner,false);
        }

        internal void AssignTeams()
        {
            for (int i = 0; i < this.WarUsers.Count; i++)
            {
                Habbo habbo = this.WarUsers[i];
                habbo.SnowStep = 2;
                switch (i)
                {
                    case 0:
                    case 2:
                    case 4:
                    case 6:
                    case 8:
                        if ((this.WarLevel == 9) && (i == 0))
                        {
                            habbo.SnowUserId = 20;
                            habbo.SnowX = 16000;
                            habbo.SnowY = 73600;
                        }
                        else if ((this.WarLevel == 9) && (i == 2))
                        {
                            habbo.SnowUserId = 0x15;
                            habbo.SnowX = 19200;
                            habbo.SnowY = 0x15e0a;
                        }
                        habbo.SnowRot = 2;
                        habbo.SnowTeam = 1;
                        this.SnowRoom.RoomUsers[habbo.SnowUserId] = new RoomUser(habbo.Id, (uint)this.WarId, habbo.SnowUserId, false);
                        break;
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 9:
                        habbo.SnowTeam = 2;
                        if ((this.WarLevel == 9) && (i == 1))
                        {
                            habbo.SnowUserId = 22;
                            //137600 64000
                            habbo.SnowX = 137600;
                            habbo.SnowY = 64000;
                        }
                        else if ((this.WarLevel == 9) && (i == 3))
                        {
                            habbo.SnowUserId = 23;
                            habbo.SnowX = 137600;
                            habbo.SnowY = 64000;
                        }
                        habbo.SnowRot = 6;
                        this.SnowRoom.RoomUsers[habbo.SnowUserId] = new RoomUser(habbo.Id, (uint)this.WarId, habbo.SnowUserId, false);
                        break;
                }
            }
        }

        internal void ProcessWar()
        {
            new ServerMessage(Outgoing.Game2GameStatusMessageEvent).AppendInt32(this.Kvalue);
            this.Kvalue++;
        }

        internal void SendToStorm(ServerMessage Packet, bool ToMe, uint UserId = 0)
        {
            foreach (Habbo habbo in this.WarUsers)
            {
                if (habbo != null && habbo.GetClient() != null && ((habbo.Id != UserId) && (!ToMe)))
                    habbo.GetClient().SendMessage(Packet);
            }
        }
        internal void SendToStorm(ServerMessage Packet)
        {
            foreach (Habbo habbo in this.WarUsers)
            {
                if(habbo != null && habbo.GetClient() != null)
                habbo.GetClient().SendMessage(Packet);
            }
        }
        internal void SnowStormStart()
        {
            if (this.WarStarted <= 1)
            {
                this.WarStarted = 2;
            }
        }
    }
}

