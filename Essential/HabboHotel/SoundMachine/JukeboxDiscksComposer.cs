using Essential.HabboHotel.Items;
using Essential.HabboHotel.SoundMachine;
using Essential.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Essential.HabboHotel.SoundMachine
{
    class JukeboxDiscksComposer
    {
        public static ServerMessage Compose(int PlaylistCapacity, List<SongInstance> Playlist)
        {
            ServerMessage message = new ServerMessage(2799); // Updated
            message.AppendInt32(PlaylistCapacity);
            message.AppendInt32(Playlist.Count);
            foreach (SongInstance instance in Playlist)
            {
                message.AppendInt32(instance.DiskItem.itemID);
                message.AppendInt32(instance.SongData.Id);
            }
            return message;
        }

        public static ServerMessage ComposePlayingComposer(int SongId, int PlaylistItemNumber, int SyncTimestampMs)
        {
            ServerMessage message = new ServerMessage(Outgoing.Playing); // Update
            if (SongId == 0)
            {
                message.AppendInt32(-1);
                message.AppendInt32(-1);
                message.AppendInt32(-1);
                message.AppendInt32(-1);
                message.AppendInt32(0);
                return message;
            }
            message.AppendInt32(SongId);
            message.AppendInt32(PlaylistItemNumber);
            message.AppendInt32(SongId);
            message.AppendInt32(0);
            message.AppendInt32(SyncTimestampMs);
            return message;
        }

        public static ServerMessage SerializeSongInventory(Hashtable songs)
        {
            ServerMessage message = new ServerMessage(Outgoing.SongInventory); // Updated
            message.AppendInt32(songs.Count);
            foreach (UserItem item in songs.Values)
            {
                int i = Convert.ToInt32(item.string_0);
                message.AppendInt32((int)item.uint_0);
                message.AppendInt32(i);
            }
            return message;
        }
    }
}
