using GDpsx_API;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class GDpsx_InventoryUI : Control
{
    [Export] public GDpsx_Inventory Inventory;
    [Export] public GridContainer InventoryGrid;
    [Export] public PackedScene itemSlot;
    [Export] public Label itemNameLabel;
    [Export] public Label itemDescLabel;
    protected bool BeingShown = false;

    public void ToggleInventory()
    {
        if(!BeingShown)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }

    private void CloseInventory()
    {
        var player = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree()) as GDpsx_HeroMovementBase;
        player.SetMouseMode(Input.MouseModeEnum.Captured);
        BeingShown = false;
        Visible = false;
        EmptyInventory();
    }

    public void ShowItemDetails(GDpsx_Item itemData)
    {
        if(itemData != null)
        {
            itemNameLabel.Text = itemData.itemName;
            itemDescLabel.Text = itemData.itemDescription;
        }
        else
        {
            itemNameLabel.Text = "";
            itemDescLabel.Text = "";
        }
    }

    private void OpenInventory()
    {
        var player = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree()) as GDpsx_HeroMovementBase;
        player.SetMouseMode(Input.MouseModeEnum.Confined);
        BeingShown = true;
        Visible = true;
        PopulateInventory();
    }

    private void RefreshInventory()
    {
        EmptyInventory();
        PopulateInventory();

    }

    private void EmptyInventory()
    {
        foreach(var child in InventoryGrid.GetChildren())
        {
            child.QueueFree();
        }
    }

    private void PopulateInventory()
    {
        foreach(var item in Inventory.Contents)
        {
            var Slot = itemSlot.Instantiate() as GDpsx_InventoryUI_ItemSlot;
            Slot.Inventory = Inventory;
            Slot.itemName = item.itemName;
            Slot.amount = item.amount;
            Slot.Init();
            InventoryGrid.AddChild(Slot);
        }
    }
}