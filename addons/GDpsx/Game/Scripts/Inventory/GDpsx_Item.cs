using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts.Inventory
{

	[GlobalClass]
	[Tool]
	public partial class GDpsx_Item : Resource
	{
		[Export] public string itemName;
		[Export(PropertyHint.MultilineText)] public string itemDescription;
		[Export] public int maxStackSize;
		[Export] public Texture2D itemIcon;
		[Export] public PackedScene pickupScene;
		[Export] public PackedScene equippedScene;
		[Export] public ItemType itemType;
	}
}
