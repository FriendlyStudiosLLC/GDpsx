using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts.Inventory
{
	[Tool]
	public partial class GDpsx_InventoryItem : Resource
	{
		[Export] public string itemName;
		[Export] public int amount;
		public GDpsx_InventoryItem(string ItemName, int Amount)
		{
			itemName = ItemName;
			amount = Amount;
		}
	}
}
