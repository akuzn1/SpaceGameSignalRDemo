using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceGameSignalRDemo.Model
{
	public class Player
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Login { get; set; }

		public int Expirience { get; set; }

		[ForeignKey("SpaceObject")]
		public Guid SpaceObjectId { get; set; }

		public SpaceObject SpaceObject { get; set; }
	}
}
