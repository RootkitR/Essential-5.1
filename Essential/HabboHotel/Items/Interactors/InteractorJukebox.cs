using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Items.Interactors;
using Essential.HabboHotel.SoundMachine;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorJukebox : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem Item)
        {
            RoomMusicController roomMusicController = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).GetRoomMusicController();
            roomMusicController.LinkRoomOutputItemIfNotAlreadyExits(Item);
            roomMusicController.Stop();
            Session.GetHabbo().CurrentRoom.LoadMusic();
        }
        public override void OnRemove(GameClient Session, RoomItem Item)
        {
            RoomMusicController roomMusicController = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).GetRoomMusicController();
            roomMusicController.Stop();
            roomMusicController.UnLinkRoomOutputItem();
            Item.UpdateState(true, true);
        }
        public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            RoomMusicController roomMusicController = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).GetRoomMusicController();
            roomMusicController.LinkRoomOutputItemIfNotAlreadyExits(Item);

            if ((UserHasRights && (Session != null)) && (Item != null))
            {
                if (roomMusicController.IsPlaying)
                {
                    roomMusicController.Stop();
                }
                else
                {
                    roomMusicController.Start(Request);
                }
            }
        }
    }
}
