using System;
using System.Collections.Generic;
using System.Data;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.Util;
using Essential.Storage;
namespace Essential.HabboHotel.Roles
{
	internal sealed class RoleManager
	{
		private Dictionary<uint, List<string>> dictionary_0;
		private Dictionary<uint, List<string>> dictionary_1;
		public Dictionary<uint, string> dictionary_2;
		private Dictionary<uint, int> dictionary_3;
		public Dictionary<string, int> dictionary_4;
		public Dictionary<string, int> dictionary_5;
        public Configuration config;
		public RoleManager()
		{
			this.dictionary_0 = new Dictionary<uint, List<string>>();
			this.dictionary_1 = new Dictionary<uint, List<string>>();
			this.dictionary_2 = new Dictionary<uint, string>();
			this.dictionary_3 = new Dictionary<uint, int>();
			this.dictionary_4 = new Dictionary<string, int>();
			this.dictionary_5 = new Dictionary<string, int>();
            config = new Configuration();
		}
        public Configuration GetConfiguration()
        {
            return config;
        }
        public void UpdateConfiguration()
        {
            config = new Configuration();
        }
		public void Initialize(DatabaseClient class6_0)
		{
			Logging.Write(EssentialEnvironment.GetExternalText("emu_loadroles"));
            this.ClearDictionaries();
			DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM ranks ORDER BY Id ASC;");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.dictionary_2.Add((uint)dataRow["Id"], dataRow["badgeid"].ToString());
				}
			}
			dataTable = class6_0.ReadDataTable("SELECT * FROM permissions_users ORDER BY userid ASC;");
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    List<string> list = new List<string>();
                    if (Essential.StringToBoolean(dataRow["cmd_update_settings"].ToString()))
                    {
                        list.Add("cmd_update_settings");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_update_bans"].ToString()))
                    {
                        list.Add("cmd_update_bans");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_update_bots"].ToString()))
                    {
                        list.Add("cmd_update_bots");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_update_catalogue"].ToString()))
                    {
                        list.Add("cmd_update_catalogue");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_update_navigator"].ToString()))
                    {
                        list.Add("cmd_update_navigator");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_update_items"].ToString()))
                    {
                        list.Add("cmd_update_items");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_award"].ToString()))
                    {
                        list.Add("cmd_award");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_coords"].ToString()))
                    {
                        list.Add("cmd_coords");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_override"].ToString()))
                    {
                        list.Add("cmd_override");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_coins"].ToString()))
                    {
                        list.Add("cmd_coins");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_pixels"].ToString()))
                    {
                        list.Add("cmd_pixels");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_ha"].ToString()))
                    {
                        list.Add("cmd_ha");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_hal"].ToString()))
                    {
                        list.Add("cmd_hal");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_freeze"].ToString()))
                    {
                        list.Add("cmd_freeze");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_enable"].ToString()))
                    {
                        list.Add("cmd_enable");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_roommute"].ToString()))
                    {
                        list.Add("cmd_roommute");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_setspeed"].ToString()))
                    {
                        list.Add("cmd_setspeed");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_masscredits"].ToString()))
                    {
                        list.Add("cmd_masscredits");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_globalcredits"].ToString()))
                    {
                        list.Add("cmd_globalcredits");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_masspixels"].ToString()))
                    {
                        list.Add("cmd_masspixels");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_globalpixels"].ToString()))
                    {
                        list.Add("cmd_globalpixels");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_roombadge"].ToString()))
                    {
                        list.Add("cmd_roombadge");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_massbadge"].ToString()))
                    {
                        list.Add("cmd_massbadge");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_userinfo"].ToString()))
                    {
                        list.Add("cmd_userinfo");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_userinfo_viewip"].ToString()))
                    {
                        list.Add("cmd_userinfo_viewip");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_shutdown"].ToString()))
                    {
                        list.Add("cmd_shutdown");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_givebadge"].ToString()))
                    {
                        list.Add("cmd_givebadge");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_removebadge"].ToString()))
                    {
                        list.Add("cmd_removebadge");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_summon"].ToString()))
                    {
                        list.Add("cmd_summon");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_invisible"].ToString()))
                    {
                        list.Add("cmd_invisible");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_ban"].ToString()))
                    {
                        list.Add("cmd_ban");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_superban"].ToString()))
                    {
                        list.Add("cmd_superban");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_roomkick"].ToString()))
                    {
                        list.Add("cmd_roomkick");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_roomalert"].ToString()))
                    {
                        list.Add("cmd_roomalert");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_mute"].ToString()))
                    {
                        list.Add("cmd_mute");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_unmute"].ToString()))
                    {
                        list.Add("cmd_unmute");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_alert"].ToString()))
                    {
                        list.Add("cmd_alert");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_motd"].ToString()))
                    {
                        list.Add("cmd_motd");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_kick"].ToString()))
                    {
                        list.Add("cmd_kick");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_update_filter"].ToString()))
                    {
                        list.Add("cmd_update_filter");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_update_permissions"].ToString()))
                    {
                        list.Add("cmd_update_permissions");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_sa"].ToString()))
                    {
                        list.Add("cmd_sa");
                    }
                    if (Essential.StringToBoolean(dataRow["receive_sa"].ToString()))
                    {
                        list.Add("receive_sa");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_ipban"].ToString()))
                    {
                        list.Add("cmd_ipban");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_spull"].ToString()))
                    {
                        list.Add("cmd_spull");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_disconnect"].ToString()))
                    {
                        list.Add("cmd_disconnect");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_update_achievements"].ToString()))
                    {
                        list.Add("cmd_update_achievements");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_update_texts"].ToString()))
                    {
                        list.Add("cmd_update_texts");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_teleport"].ToString()))
                    {
                        list.Add("cmd_teleport");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_points"].ToString()))
                    {
                        list.Add("cmd_points");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_masspoints"].ToString()))
                    {
                        list.Add("cmd_masspoints");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_globalpoints"].ToString()))
                    {
                        list.Add("cmd_globalpoints");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_empty"].ToString()))
                    {
                        list.Add("cmd_empty");
                    }
                    if (Essential.StringToBoolean(dataRow["ignore_roommute"].ToString()))
                    {
                        list.Add("ignore_roommute");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_anyroomrights"].ToString()))
                    {
                        list.Add("acc_anyroomrights");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_anyroomowner"].ToString()))
                    {
                        list.Add("acc_anyroomowner");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_supporttool"].ToString()))
                    {
                        list.Add("acc_supporttool");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_chatlogs"].ToString()))
                    {
                        list.Add("acc_chatlogs");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_enter_fullrooms"].ToString()))
                    {
                        list.Add("acc_enter_fullrooms");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_enter_anyroom"].ToString()))
                    {
                        list.Add("acc_enter_anyroom");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_restrictedrooms"].ToString()))
                    {
                        list.Add("acc_restrictedrooms");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_unkickable"].ToString()))
                    {
                        list.Add("acc_unkickable");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_unbannable"].ToString()))
                    {
                        list.Add("acc_unbannable");
                    }
                    if (Essential.StringToBoolean(dataRow["ignore_friendsettings"].ToString()))
                    {
                        list.Add("ignore_friendsettings");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_sql"].ToString()))
                    {
                        list.Add("wired_give_sql");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_badge"].ToString()))
                    {
                        list.Add("wired_give_badge");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_effect"].ToString()))
                    {
                        list.Add("wired_give_effect");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_award"].ToString()))
                    {
                        list.Add("wired_give_award");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_dance"].ToString()))
                    {
                        list.Add("wired_give_dance");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_send"].ToString()))
                    {
                        list.Add("wired_give_send");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_credits"].ToString()))
                    {
                        list.Add("wired_give_credits");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_pixels"].ToString()))
                    {
                        list.Add("wired_give_pixels");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_points"].ToString()))
                    {
                        list.Add("wired_give_points");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_rank"].ToString()))
                    {
                        list.Add("wired_give_rank");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_respect"].ToString()))
                    {
                        list.Add("wired_give_respect");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_handitem"].ToString()))
                    {
                        list.Add("wired_give_handitem");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_alert"].ToString()))
                    {
                        list.Add("wired_give_alert");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_roomusers"].ToString()))
                    {
                        list.Add("wired_cnd_roomusers");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_userhasachievement"].ToString()))
                    {
                        list.Add("wired_cnd_userhasachievement");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_userhasbadge"].ToString()))
                    {
                        list.Add("wired_cnd_userhasbadge");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_userhasvip"].ToString()))
                    {
                        list.Add("wired_cnd_userhasvip");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_userhaseffect"].ToString()))
                    {
                        list.Add("wired_cnd_userhaseffect");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_userrank"].ToString()))
                    {
                        list.Add("wired_cnd_userrank");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_usercredits"].ToString()))
                    {
                        list.Add("wired_cnd_usercredits");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_userpixels"].ToString()))
                    {
                        list.Add("wired_cnd_userpixels");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_userpoints"].ToString()))
                    {
                        list.Add("wired_cnd_userpoints");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_usergroups"].ToString()))
                    {
                        list.Add("wired_cnd_usergroups");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_wearing"].ToString()))
                    {
                        list.Add("wired_cnd_wearing");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_carrying"].ToString()))
                    {
                        list.Add("wired_cnd_carrying");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_give_wiredactived"].ToString()))
                    {
                        list.Add("wired_give_wiredactived");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_wiredactived"].ToString()))
                    {
                        list.Add("wired_cnd_wiredactived");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_unlimitedselects"].ToString()))
                    {
                        list.Add("wired_unlimitedselects");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_dance"].ToString()))
                    {
                        list.Add("cmd_dance");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_rave"].ToString()))
                    {
                        list.Add("cmd_rave");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_roll"].ToString()))
                    {
                        list.Add("cmd_roll");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_control"].ToString()))
                    {
                        list.Add("cmd_control");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_makesay"].ToString()))
                    {
                        list.Add("cmd_makesay");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_sitdown"].ToString()))
                    {
                        list.Add("cmd_sitdown");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_lay"].ToString()))
                    {
                        list.Add("cmd_lay");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_push"].ToString()))
                    {
                        list.Add("cmd_push");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_pull"].ToString()))
                    {
                        list.Add("cmd_pull");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_flagme"].ToString()))
                    {
                        list.Add("cmd_flagme");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_mimic"].ToString()))
                    {
                        list.Add("cmd_mimic");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_moonwalk"].ToString()))
                    {
                        list.Add("cmd_moonwalk");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_follow"].ToString()))
                    {
                        list.Add("cmd_follow");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_handitem"].ToString()))
                    {
                        list.Add("cmd_handitem");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_startquestion"].ToString()))
                    {
                        list.Add("cmd_startquestion");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_vipha"].ToString()))
                    {
                        list.Add("cmd_vipha");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_spush"].ToString()))
                    {
                        list.Add("cmd_spush");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_roomeffect"].ToString()))
                    {
                        list.Add("cmd_roomeffect");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_viphal"].ToString()))
                    {
                        list.Add("cmd_viphal");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_moveotheruserstodoor"].ToString()))
                    {
                        list.Add("acc_moveotheruserstodoor");
                    }
                    this.dictionary_0.Add((uint)dataRow["userid"], list);
                }
            }
			dataTable = class6_0.ReadDataTable("SELECT * FROM permissions_ranks ORDER BY rank ASC;");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.dictionary_3.Add((uint)dataRow["rank"], (int)dataRow["floodtime"]);
				}
				foreach (DataRow dataRow in dataTable.Rows)
				{
					List<string> list = new List<string>();
					if (Essential.StringToBoolean(dataRow["cmd_update_settings"].ToString()))
					{
						list.Add("cmd_update_settings");
					}
					if (Essential.StringToBoolean(dataRow["cmd_update_bans"].ToString()))
					{
						list.Add("cmd_update_bans");
					}
					if (Essential.StringToBoolean(dataRow["cmd_update_bots"].ToString()))
					{
						list.Add("cmd_update_bots");
					}
					if (Essential.StringToBoolean(dataRow["cmd_update_catalogue"].ToString()))
					{
						list.Add("cmd_update_catalogue");
					}
					if (Essential.StringToBoolean(dataRow["cmd_update_navigator"].ToString()))
					{
						list.Add("cmd_update_navigator");
					}
					if (Essential.StringToBoolean(dataRow["cmd_update_items"].ToString()))
					{
						list.Add("cmd_update_items");
					}
					if (Essential.StringToBoolean(dataRow["cmd_award"].ToString()))
					{
						list.Add("cmd_award");
					}
					if (Essential.StringToBoolean(dataRow["cmd_coords"].ToString()))
					{
						list.Add("cmd_coords");
					}
					if (Essential.StringToBoolean(dataRow["cmd_override"].ToString()))
					{
						list.Add("cmd_override");
					}
					if (Essential.StringToBoolean(dataRow["cmd_coins"].ToString()))
					{
						list.Add("cmd_coins");
					}
					if (Essential.StringToBoolean(dataRow["cmd_pixels"].ToString()))
					{
						list.Add("cmd_pixels");
					}
					if (Essential.StringToBoolean(dataRow["cmd_ha"].ToString()))
					{
						list.Add("cmd_ha");
					}
					if (Essential.StringToBoolean(dataRow["cmd_hal"].ToString()))
					{
						list.Add("cmd_hal");
					}
					if (Essential.StringToBoolean(dataRow["cmd_freeze"].ToString()))
					{
						list.Add("cmd_freeze");
					}
					if (Essential.StringToBoolean(dataRow["cmd_enable"].ToString()))
					{
						list.Add("cmd_enable");
					}
					if (Essential.StringToBoolean(dataRow["cmd_roommute"].ToString()))
					{
						list.Add("cmd_roommute");
					}
					if (Essential.StringToBoolean(dataRow["cmd_setspeed"].ToString()))
					{
						list.Add("cmd_setspeed");
					}
					if (Essential.StringToBoolean(dataRow["cmd_masscredits"].ToString()))
					{
						list.Add("cmd_masscredits");
					}
					if (Essential.StringToBoolean(dataRow["cmd_globalcredits"].ToString()))
					{
						list.Add("cmd_globalcredits");
					}
					if (Essential.StringToBoolean(dataRow["cmd_masspixels"].ToString()))
					{
						list.Add("cmd_masspixels");
					}
					if (Essential.StringToBoolean(dataRow["cmd_globalpixels"].ToString()))
					{
						list.Add("cmd_globalpixels");
					}
					if (Essential.StringToBoolean(dataRow["cmd_roombadge"].ToString()))
					{
						list.Add("cmd_roombadge");
					}
					if (Essential.StringToBoolean(dataRow["cmd_massbadge"].ToString()))
					{
						list.Add("cmd_massbadge");
					}
					if (Essential.StringToBoolean(dataRow["cmd_userinfo"].ToString()))
					{
						list.Add("cmd_userinfo");
					}
					if (Essential.StringToBoolean(dataRow["cmd_userinfo_viewip"].ToString()))
					{
						list.Add("cmd_userinfo_viewip");
					}
					if (Essential.StringToBoolean(dataRow["cmd_shutdown"].ToString()))
					{
						list.Add("cmd_shutdown");
					}
					if (Essential.StringToBoolean(dataRow["cmd_givebadge"].ToString()))
					{
						list.Add("cmd_givebadge");
					}
					if (Essential.StringToBoolean(dataRow["cmd_removebadge"].ToString()))
					{
						list.Add("cmd_removebadge");
					}
					if (Essential.StringToBoolean(dataRow["cmd_summon"].ToString()))
					{
						list.Add("cmd_summon");
					}
					if (Essential.StringToBoolean(dataRow["cmd_invisible"].ToString()))
					{
						list.Add("cmd_invisible");
					}
					if (Essential.StringToBoolean(dataRow["cmd_ban"].ToString()))
					{
						list.Add("cmd_ban");
					}
					if (Essential.StringToBoolean(dataRow["cmd_superban"].ToString()))
					{
						list.Add("cmd_superban");
					}
					if (Essential.StringToBoolean(dataRow["cmd_roomkick"].ToString()))
					{
						list.Add("cmd_roomkick");
					}
					if (Essential.StringToBoolean(dataRow["cmd_roomalert"].ToString()))
					{
						list.Add("cmd_roomalert");
					}
					if (Essential.StringToBoolean(dataRow["cmd_mute"].ToString()))
					{
						list.Add("cmd_mute");
					}
					if (Essential.StringToBoolean(dataRow["cmd_unmute"].ToString()))
					{
						list.Add("cmd_unmute");
					}
					if (Essential.StringToBoolean(dataRow["cmd_alert"].ToString()))
					{
						list.Add("cmd_alert");
					}
					if (Essential.StringToBoolean(dataRow["cmd_motd"].ToString()))
					{
						list.Add("cmd_motd");
					}
					if (Essential.StringToBoolean(dataRow["cmd_kick"].ToString()))
					{
						list.Add("cmd_kick");
					}
					if (Essential.StringToBoolean(dataRow["cmd_update_filter"].ToString()))
					{
						list.Add("cmd_update_filter");
					}
					if (Essential.StringToBoolean(dataRow["cmd_update_permissions"].ToString()))
					{
						list.Add("cmd_update_permissions");
					}
					if (Essential.StringToBoolean(dataRow["cmd_sa"].ToString()))
					{
						list.Add("cmd_sa");
					}
					if (Essential.StringToBoolean(dataRow["receive_sa"].ToString()))
					{
						list.Add("receive_sa");
					}
					if (Essential.StringToBoolean(dataRow["cmd_ipban"].ToString()))
					{
						list.Add("cmd_ipban");
					}
					if (Essential.StringToBoolean(dataRow["cmd_spull"].ToString()))
					{
						list.Add("cmd_spull");
					}
					if (Essential.StringToBoolean(dataRow["cmd_disconnect"].ToString()))
					{
						list.Add("cmd_disconnect");
					}
					if (Essential.StringToBoolean(dataRow["cmd_update_achievements"].ToString()))
					{
						list.Add("cmd_update_achievements");
					}
					if (Essential.StringToBoolean(dataRow["cmd_update_texts"].ToString()))
					{
						list.Add("cmd_update_texts");
					}
					if (Essential.StringToBoolean(dataRow["cmd_teleport"].ToString()))
					{
						list.Add("cmd_teleport");
					}
					if (Essential.StringToBoolean(dataRow["cmd_points"].ToString()))
					{
						list.Add("cmd_points");
					}
					if (Essential.StringToBoolean(dataRow["cmd_masspoints"].ToString()))
					{
						list.Add("cmd_masspoints");
					}
					if (Essential.StringToBoolean(dataRow["cmd_globalpoints"].ToString()))
					{
						list.Add("cmd_globalpoints");
					}
					if (Essential.StringToBoolean(dataRow["cmd_empty"].ToString()))
					{
						list.Add("cmd_empty");
					}
					if (Essential.StringToBoolean(dataRow["ignore_roommute"].ToString()))
					{
						list.Add("ignore_roommute");
					}
					if (Essential.StringToBoolean(dataRow["acc_anyroomrights"].ToString()))
					{
						list.Add("acc_anyroomrights");
					}
					if (Essential.StringToBoolean(dataRow["acc_anyroomowner"].ToString()))
					{
						list.Add("acc_anyroomowner");
					}
					if (Essential.StringToBoolean(dataRow["acc_supporttool"].ToString()))
					{
						list.Add("acc_supporttool");
					}
					if (Essential.StringToBoolean(dataRow["acc_chatlogs"].ToString()))
					{
						list.Add("acc_chatlogs");
					}
					if (Essential.StringToBoolean(dataRow["acc_enter_fullrooms"].ToString()))
					{
						list.Add("acc_enter_fullrooms");
					}
					if (Essential.StringToBoolean(dataRow["acc_enter_anyroom"].ToString()))
					{
						list.Add("acc_enter_anyroom");
					}
					if (Essential.StringToBoolean(dataRow["acc_restrictedrooms"].ToString()))
					{
						list.Add("acc_restrictedrooms");
					}
					if (Essential.StringToBoolean(dataRow["acc_unkickable"].ToString()))
					{
						list.Add("acc_unkickable");
					}
					if (Essential.StringToBoolean(dataRow["acc_unbannable"].ToString()))
					{
						list.Add("acc_unbannable");
					}
					if (Essential.StringToBoolean(dataRow["ignore_friendsettings"].ToString()))
					{
						list.Add("ignore_friendsettings");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_sql"].ToString()))
					{
						list.Add("wired_give_sql");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_badge"].ToString()))
					{
						list.Add("wired_give_badge");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_effect"].ToString()))
					{
						list.Add("wired_give_effect");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_award"].ToString()))
					{
						list.Add("wired_give_award");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_dance"].ToString()))
					{
						list.Add("wired_give_dance");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_send"].ToString()))
					{
						list.Add("wired_give_send");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_credits"].ToString()))
					{
						list.Add("wired_give_credits");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_pixels"].ToString()))
					{
						list.Add("wired_give_pixels");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_points"].ToString()))
					{
						list.Add("wired_give_points");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_rank"].ToString()))
					{
						list.Add("wired_give_rank");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_respect"].ToString()))
					{
						list.Add("wired_give_respect");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_handitem"].ToString()))
					{
						list.Add("wired_give_handitem");
					}
					if (Essential.StringToBoolean(dataRow["wired_give_alert"].ToString()))
					{
						list.Add("wired_give_alert");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_roomusers"].ToString()))
					{
						list.Add("wired_cnd_roomusers");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_userhasachievement"].ToString()))
					{
						list.Add("wired_cnd_userhasachievement");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_userhasbadge"].ToString()))
					{
						list.Add("wired_cnd_userhasbadge");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_userhasvip"].ToString()))
					{
						list.Add("wired_cnd_userhasvip");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_userhaseffect"].ToString()))
					{
						list.Add("wired_cnd_userhaseffect");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_userrank"].ToString()))
					{
						list.Add("wired_cnd_userrank");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_usercredits"].ToString()))
					{
						list.Add("wired_cnd_usercredits");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_userpixels"].ToString()))
					{
						list.Add("wired_cnd_userpixels");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_userpoints"].ToString()))
					{
						list.Add("wired_cnd_userpoints");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_usergroups"].ToString()))
					{
						list.Add("wired_cnd_usergroups");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_wearing"].ToString()))
					{
						list.Add("wired_cnd_wearing");
					}
					if (Essential.StringToBoolean(dataRow["wired_cnd_carrying"].ToString()))
					{
						list.Add("wired_cnd_carrying");
					}
                    if (Essential.StringToBoolean(dataRow["wired_give_wiredactived"].ToString()))
                    {
                        list.Add("wired_give_wiredactived");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_cnd_wiredactived"].ToString()))
                    {
                        list.Add("wired_cnd_wiredactived");
                    }
                    if (Essential.StringToBoolean(dataRow["wired_unlimitedselects"].ToString()))
                    {
                        list.Add("wired_unlimitedselects");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_dance"].ToString()))
                    {
                        list.Add("cmd_dance");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_rave"].ToString()))
                    {
                        list.Add("cmd_rave");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_roll"].ToString()))
                    {
                        list.Add("cmd_roll");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_control"].ToString()))
                    {
                        list.Add("cmd_control");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_makesay"].ToString()))
                    {
                        list.Add("cmd_makesay");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_sitdown"].ToString()))
                    {
                        list.Add("cmd_sitdown");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_lay"].ToString()))
                    {
                        list.Add("cmd_lay");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_push"].ToString()))
                    {
                        list.Add("cmd_push");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_pull"].ToString()))
                    {
                        list.Add("cmd_pull");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_flagme"].ToString()))
                    {
                        list.Add("cmd_flagme");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_mimic"].ToString()))
                    {
                        list.Add("cmd_mimic");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_moonwalk"].ToString()))
                    {
                        list.Add("cmd_moonwalk");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_follow"].ToString()))
                    {
                        list.Add("cmd_follow");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_handitem"].ToString()))
                    {
                        list.Add("cmd_handitem");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_startquestion"].ToString()))
                    {
                        list.Add("cmd_startquestion");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_vipha"].ToString()))
                    {
                        list.Add("cmd_vipha");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_spush"].ToString()))
                    {
                        list.Add("cmd_spush");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_roomeffect"].ToString()))
                    {
                        list.Add("cmd_roomeffect");
                    }
                    if (Essential.StringToBoolean(dataRow["cmd_viphal"].ToString()))
                    {
                        list.Add("cmd_viphal");
                    }
                    if (Essential.StringToBoolean(dataRow["acc_moveotheruserstodoor"].ToString()))
                    {
                        list.Add("acc_moveotheruserstodoor");
                    }
					this.dictionary_1.Add((uint)dataRow["rank"], list);
				}
			}

			dataTable = class6_0.ReadDataTable("SELECT * FROM permissions_vip;");

			if (dataTable != null)
			{
				ServerConfiguration.UnknownBoolean1 = false;
				ServerConfiguration.UnknownBoolean2 = false;
				ServerConfiguration.UnknownBoolean3 = false;
				ServerConfiguration.UnknownBoolean7 = false;
				ServerConfiguration.UnknownBoolean8 = false;
				ServerConfiguration.UnknownBoolean9 = false;

				foreach (DataRow dataRow in dataTable.Rows)
				{
					if (Essential.StringToBoolean(dataRow["cmdPush"].ToString()))
						ServerConfiguration.UnknownBoolean1 = true;

					if (Essential.StringToBoolean(dataRow["cmdPull"].ToString()))
						ServerConfiguration.UnknownBoolean2 = true;

					if (Essential.StringToBoolean(dataRow["cmdFlagme"].ToString()))
						ServerConfiguration.UnknownBoolean3 = true;

					if (Essential.StringToBoolean(dataRow["cmdMimic"].ToString()))
						ServerConfiguration.UnknownBoolean7 = true;

					if (Essential.StringToBoolean(dataRow["cmdMoonwalk"].ToString()))
						ServerConfiguration.UnknownBoolean8 = true;

					if (Essential.StringToBoolean(dataRow["cmdFollow"].ToString()))
						ServerConfiguration.UnknownBoolean9 = true;
				}
			}
			this.dictionary_5.Clear();
			this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_free"), 0);
			this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_sit"), 1);
			this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_down"), 2);
			this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_here"), 3);
			this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_beg"), 4);
			this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_play_dead"), 5);
			this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_stay"), 6);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_follow"), 7);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_stand"), 8);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_jump"), 9);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_speak"), 10);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_play"), 11);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_silent"), 12);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_nest"), 13);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_drink"), 14);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_follow_left"), 15);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_follow_right"), 16);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_play_football"), 17);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_move_forwar"), 24);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_turn_left"), 25);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_turn_right"), 26);
            this.dictionary_5.Add(EssentialEnvironment.GetExternalText("pet_cmd_eat"), 43);
			this.dictionary_4.Clear();
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_about_name"), 1);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_alert_name"), 2);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_award_name"), 3);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_ban_name"), 4);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_buy_name"), 5);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_coins_name"), 6);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_coords_name"), 7);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_disablediagonal_name"), 8);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_emptyitems_name"), 9);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_empty_name"), 10);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_enable_name"), 11);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_flagme_name"), 12);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_follow_name"), 13);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_freeze_name"), 14);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_givebadge_name"), 15);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_globalcredits_name"), 16);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_globalpixels_name"), 17);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_globalpoints_name"), 18);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_hal_name"), 19);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_ha_name"), 20);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_invisible_name"), 21);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_ipban_name"), 22);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_kick_name"), 23);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_massbadge_name"), 24);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_masscredits_name"), 25);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_masspixels_name"), 26);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_masspoints_name"), 27);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_mimic_name"), 28);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_moonwalk_name"), 29);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_motd_name"), 30);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_mute_name"), 31);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_override_name"), 32);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_pickall_name"), 33);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_pixels_name"), 34);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_points_name"), 35);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_pull_name"), 36);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_push_name"), 37);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_redeemcreds_name"), 38);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_removebadge_name"), 39);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_ride_name"), 40);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_roomalert_name"), 41);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_roombadge_name"), 42);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_roomkick_name"), 43);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_roommute_name"), 44);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_sa_name"), 45);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_setmax_name"), 46);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_setspeed_name"), 47);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_shutdown_name"), 48);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_spull_name"), 49);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_summon_name"), 50);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_superban_name"), 51);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_teleport_name"), 52);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_unload_name"), 53);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_unmute_name"), 54);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_update_achievements_name"), 55);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_update_bans_name"), 56);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_update_bots_name"), 57);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_update_catalogue_name"), 58);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_update_filter_name"), 59);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_update_items_name"), 60);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_update_navigator_name"), 61);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_update_permissions_name"), 62);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_update_settings_name"), 63);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_userinfo_name"), 64);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_update_texts_name"), 65);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_disconnect_name"), 66);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_commands_name"), 67);
			this.dictionary_4.Add("about", 68);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_roominfo_name"), 69);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_dance_name"), 71);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_rave_name"), 72);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_roll_name"), 73);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_control_name"), 74);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_makesay_name"), 75);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_sitdown_name"), 76);
            this.dictionary_4.Add("exe", 77);
			this.dictionary_4.Add("giveitem", 79);
			this.dictionary_4.Add("sit", 80);
			this.dictionary_4.Add("dismount", 81);
			this.dictionary_4.Add("getoff", 82);
			this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_emptypets_name"), 83);
            this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_startquestion_name"), 94);
            this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_lay_name"), 86);
           // this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_handitem_name"), 85);
            this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_spush_name"), 88);
            this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_roomeffect_name"), 91);
            this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_redeempixel_name"), 95);
            this.dictionary_4.Add(EssentialEnvironment.GetExternalText("cmd_redeemshell_name"), 96);
            this.dictionary_4.Add("kuss", 97);
            if (config.getData("cmd.sellroom.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.sellroom.name"), 100);

            if (config.getData("cmd.buyroom.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.buyroom.name"), 101);

            if (config.getData("cmd.handitem.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.handitem.name"), 104);

            if (config.getData("cmd.hipster.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.hipster.name"), 105);

            if (config.getData("cmd.noob.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.noob.name"), 106);

            if (config.getData("cmd.bkg.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.bkg.name"), 107);

            if (config.getData("cmd.drive.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.drive.name"), 109);

            if (config.getData("cmd.roomalert.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.roomalert.name"), 110);

            if (config.getData("cmd.makemedance.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.makemedance.name"), 111);

            if (config.getData("cmd.rotate.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.rotate.name"), 112);

            if (config.getData("cmd.faceless.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.faceless.name"), 114);

            if (config.getData("cmd.roomfreeze.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.roomfreeze.name"), 115);

            if (config.getData("cmd.habnam.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.habnam.name"), 120);

            if (config.getData("cmd.super.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.super.name"), 121);

            if (config.getData("cmd.laydown.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.laydown.name"), 122);


            if (config.getData("cmd.afk.enabled") == "1")
            {
                this.dictionary_4.Add(config.getData("cmd.afk.name").Split('|')[0], 124);
                this.dictionary_4.Add(config.getData("cmd.afk.name").Split('|')[1], 124);
            }
            if (config.getData("cmd.cpu.enabled") == "1")
                this.dictionary_4.Add("cpu", 128);
            if (config.getData("cmd.slap.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.slap.name"), 131);
            if (config.getData("cmd.miau.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.miau.name"), 123);
            if (config.getData("cmd.staff.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.staff.name"), 132);
            if (config.getData("cmd.howmanyrooms.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.howmanyrooms.name"), 133);
            if (config.getData("cmd.protect.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.protect.name"), 134);
            if (config.getData("cmd.trade.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.trade.name"), 135);
            if (config.getData("cmd.werber.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.werber.name"), 136);
            if (config.getData("cmd.customhotelalert.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.customhotelalert.name"), 137);
            if (config.getData("cmd.toggletrade.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.toggletrade.name"), 138);
            if (config.getData("cmd.eingang.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.eingang.name"), 139);
            if (config.getData("cmd.homeroom.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.homeroom.name"), 140);
            if (config.getData("cmd.infocenter.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.infocenter.name"), 141);
            if (config.getData("cmd.petcmds.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.petcmds.name"), 142);
            if (config.getData("cmd.eventha.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.eventha.name"), 143);
            if (config.getData("cmd.backup.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.backup.name"), 144);
            if (config.getData("cmd.emptybots.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.emptybots.name"), 145);
            if (config.getData("cmd.looktome.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.looktome.name"), 146);
            if (config.getData("cmd.stand.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.stand.name"), 147);
            if (config.getData("cmd.mutepets.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.mutepets.name"), 148);
            if (config.getData("cmd.mutebots.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.mutebots.name"), 149);
            if (config.getData("cmd.kickpets.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.kickpets.name"), 150);
            if (config.getData("cmd.kickbots.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.kickbots.name"), 151);
            if (config.getData("cmd.roompush.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.roompush.name"), 152);
            if (config.getData("cmd.roompull.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.roompull.name"), 153);
            if (config.getData("cmd.roomenable.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.roomenable.name"), 154);
            if (config.getData("cmd.roomrespect.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.roomrespect.name"), 155);
            if (config.getData("cmd.disablegiftalerts.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.disablegiftalerts.name"), 156);
            if (config.getData("cmd.disablemimic.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.disablemimic.name"), 157);
            if (config.getData("cmd.deletegroup.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.deletegroup.name"), 158);
            if (config.getData("cmd.enablewalkunder.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.enablewalkunder.name"), 159);
            if (config.getData("cmd.disablewalkunder.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.disablewalkunder.name"), 160);
            if (config.getData("cmd.aws.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.aws.name"), 161);
            if (config.getData("cmd.hug.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.hug.name"), 162);
            if (config.getData("cmd.punch.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.punch.name"), 163);
            if (config.getData("cmd.eventwin.enabled") == "1")
                this.dictionary_4.Add(config.getData("cmd.eventwin.name"), 164);
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}
		public bool HasFuse(uint uint_0, string string_0)
		{
			bool result;
			/*if (Essential.Length == 0)
			{
				result = false;
			}
			else*/
			{
                if (!this.RankExists(uint_0))
				{
					result = false;
				}
				else
				{
					List<string> list = this.dictionary_1[uint_0];
					result = list.Contains(string_0);
				}
			}
			return result;
		}
		public int GetFloodTime(uint uint_0)
		{
			return this.dictionary_3[uint_0];
		}
		public bool HasSpecialFuse(uint uint_0, string string_0)
		{
			bool result;
            if (!this.HasSpecialFuse(uint_0))
			{
				result = false;
			}
			else
			{
				List<string> list = this.dictionary_0[uint_0];
				result = list.Contains(string_0);
			}
			return result;
		}
		public bool HasSpecialFuse(uint uint_0)
		{
			return this.dictionary_0.ContainsKey(uint_0);
		}
		public bool RankExists(uint uint_0)
		{
			return this.dictionary_1.ContainsKey(uint_0);
		}
		public string GetBadgeByRank(uint uint_0)
		{
            if (this.dictionary_2.ContainsKey(uint_0))
            {
                return this.dictionary_2[uint_0];
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nCan't find rank: " + uint_0);
                Console.ForegroundColor = ConsoleColor.Gray;
                return "error";
            }
		}
		public int GetRankCount()
		{
			return this.dictionary_2.Count;
		}
		public void ClearDictionaries()
		{
			this.dictionary_2.Clear();
			this.dictionary_0.Clear();
			this.dictionary_1.Clear();
			this.dictionary_3.Clear();
		}
		public bool HasSuperWiredcndFuse(string string_0, GameClient Session)
		{
			bool result;
			switch (string_0)
			{
			case "roomuserseq":
			case "roomuserslt":
			case "roomusersmt":
			case "roomusersmte":
			case "roomuserslte":
				if (Session.GetHabbo().HasFuse("wired_cnd_roomusers"))
				{
					result = true;
					return result;
				}
				break;
			case "userhasachievement":
			case "userhasntachievement":
				if (Session.GetHabbo().HasFuse("wired_cnd_userhasachievement"))
				{
					result = true;
					return result;
				}
				break;
			case "userhasbadge":
			case "userhasntbadge":
				if (Session.GetHabbo().HasFuse("wired_cnd_userhasbadge"))
				{
					result = true;
					return result;
				}
				break;
			case "userhasvip":
			case "userhasntvip":
				if (Session.GetHabbo().HasFuse("wired_cnd_userhasvip"))
				{
					result = true;
					return result;
				}
				break;
			case "userhaseffect":
			case "userhasnteffect":
				if (Session.GetHabbo().HasFuse("wired_cnd_userhaseffect"))
				{
					result = true;
					return result;
				}
				break;
			case "userrankeq":
			case "userrankmt":
			case "userrankmte":
			case "userranklt":
			case "userranklte":
				if (Session.GetHabbo().HasFuse("wired_cnd_userrank"))
				{
					result = true;
					return result;
				}
				break;
			case "usercreditseq":
			case "usercreditsmt":
			case "usercreditsmte":
			case "usercreditslt":
			case "usercreditslte":
				if (Session.GetHabbo().HasFuse("wired_cnd_usercredits"))
				{
					result = true;
					return result;
				}
				break;
			case "userpixelseq":
			case "userpixelsmt":
			case "userpixelsmte":
			case "userpixelslt":
			case "userpixelslte":
				if (Session.GetHabbo().HasFuse("wired_cnd_userpixels"))
				{
					result = true;
					return result;
				}
				break;
			case "userpointseq":
			case "userpointsmt":
			case "userpointsmte":
			case "userpointslt":
			case "userpointslte":
				if (Session.GetHabbo().HasFuse("wired_cnd_userpoints"))
				{
					result = true;
					return result;
				}
				break;
			case "usergroupeq":
			case "userisingroup":
				if (Session.GetHabbo().HasFuse("wired_cnd_usergroups"))
				{
					result = true;
					return result;
				}
				break;
			case "wearing":
			case "notwearing":
				if (Session.GetHabbo().HasFuse("wired_cnd_wearing"))
				{
					result = true;
					return result;
				}
				break;
			case "carrying":
			case "notcarrying":
				if (Session.GetHabbo().HasFuse("wired_cnd_carrying"))
				{
					result = true;
					return result;
				}
				break;
            case "wiredactived":
            case "notwiredactived":
                if (Session.GetHabbo().HasFuse("wired_cnd_wiredactived"))
                {
                    result = true;
                    return result;
                }
                break;
                case "onlinecount":
                    if(Session.GetHabbo().Rank >= int.Parse(config.getData("wf.cnd.onlinecount.minrank")))
                    {
                        return true;
                    }
                    break;
			}
			result = false;
			return result;
		}
		public bool HasSuperWiredFXFuse(string string_0, GameClient Session)
		{
			bool result;
			switch (string_0)
			{
			case "sql":
				if (Session.GetHabbo().HasFuse("wired_give_sql"))
				{
					result = true;
					return result;
				}
				break;
			case "badge":
				if (Session.GetHabbo().HasFuse("wired_give_badge"))
				{
					result = true;
					return result;
				}
				break;
			case "effect":
				if (Session.GetHabbo().HasFuse("wired_give_effect"))
				{
					result = true;
					return result;
				}
				break;
			case "award":
				if (Session.GetHabbo().HasFuse("wired_give_award"))
				{
					result = true;
					return result;
				}
				break;
			case "dance":
				if (Session.GetHabbo().HasFuse("wired_give_dance"))
				{
					result = true;
					return result;
				}
				break;
			case "send":
				if (Session.GetHabbo().HasFuse("wired_give_send"))
				{
					result = true;
					return result;
				}
				break;
			case "credits":
				if (Session.GetHabbo().HasFuse("wired_give_credits"))
				{
					result = true;
					return result;
				}
				break;
			case "pixels":
				if (Session.GetHabbo().HasFuse("wired_give_pixels"))
				{
					result = true;
					return result;
				}
				break;
			case "points":
				if (Session.GetHabbo().HasFuse("wired_give_points"))
				{
					result = true;
					return result;
				}
				break;
			case "rank":
				if (Session.GetHabbo().HasFuse("wired_give_rank"))
				{
					result = true;
					return result;
				}
				break;
			case "respect":
				if (Session.GetHabbo().HasFuse("wired_give_respect"))
				{
					result = true;
					return result;
				}
				break;
			case "handitem":
				if (Session.GetHabbo().HasFuse("wired_give_handitem"))
				{
					result = true;
					return result;
				}
				break;
			case "alert":
				if (Session.GetHabbo().HasFuse("wired_give_alert"))
				{
					result = true;
					return result;
				}
				break;
            case "wiredactived":
                if (Session.GetHabbo().HasFuse("wired_give_wiredactived"))
                {
                    result = true;
                    return result;
                }
                break;
            case "item":
                if(Session.GetHabbo().HasFuse("wired_give_award"))
                    return true;
                break;
            case "enable":
                if(Session.GetHabbo().HasFuse("wired_give_effect"))
                    return true;
                break;
			}

			result = false;
			return result;
		}
	}
}