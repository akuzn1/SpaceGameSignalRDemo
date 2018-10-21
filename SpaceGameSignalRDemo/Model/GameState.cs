using System;
using System.Collections.Generic;

namespace SpaceGameSignalRDemo.Model
{
	public class GameState
	{
		public Player Player { get; internal set; }

		public List<SpaceObject> SpaceObjects { get; internal set; }
	}
}
