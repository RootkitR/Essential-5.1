﻿using System;
using System.Collections.Generic;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorPrivateTile : FurniInteractor
    {
        private int Modes;
        public InteractorPrivateTile(int Modes)
        {
            this.Modes = Modes - 1;
            if (this.Modes < 0)
            {
                this.Modes = 0;
            }
        }
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
            for (int i = 0; i < RoomItem_0.GetRoom().RoomUsers.Length; i++)
            {
                RoomUser user = RoomItem_0.GetRoom().RoomUsers[i];
                if (user != null)
                {
                    if (user.onPrivateTile && user.privateTileID == RoomItem_0.uint_0)
                    {
                        user.privateTileID = 0;
                        user.onPrivateTile = false;
                    }
                }
            }
        }
        public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if (this.Modes != 0 && (bool_0 || RoomItem_0.GetBaseItem().InteractionType.ToLower() == "switch"))
            {
                if (RoomItem_0.GetBaseItem().InteractionType.ToLower() == "switch" && Session != null)
                {
                    RoomUser @class = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (@class.Position.x - RoomItem_0.GStruct1_1.x > 1 || @class.Position.y - RoomItem_0.GStruct1_1.y > 1)
                    {
                        if (@class.bool_0)
                        {
                            @class.MoveTo(RoomItem_0.GStruct1_0);
                            return;
                        }
                        return;
                    }
                }
                int num = 0;
                if (RoomItem_0.ExtraData.Length > 0)
                {
                    num = int.Parse(RoomItem_0.ExtraData);
                }
                int num2;
                if (num <= 0)
                {
                    num2 = 1;
                }
                else
                {
                    if (num >= this.Modes)
                    {
                        num2 = 0;
                    }
                    else
                    {
                        num2 = num + 1;
                    }
                }
                double double_ = RoomItem_0.Double_1;
                RoomItem_0.ExtraData = num2.ToString();
                RoomItem_0.UpdateState();
                if (double_ != RoomItem_0.Double_1)
                {
                    Dictionary<int, AffectedTile> dictionary = RoomItem_0.Dictionary_0;
                    if (dictionary == null)
                    {
                        dictionary = new Dictionary<int, AffectedTile>();
                    }
                    RoomItem_0.GetRoom().method_87(RoomItem_0.GetRoom().method_43(RoomItem_0.GetX, RoomItem_0.Int32_1), true, false);
                    foreach (AffectedTile current in dictionary.Values)
                    {
                        RoomItem_0.GetRoom().method_87(RoomItem_0.GetRoom().method_43(current.Int32_0, current.Int32_1), true, false);
                    }
                }
                if (Session != null)
                {
                    RoomUser RoomUser_ = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    RoomItem_0.GetRoom().method_10(RoomUser_, RoomItem_0);
                }
            }
        }
    }
}
