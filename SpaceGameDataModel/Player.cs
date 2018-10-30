using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceGameDataModel
{
	public class Player
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string ConnectionId { get; set; }

		public string Login { get; set; }

		public int Expirience { get; set; }

		public int Level { get; set; }

		[ForeignKey("SpaceObject")]
		public Guid ShipId { get; set; }

		public SpaceObject Ship { get; set; }
		public bool Active { get; set; }
	}
}
