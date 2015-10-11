using System;
using System.Diagnostics;
using System.Threading;
using Essential.Core;
using Essential.Storage;
using System.Globalization;
using Essential.Messages;
namespace Essential.HabboHotel.Misc
{
    public sealed class LowPriorityWorker
    {
        public static void Work()
        {
            double lastDatabaseUpdate = Essential.GetUnixTimestamp();

            while (true)
            {
                try
                {
                    DateTime now = DateTime.Now;
                    TimeSpan timeSpan = now - Essential.ServerStarted;
                    new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    int Status = 1;

                    int UsersOnline = Essential.GetGame().GetClientManager().ClientCount;
                  //  Essential.GetGame().GetRoomManager().UnloadEmptyRooms();
                    int RoomsLoaded = Essential.GetGame().GetRoomManager().LoadedRoomsCount;
                    double timestamp = Essential.GetUnixTimestamp() - lastDatabaseUpdate;

                    if (timestamp >= 5)
                    {
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            dbClient.ExecuteQuery(string.Concat(new object[]
						    {
							    "UPDATE server_status SET stamp = UNIX_TIMESTAMP(), status = '", Status, "', users_online = '",	UsersOnline, "', rooms_loaded = '",	RoomsLoaded, "', server_ver = '", Essential.PrettyVersion,	"' LIMIT 1" 	}));
                                uint num3 = (uint)dbClient.ReadInt32("SELECT users FROM system_stats ORDER BY ID DESC LIMIT 1");
                                if ((long)UsersOnline > (long)((ulong)num3))
                                {
                                    dbClient.ExecuteQuery(string.Concat(new object[]
							    {
								    "UPDATE system_stats SET users = '",
								    UsersOnline,
								    "', rooms = '",
								    RoomsLoaded,
								    "' ORDER BY ID DESC LIMIT 1"
							    }));
                            }
                        }

                        lastDatabaseUpdate = Essential.GetUnixTimestamp();
                    }

                    Essential.GetGame().GetClientManager().UpdateEffects();

                    Console.Title = string.Concat(new object[]
					{
						"Essential | Online Users: ",
						UsersOnline,
						" | Rooms Loaded: ",
						RoomsLoaded,
						" | Uptime: ",
						timeSpan.Days,
						" days, ",
						timeSpan.Hours,
						" hours and ",
						timeSpan.Minutes,
						" minutes"
					});
                }
                catch (Exception ex)
                {
                    Program.DeleteMenu(Program.GetSystemMenu(Program.GetConsoleWindow(), true), Program.SC_CLOSE, Program.MF_BYCOMMAND);
                    Logging.LogThreadException(ex.ToString(), "Server status update task");
                }
                Thread.Sleep(5000);
            }
        }
    }
}
