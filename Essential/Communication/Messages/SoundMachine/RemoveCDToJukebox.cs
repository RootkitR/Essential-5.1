using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.SoundMachine;
using Essential.Messages;
using Essential.Source.HabboHotel.SoundMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Essential.Communication.Messages.SoundMachine
{
    class RemoveCDToJukebox : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (((Session != null) && (Session.GetHabbo() != null)) && (Session.GetHabbo().CurrentRoom != null))
            {
                Room currentRoom = Session.GetHabbo().CurrentRoom;
                if (currentRoom.CheckRights(Session, true) && currentRoom.GotMusicController())
                {
                    RoomMusicController roomMusicController = currentRoom.GetRoomMusicController();
                    SongItem item = roomMusicController.RemoveDisk(Event.PopWiredInt32());
                    if (item != null)
                    {
                        item.RemoveFromDatabase();
                        Session.GetHabbo().GetInventoryComponent().method_11((uint)item.itemID, item.baseItem.UInt32_0, item.songID.ToString(), false, 0, 0, "");
                        Session.GetHabbo().GetInventoryComponent().method_9(true);
                        Session.SendMessage(JukeboxDiscksComposer.SerializeSongInventory(Session.GetHabbo().GetInventoryComponent().songDisks));
                        Session.SendMessage(JukeboxDiscksComposer.Compose(roomMusicController.PlaylistCapacity, roomMusicController.Playlist.Values.ToList<SongInstance>()));
                    }
                }
            }
        }
    }
}
