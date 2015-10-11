using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Essential.HabboHotel.Roles
{
    class Configuration
    {
        private Dictionary<string, string> config = new Dictionary<string, string>();
        public Configuration()
        {
            if (!File.Exists("commands.conf"))
            {
                throw new Exception("Unable to locate configuration file at 'commands.conf'.");
            }
            foreach (string s in System.IO.File.ReadAllLines("commands.conf"))
            {
                if (s != "")
                    config.Add(s.Split('=')[0], s.Split('=')[1]);

            }
        }
        public string getData(string key)
        {
            return config[key];
        }
    }
}
