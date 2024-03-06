using Godot;
using System;
using System.Reflection;

namespace GDpsx_API.EventSystem 
{ 
    public partial class GDpsx_ES_FunctionNodeLibrary : Node
    {
        public Godot.Collections.Array<Variant> currentParameters = new Godot.Collections.Array<Variant>();
        public void DisplayMessageBox(string Message, int TypeSpeedMS, int MessageHoldTimeMS){}
        public void DamagePlayer(int Damage_To_Apply)
        {
            
           
            FPS_HeroMovement player = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree()) as FPS_HeroMovement;
            player.TakeDamage((double)Damage_To_Apply, (Node3D)player);
        }
        public void GivePlayerItem(string ItemName, int Amount)
        {   
            var _itemName = currentParameters[0].AsStringName().ToString();
            var _amount = currentParameters[1].AsInt32();
            FPS_HeroMovement player = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree()) as FPS_HeroMovement;
            player.Inventory.AddItem(ItemName, Amount);
        }
        public void ToggleLight(bool value){}

    }
}