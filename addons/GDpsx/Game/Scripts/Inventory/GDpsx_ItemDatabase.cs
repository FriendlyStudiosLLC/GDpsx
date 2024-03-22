using Godot;
using Godot.Collections;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts.Inventory
{
	[GlobalClass]
	[Tool]
	public partial class GDpsx_ItemDatabase : Resource
	{
		[Export] public Array<GDpsx_Item> Items;


		public GDpsx_Item GetItemData(string itemName)
		{
			for (int i = 0; i < Items.Count; i++)
			{
				if (Items[i].itemName == itemName)
				{
					return Items[i];
				}
			}
			return null;
		}

	}
}
