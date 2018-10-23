using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

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

		public int Width { get; set; }

		public int Height { get; set; }

		public int TargetX { get; set; }

		public int TargetY { get; set; }

		public int Speed { get; set; }

		public int Life { get; set; }

		public int Level { get; set; }

		public bool Visible { get; set; }

		[IgnoreDataMember]
		public Player Player { get; set; }
	}
}
