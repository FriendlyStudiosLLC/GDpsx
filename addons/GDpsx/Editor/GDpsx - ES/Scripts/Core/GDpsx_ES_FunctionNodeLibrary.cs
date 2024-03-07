using Godot;
using System;
using System.Reflection;

namespace GDpsx_API.EventSystem 
{ 
    public partial class GDpsx_ES_FunctionNodeLibrary : Node
    {
        public void DisplayMessageBox(string Message, int TypeSpeedMS, int MessageHoldTimeMS){}
        public void DamagePlayer(int Damage_To_Apply)
        {
            var player = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree());
            player.TakeDamage((double)Damage_To_Apply, (Node3D)player);
        }
        public void GivePlayerItem(string ItemName, int Amount)
        {   
            var player = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree());
            player.Inventory.AddItem(ItemName, Amount);
        }
        public void ToggleLight(bool value){}


        public void TestSecondFunction(bool value, int number){}
    }
}