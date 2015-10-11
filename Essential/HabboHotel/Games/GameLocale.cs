using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Games
{
    class GameLocale
    {
        internal string LanguageId;
        internal string LocaleKey;
        internal string LocaleValue;

        public GameLocale(string LanguageId, string LocaleKey, string LocaleValue)
        {
            this.LanguageId = LanguageId;
            this.LocaleKey = LocaleKey;
            this.LocaleValue = LocaleValue;
        }
    }
}
