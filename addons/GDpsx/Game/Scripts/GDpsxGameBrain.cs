using System.Linq;
using Godot;
using Godot.Collections;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts
{
	[Tool]
	[GlobalClass]
	public partial class GDpsxGameBrain : Resource
	{
		[Export] public Array<GDpsx_Character> Characters = new Array<GDpsx_Character>();
		[Export] public Array<Inventory.GDpsx_Item> Items;
		[Export] public Dictionary<string, Vector3> Locations = new Dictionary<string, Vector3>();


		private Inventory.GDpsx_Item GetItemData(string itemName)
		{
			return Items.FirstOrDefault(t => t.itemName == itemName);
		}
	}
}
