using System;
using System.Collections.Generic;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pathfinding;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.HabboHotel.Items.Interactors
{
    internal sealed class InteractorMannequin : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
        }
        internal string GetHair(string _Figure)
        {

            string FigurePartHair = _Figure;
            string GetHairPart;

            GetHairPart = System.Text.RegularExpressions.Regex.Split(_Figure, "hr")[1];
            FigurePartHair = GetHairPart.Split('.')[0];
            string FigurePartBody = _Figure;
            string GetBodyPart;

            GetBodyPart = System.Text.RegularExpressions.Regex.Split(_Figure, "hd")[1];
            FigurePartBody = GetBodyPart.Split('.')[0];

            string _Uni = Convert.ToString("hr" + FigurePartHair + "." + "hd" + FigurePartBody + ".");

            return _Uni;
        }
        public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if (RoomItem_0.ExtraData != "")
            {
                Session.GetHabbo().Figure = RoomItem_0.ExtraData + "." + GetHair(Session.GetHabbo().Figure);
                ServerMessage response = new ServerMessage(Outgoing.UpdateUserInformation);
                response.AppendInt32(-1);
                response.AppendString(Session.GetHabbo().Figure);
                response.AppendString(Session.GetHabbo().Gender.ToLower());
                response.AppendString(Session.GetHabbo().Motto);
                response.AppendInt32(Session.GetHabbo().AchievementScore);
                Session.SendMessage(response);
                if (Session.GetHabbo().InRoom)
                {
                    Room Room = Session.GetHabbo().CurrentRoom;


                    if (Room == null)
                    {
                        return;
                    }


                    RoomUser User = Room.GetRoomUserByHabbo(Session.GetHabbo().Id);


                    if (User == null)
                    {
                        return;
                    }

                    ServerMessage RoomUpdate = new ServerMessage(Outgoing.UpdateUserInformation);
                    RoomUpdate.AppendInt32(User.VirtualId);
                    RoomUpdate.AppendString(Session.GetHabbo().Figure);
                    RoomUpdate.AppendString(Session.GetHabbo().Gender.ToLower());
                    RoomUpdate.AppendString(Session.GetHabbo().Motto);
                    RoomUpdate.AppendInt32(Session.GetHabbo().AchievementScore);
                    Room.SendMessage(RoomUpdate, null);
                }
            }
            else
            {

            }
        }
    }
}
