using System;
using System.Collections.Generic;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.HabboHotel.SoundMachine;

namespace Essential.Communication.Messages.SoundMachine
{
    internal sealed class GetUserSongDisksMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            List<UserItem> list = new List<UserItem>();
            foreach (UserItem current in Session.GetHabbo().GetInventoryComponent().Items)
            {
                if (current != null && !(current.GetBaseItem().Name != "song_disk") && !Session.GetHabbo().GetInventoryComponent().list_1.Contains(current.uint_0))
                {
                    list.Add(current);
                }
            }


            ServerMessage Message = new ServerMessage(Outgoing.Inventory); // Updated
            Message.AppendInt32(list.Count);
            foreach (UserItem current2 in list) //MUN OMA
            {
                int int_ = 0;
                if (current2.string_0.Length > 0)
                {
                    int_ = int.Parse(current2.string_0);
                }
                SongData SongData = SongManager.GetSong(int_);
                if (SongData == null)
                {
                    return;
                }
                Message.AppendUInt(current2.uint_0);
                Message.AppendInt32(SongData.Id);
            }
            Session.SendMessage(Message);
        }
    }
}
