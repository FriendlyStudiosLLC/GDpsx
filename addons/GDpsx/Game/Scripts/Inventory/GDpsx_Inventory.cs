using GDpsx_API;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;



public partial class GDpsx_Inventory : Node
{
    [Export] public Array<GDpsx_InventoryItem> Contents = new Array<GDpsx_InventoryItem>();
    [Export] public int MaximumSize = 8;
    [Export] public GDpsx_ItemDatabase GlobalItemList;
    [Export] public GDpsx_InventoryUI InventoryUI;


    public bool AddItem(string itemName, int amount)
    {
        GDpsx_Item ItemData = GlobalItemList.GetItemData(itemName);
        foreach (var invItem in Contents)
        {
            // Check if the item exists and can stack more
            if (invItem.itemName == itemName && invItem.amount < ItemData.maxStackSize)
            {
                int possibleToAdd = ItemData.maxStackSize - invItem.amount;
                int toAdd = Math.Min(possibleToAdd, amount);

                invItem.amount += toAdd;
                amount -= toAdd;

                if (amount <= 0)
                    //_pickupUI.AddPickup(item.ItemName, toAdd);
                    return true; // Item fully added
            }
        }

        // If item was not fully added, or doesn't exist in the inventory, add it to a new slot if possible
        if (Contents.Count < MaximumSize)
        {
            int toAdd = Math.Min(ItemData.maxStackSize, amount);
            Contents.Add(new GDpsx_InventoryItem(itemName, toAdd));
            amount -= toAdd;

            // If there are still items left, this means the inventory is full and the item cannot be fully added
            if (amount > 0)
            {
                // Optionally handle the leftover items (e.g., notify the user the inventory is full)
                GD.Print("Not all items could be added, inventory is full.");
                return false;
            }
            //_pickupUI.AddPickup(item.ItemName, toAdd);
            
            GD.Print($"Added {amount} ||| {itemName} to a new slot.");
            return true;
        }

        GD.Print("Inventory is full.");
        return false;
    }


    public bool RemoveItem(string itemName, int amount = 1)
    {
        for (int i = 0; i < Contents.Count; i++)
        {
            var _item = Contents[i];
            // Check if the item exists
            if (_item.itemName == itemName)
            {
                if (_item.amount > amount)
                {
                    // If the slot has more items than we need to remove, just decrease the amount
                    _item.amount -= amount;
                    GD.Print($"Removing {amount} of {itemName}");
                    return true; // Item(s) successfully removed
                }
                else if (_item.amount == amount)
                {
                    // If the slot has exactly the amount to remove, delete the slot entirely
                    Contents.RemoveAt(i);
                    GD.Print($"Removing {amount} of {itemName}");
                    return true; // Item(s) successfully removed
                }
                // If the slot has less than the amountToRemove, this block could be extended to handle such cases
            }
        }

        // If the function hasn't returned by now, it means the item wasn't found or there wasn't enough to remove
        GD.Print("Item not found or not enough quantity to remove.");
        return false;
    }

    public void TransferItem(GDpsx_Inventory sourceInventory, GDpsx_Inventory destinationInventory, string itemName, int amount)
    {
        
        sourceInventory.RemoveItem(itemName, amount);
        destinationInventory.AddItem(itemName, amount);
    }


    public bool HasItem (string itemName)
    {
        foreach(var item in Contents)
        {
            if(item.itemName == itemName) return true;
        }
        return false;
    }

    
}
