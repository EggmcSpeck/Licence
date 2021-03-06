using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Inventory.Trading
{
	internal sealed class AcceptTradingEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CanTrade)
			{
				Trade class2 = @class.method_76(Session.GetHabbo().Id);
				if (class2 != null)
				{
					class2.method_4(Session.GetHabbo().Id);
				}
			}
		}
	}
}
