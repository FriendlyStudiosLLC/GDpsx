using Godot;
using System;
using System.Drawing;
using Godot.Collections;

[Tool][GlobalClass]
public partial class GDpsx_GameBrain : Resource
{
    [Export] public Array<GDpsx_Character> Characters = new Array<GDpsx_Character>();
    [Export] public Array<GDpsx_Item> Items;
    [Export] public Dictionary<string, Vector3> Locations = new Dictionary<string, Vector3>();


    public GDpsx_Item GetItemData(string itemName)
    {
        for(int i = 0; i < Items.Count; i++)
        {
            if (Items[i].itemName == itemName)
            {
                return Items[i];
            }
        }
        return null;
    }
}