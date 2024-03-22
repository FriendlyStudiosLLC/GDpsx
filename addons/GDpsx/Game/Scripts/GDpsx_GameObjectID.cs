namespace GDpsx_Project.addons.GDpsx.Game.Scripts
{
	public class GDpsx_GameObjectID
	{
		private string _value;
		public GDpsx_GameObjectID(string value)
		{
			_value = value;
		}
		public override string ToString()
		{
			return _value;
		}
		// Add other methods as needed to mimic System.String functionality
	}
}