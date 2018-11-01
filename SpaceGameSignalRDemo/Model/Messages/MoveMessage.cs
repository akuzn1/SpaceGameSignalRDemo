using System;

namespace SpaceGameSignalRDemo.Model.Messages
{
	public class MoveMessage
	{
		public Guid ShipId { get; internal set; }
		public int X { get; internal set; }
		public int Y { get; internal set; }
		public int TargetX { get; internal set; }
		public int TargetY { get; internal set; }
		public int Speed { get; internal set; }
		public int Level { get; internal set; }
	}
}
