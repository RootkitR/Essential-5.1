using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using Essential.Core;
using Essential.Crypto;
using Essential.Storage;
using HabboEncryption;
namespace Essential
{
	internal sealed class EssentialEnvironment
	{
		private static Dictionary<string, string> ExternalTexts;

        internal static Crypto.HabboCrypto globalCrypto;
        internal static string publicToken;
        private static BigInteger n = new BigInteger("86851DD364D5C5CECE3C883171CC6DDC5760779B992482BD1E20DD296888DF91B33B936A7B93F06D29E8870F703A216257DEC7C81DE0058FEA4CC5116F75E6EFC4E9113513E45357DC3FD43D4EFAB5963EF178B78BD61E81A14C603B24C8BCCE0A12230B320045498EDC29282FF0603BC7B7DAE8FC1B05B52B2F301A9DC783B7", 16);
        private static BigInteger e = new BigInteger(3);
        private static BigInteger d = new BigInteger("59AE13E243392E89DED305764BDD9E92E4EAFA67BB6DAC7E1415E8C645B0950BCCD26246FD0D4AF37145AF5FA026C0EC3A94853013EAAE5FF1888360F4F9449EE023762EC195DFF3F30CA0B08B8C947E3859877B5D7DCED5C8715C58B53740B84E11FBC71349A27C31745FCEFEEEA57CFF291099205E230E0C7C27E8E1C0512B", 16);
        internal static string secretKey = "12844543231839046982589043811871065476910599239608903221675649771360705087933";//old: 24231219992253632572058933470468103090824667747608911151318774416044820318109
        


		public EssentialEnvironment()
		{
            publicToken = new BigInteger(DiffieHellman.GenerateRandomHexString(15), 16).ToString();
            globalCrypto = new Crypto.HabboCrypto(n, e, d);
			EssentialEnvironment.ExternalTexts = new Dictionary<string, string>();
		}

		public static void LoadExternalTexts(DatabaseClient dbClient)
		{
            Logging.Write("Fetching external texts for the Essential Environment.. ");

            if (ExternalTexts.Count > 0)
                ExternalTexts.Clear();

			DataTable dataTable = dbClient.ReadDataTable("SELECT identifier, display_text FROM texts ORDER BY identifier ASC;");

			if (dataTable != null)
			{
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    EssentialEnvironment.ExternalTexts.Add(dataRow["identifier"].ToString(), dataRow["display_text"].ToString());
                }
			}

            Logging.WriteLine("completed!", ConsoleColor.Green);
		}

		public static string GetExternalText(string key)
		{
			string result;

            if (EssentialEnvironment.ExternalTexts != null && EssentialEnvironment.ExternalTexts.ContainsKey(key))
				result = EssentialEnvironment.ExternalTexts[key];
			else
				result = key;

			return result;
		}

        public static int GetRandomNumber(int Min, int Max)
        {
            Random Quick = new Random();

            try
            {
                return Quick.Next(Min, Max);
            }
            catch
            {
                return Min;
            }
        }
	}
}