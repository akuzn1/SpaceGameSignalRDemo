using SpaceGameDataModel;

namespace DataEraser
{
	class Program
	{
		static void Main(string[] args)
		{
			var dbPath = "..\\..\\..\\..\\data.db";
			using (DataContext context = new DataContext(dbPath, true))
			{

			}
		}
	}
}
