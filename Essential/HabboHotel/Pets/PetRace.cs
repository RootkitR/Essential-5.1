using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;

namespace Essential.HabboHotel.Pets
{
    internal sealed class PetRace
    {
        public int RaceId;
        public int Color1;
        public int Color2;
        public bool Has1Color;
        public bool Has2Color;

        public static List<PetRace> Races;

        public static void Init(DatabaseClient dbClient)
        {

            DataTable Table = dbClient.ReadDataTable("SELECT * FROM pets_breeds");

            Races = new List<PetRace>();
            foreach (DataRow Race in Table.Rows)
            {
                PetRace R = new PetRace();
                R.RaceId = (int)Race["breed_id"];
                R.Color1 = (int)Race["color1"];
                R.Color2 = (int)Race["color2"];
                R.Has1Color = ((string)Race["color1_enabled"] == "1");
                R.Has2Color = ((string)Race["color2_enabled"] == "1");
                Races.Add(R);
            }
        }

        public static List<PetRace> GetRacesForRaceId(int sRaceId)
        {
            List<PetRace> sRaces = new List<PetRace>();
            foreach (PetRace R in Races)
            {
                if (R.RaceId == sRaceId)
                    sRaces.Add(R);
            }

            return sRaces;
        }

        public static bool RaceGotRaces(int sRaceId)
        {
            if (GetRacesForRaceId(sRaceId).Count > 0)
                return true;
            else
                return false;
        }
    }
}
