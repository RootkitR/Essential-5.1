using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Essential.Core;
using Essential.HabboHotel;
using Essential.Net;
using Essential.Storage;
using Essential.Util;
using Essential.Communication;
using Essential.Messages;
using System.Net;
using System.IO;
using System.Globalization;
namespace Essential.HabboHotel.AntiAd
{
    public class AntiAd
    {
        public List<string> IllegalWords = new List<string>();
        public AntiAd()
        {
            IllegalWords.Clear();
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                DataTable dt1 = dbClient.ReadDataTable("SELECT * FROM wordfilter");
                foreach (DataRow dr in dt1.Rows)
                {
                    IllegalWords.Add((string)dr["word"]);
                }
            }
        }
        public void Refresh()
        {
            IllegalWords.Clear();
            using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                DataTable dt1 = dbClient.ReadDataTable("SELECT * FROM wordfilter");
                foreach (DataRow dr in dt1.Rows)
                {
                    IllegalWords.Add((string)dr["word"]);
                }
            }
        }
        public bool ContainsIllegalWord(string t)
        {
            string s = Utf8ToUtf16(t).ToLower();
            string txt = s.Replace("+", "").Replace(" ", "").Replace("|", "").Replace("?", "").Replace("/", "").Replace("'", "").Replace('"'.ToString(), "");
            string txt1 = txt.Replace("*", "").Replace("~", "").Replace("(", "").Replace(")", "").Replace("=", "").Replace("&", "").Replace("§", "").Replace(",", "").Replace("[", "").Replace("^", "").Replace("´", "").Replace("`", "").Replace("_", "").Replace("°", "");
            string txt3 = txt1.Replace("û", "u").Replace("ú", "u").Replace("ù", "u").Replace("â", "a").Replace("á", "a").Replace("à", "a").Replace("@", "a").Replace("ô", "o").Replace("ó", "o").Replace("ò", "o").Replace("ê", "e").Replace("é", "e").Replace("è", "e").Replace("$", "s").Replace("3", "e").Replace("€", "e").Replace("î", "i").Replace("í", "i").Replace("ì", "i").Replace("!", "i").Replace("1", "i");
            string txt2 = txt3.Replace("!", "").Replace("?", "").Replace(":", "").Replace("-", "").Replace("_", "").Replace(";", "").Replace("<", "").Replace(">", "").Replace("$", "").Replace("#", "").Replace("¦", "").Replace("}", "").Replace("{", "").Replace("]", "");
            string txt4 = txt2.Replace("ä", "a").Replace("ü", "u").Replace("v", "u").Replace("ö", "o").Replace("ë", "e").Replace("ï", "i").Replace("ÿ", "y").Replace("•","").Replace("¬","").Replace("§","");
            string txt5 = txt4.Replace(".", "").Replace("-", "").Replace("/-/", "h");
            foreach (string word in IllegalWords)
            {
                string word2 = word.ToLower();
                if (txt5.Contains(word2) || txt4.Contains(word2))
                    return true;
            }
            return false;
        }
        public static string Utf8ToUtf16(string utf8String)
        {
            List<byte> list = new List<byte>(utf8String.Length);
            for (int i = 0; i < utf8String.Length; i++)
            {
                byte b = (byte)utf8String[i];
                if (b > 0)
                {
                    list.Add(b);
                }
            }
            return Encoding.UTF8.GetString(list.ToArray());
        }
    }
}
