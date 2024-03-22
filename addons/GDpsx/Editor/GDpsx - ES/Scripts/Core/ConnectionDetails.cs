using Godot;

namespace GDpsx_API.EventSystem
{
	public class ConnectionDetails
	{
		public StringName From { get; set; }
		public StringName To { get; set; }
		public int FromPort { get; set; }
		public int ToSlot { get; set; }
	}
}