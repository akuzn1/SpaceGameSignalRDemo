using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceGameSignalRDemo.Model
{
	public class GameState
	{
		public Player Player { get; internal set; }
		public List<SpaceObject> SpaceObjects { get; internal set; }
	}
}
