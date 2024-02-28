using GDpsx_API;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class GDpsx_InventoryUI_ItemSlot : Control
{
    [Export] public string itemName;
    [Export] public int amount;
    [Export] public TextureRect Icon;
    [Export] public Label amountLabel;
    [Export] public GDpsx_Inventory Inventory;

    public override void _Ready()
    {
       

    }
    public void Init()
    {
        GDpsx_Item itemData = Inventory.GlobalItemList.GetItemData(itemName);
        Icon.Texture = itemData.itemIcon;
        amountLabel.Text = amount.ToString();
    }

    public void ParseItemDetails()
    {
        
        GDpsx_Item itemData = Inventory.GlobalItemList.GetItemData(itemName);
        Inventory.InventoryUI.ShowItemDetails(itemData);
    }

    public void HideItemDetails()
    {
        Inventory.InventoryUI.ShowItemDetails(null);
    }
}