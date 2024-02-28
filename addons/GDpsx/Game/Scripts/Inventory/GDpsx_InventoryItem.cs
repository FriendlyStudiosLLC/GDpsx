using GDpsx_API;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

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