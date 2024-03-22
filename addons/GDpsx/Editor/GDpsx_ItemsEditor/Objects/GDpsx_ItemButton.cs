using Godot;
using System;

[Tool]
public partial class GDpsx_ItemButton : Button
{
	public GDpsx_Project.addons.GDpsx.Game.Scripts.Inventory.GDpsx_Item itemData;
	[Export] public GDpsx_ItemEditor itemEditor;
}
