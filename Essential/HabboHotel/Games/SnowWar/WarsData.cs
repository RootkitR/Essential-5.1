using Essential;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Games.SnowWar
{
    internal class WarsData
    {
        internal int LastWarId;
        internal Dictionary<int, int> LevelScore;
        internal Dictionary<int, List<SnowItems>> RoomItems;
        internal Dictionary<int, SnowModel> RoomModel;
        internal Dictionary<int, SnowStorm> Wars;

        internal WarsData()
        {
            DatabaseClient adapter;
            DataTable table;
            this.Wars = new Dictionary<int, SnowStorm>();
            this.LastWarId = 0;
            this.GetLevelScore();
            this.RoomModel = new Dictionary<int, SnowModel>();
            this.RoomItems = new Dictionary<int, List<SnowItems>>();
            using (adapter = Essential.GetDatabase().GetClient())
            {

                table = adapter.ReadDataTable("SELECT * FROM storm_levels");
                foreach (DataRow row in table.Rows)
                {
                    List<SnowItems> list = new List<SnowItems>();
                    this.RoomItems.Add((int)row["level"], new SnowItems().GetSnowItems((int)row["level"], adapter));
                }
            }
            using (adapter = Essential.GetDatabase().GetClient())
            {
                table = adapter.ReadDataTable("SELECT * FROM storm_models");
                foreach (DataRow row in table.Rows)
                {
                    SnowModel model = new SnowModel(row)
                    {
                        SnowItems = this.RoomItems[(int)row["id"]]
                    };
                    this.RoomModel.Add((int)row["id"], model);
                }
            }
        }

        internal void GetLevelScore()
        {
            this.LevelScore = new Dictionary<int, int>();
            this.LevelScore.Add(0, 100);
            this.LevelScore.Add(1, 200);
        }

        internal SnowStorm GetWarByGameId(int GameId)
        {
            return Essential.GetGame().GetStormWars().Wars[GameId];
        }

        internal void OnCycle()
        {
            try
            {
                DateTime now = DateTime.Now;
                this.SnowCycleTask();
                TimeSpan span = (TimeSpan)(DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("WarsData.OnCycle spent: " + span.TotalSeconds + " seconds in working.");
                }
                Thread.Sleep(250);
            }
            catch
            {
            }
        }

        internal void SnowCycleTask()
        {
            foreach (SnowStorm storm in this.Wars.Values)
            {
                if (storm.WarStarted >= 2)
                {
                    new Task(new Action(storm.ProcessWar)).Start();
                }
            }
        }
    }
}

