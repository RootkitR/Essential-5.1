using System;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Catalogs
{
	internal sealed class VoucherHandler
	{
		public bool VoucherExists(string string_0)
		{
			bool result;
			using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("code", string_0);
				if (adapter.ReadDataRow("SELECT null FROM vouchers WHERE code = @code LIMIT 1") != null)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		public void DeleteVoucher(string string_0)
		{
			using (DatabaseClient @class = Essential.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("code", string_0);
				@class.ExecuteQuery("DELETE FROM vouchers WHERE code = @code LIMIT 1");
			}
		}
        public void LogVoucher(GameClient Session, string Code)
        {

        }
		public void HandleVoucher(GameClient Session, string string_0)
		{
			if (!this.VoucherExists(string_0))
			{
                ServerMessage Message = new ServerMessage(Outgoing.VoucherRedeemError);
                Message.AppendString("1");
				Session.SendMessage(Message);
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient @class = Essential.GetDatabase().GetClient())
				{
					@class.AddParamWithValue("code", string_0);
					dataRow = @class.ReadDataRow("SELECT * FROM vouchers WHERE code = @code LIMIT 1");
				}
				int num = (int)dataRow["credits"];
				int num2 = (int)dataRow["pixels"];
				int num3 = (int)dataRow["vip_points"];

                this.LogVoucher(Session, string_0);
				this.DeleteVoucher(string_0);
				if (num > 0)
				{
					Session.GetHabbo().GiveCredits(num, "Reedem Voucher");
					Session.GetHabbo().UpdateCredits(true);
				}
				if (num2 > 0)
				{
					Session.GetHabbo().ActivityPoints += num2;
					Session.GetHabbo().UpdateActivityPoints(true);
				}
				if (num3 > 0)
				{
					Session.GetHabbo().VipPoints += num3;
					Session.GetHabbo().UpdateVipPoints(false, true);
				}
                ServerMessage Voucher = new ServerMessage(Outgoing.VoucherRedeemOk);
                Voucher.AppendString("");
                Voucher.AppendString("");
                Session.SendMessage(Voucher);
			}
		}
	}
}
