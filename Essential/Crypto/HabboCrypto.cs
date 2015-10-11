using Essential.HabboHotel.GameClients;
using HabboEncryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Essential.Crypto
{
    internal class HabboCrypto : DiffieHellman
    {
        private HabboEncryption.RSA RSA;

        public HabboCrypto(BigInteger n, BigInteger e, BigInteger d)
            : base(200)
        {
            this.RSA = new HabboEncryption.RSA(n, e, d, 0, 0, 0, 0, 0);
            this.RC4 = new HabboEncryption.RC4();
            this.Initialized = false;
        }

        public bool InitializeRC4ToSession(GameClient Session, string ctext)
        {
            try
            {
                string str = this.RSA.Decrypt(ctext);
                char ch = '\0';
                base.GenerateSharedKey(str.Replace(ch.ToString(), ""));
                Session.DesignedHandler = new Random().Next(1, 5);
                HabboEncryption.RC4.Init(base.SharedKey.getBytes(), ref Session.i, ref Session.j, ref Session.table);
                Session.CryptoInitialized = true;
                this.Initialized = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Initialized { get; private set; }

        public HabboEncryption.RC4 RC4 { get; private set; }
    }
}
