using Godot;
using System;
using System.Reflection;

namespace GDpsx_API.EventSystem 
{ 
    public partial class GDpsx_ES_ConditionalNodeLibrary : Node
    {
        public bool PlayerHasItem(string itemName, int amount)
        {
            FPS_HeroMovement player = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree()) as FPS_HeroMovement;
            return player.Inventory.HasItem(itemName);
        }
        public bool PlayerStatEquals(string statName, double value){return false;}
        public bool PlayerStatGreaterThan(string statName, double value){return false;}
        public bool PlayerStatLessThan(string statName, double value){return false;}
        public bool ObjectSolved(string objectName){return false;}
        



    }
}