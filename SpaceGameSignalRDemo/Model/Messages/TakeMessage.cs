using System;
using SpaceGameDataModel;

namespace SpaceGameSignalRDemo.Model.Messages
{
	public class TakeMessage
	{
		public Guid RemovedObjectId { get; internal set; }
		public SpaceObject TakedBy { get; internal set; }
	}
}
