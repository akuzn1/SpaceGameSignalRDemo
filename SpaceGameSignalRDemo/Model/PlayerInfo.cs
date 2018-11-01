using SpaceGameDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceGameSignalRDemo.Model
{
	public class PlayerInfo
	{
		public PlayerInfo(Player player)
		{
			Id = player.Id;
			Login = player.Login;
			Expirience = player.Expirience;
			SpaceLevel = player.SpaceLevel;
			ShipId = player.ShipId;
		}

		public Guid Id { get; set; }

		public string Login { get; set; }

		public int Expirience { get; set; }

		public int SpaceLevel { get; set; }

		public Guid ShipId { get; set; }
	}
}
