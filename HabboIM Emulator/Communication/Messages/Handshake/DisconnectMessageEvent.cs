using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Handshake
{
	internal sealed class DisconnectMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
		}
	}
}
