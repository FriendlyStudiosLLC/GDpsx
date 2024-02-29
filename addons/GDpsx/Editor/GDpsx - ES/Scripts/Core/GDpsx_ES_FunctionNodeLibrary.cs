using Godot;
using System;
using System.Reflection;

namespace GDpsx_API.EventSystem
{ 
    public partial class GDpsx_ES_FunctionNodeLibrary : Node
    {
        public void DisplayMessageBox(string Message, int TypeSpeedMS, int MessageHoldTimeMS){}
        public void DamagePlayer(int Damage_To_Apply){}
        public void GivePlayerItem(string ItemName, int Amount){}

    }
}