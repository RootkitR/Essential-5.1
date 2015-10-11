using System;
using Essential.HabboHotel.SoundMachine;
using Essential.Source.HabboHotel.SoundMachine;

namespace Essential.HabboHotel.SoundMachine
{
    internal class SongInstance
    {
        private SongItem mDiskItem;
        private HabboHotel.SoundMachine.SongData mSongData;

        public SongInstance(SongItem Item, SongData SongData)
        {
            this.mDiskItem = Item;
            this.mSongData = SongData;
        }

        public SongItem DiskItem
        {
            get
            {
                return this.mDiskItem;
            }
        }

        public SongData SongData
        {
            get
            {
                return this.mSongData;
            }
        }
    }
}

