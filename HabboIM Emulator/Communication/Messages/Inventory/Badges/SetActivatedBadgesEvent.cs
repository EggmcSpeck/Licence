using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Users.Badges;
using HabboIM.Storage;
namespace HabboIM.Communication.Messages.Inventory.Badges
{
	internal sealed class SetActivatedBadgesEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetHabbo().GetBadgeComponent().ResetBadgeSlots();
			using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
			{
				@class.ExecuteQuery("UPDATE user_badges SET badge_slot = '0' WHERE user_id = '" + Session.GetHabbo().Id + "'");
				goto IL_131;
			}
			IL_52:
			int num = Event.PopWiredInt32();
			string text = Event.PopFixedString();
			if (text.Length != 0)
			{
				if (!Session.GetHabbo().GetBadgeComponent().HasBadge(text) || num < 1 || num > 5)
				{
					return;
				}
                if (Session.GetHabbo().CurrentQuestId > 0 && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "WEARBADGE")
				{
                    HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
				}
				Session.GetHabbo().GetBadgeComponent().GetBadgeByCode(text).Slot = num;
				using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
				{
					@class.AddParamWithValue("slotid", num);
					@class.AddParamWithValue("badge", text);
					@class.AddParamWithValue("userid", Session.GetHabbo().Id);
					@class.ExecuteQuery("UPDATE user_badges SET badge_slot = @slotid WHERE badge_id = @badge AND user_id = @userid LIMIT 1");
				}
			}
			IL_131:
			if (Event.RemainingLength > 0)
			{
				goto IL_52;
			}
			ServerMessage Message = new ServerMessage(228u);
			Message.AppendUInt(Session.GetHabbo().Id);
			Message.AppendInt32(Session.GetHabbo().GetBadgeComponent().VisibleBadges);
			foreach (Badge current in Session.GetHabbo().GetBadgeComponent().GetBadges())
			{
				if (current.Slot > 0)
				{
					Message.AppendInt32(current.Slot);
					Message.AppendStringWithBreak(current.Code);
				}
			}
            if (Session.GetHabbo().InRoom && HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId) != null)
			{
				HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).SendMessage(Message, null);
			}
			else
			{
				Session.SendMessage(Message);
			}
		}
	}
}
