using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SpaceGameSignalRDemo.Model
{
	public class SpaceObject
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public ObjectType Type { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public int Dx { get; set; }

		public int Dy { get; set; }

		public int Speed { get; set; }

		public int Life { get; set; }

		public int Level { get; set; }

		public bool Visible { get; set; }

		[IgnoreDataMember]
		public Player Player { get; set; }
	}
}
