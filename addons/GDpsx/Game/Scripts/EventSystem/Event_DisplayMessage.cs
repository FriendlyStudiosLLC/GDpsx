using Godot;
using System;
using System.Reflection.Metadata;


[Tool][GlobalClass]
public partial class Event_DisplayMessage : GDpsx_InteractionEventBase
{
    
    [Export(PropertyHint.MultilineText)] public string Message = "The door is locked";
    [Export] public int TypeSpeedMS = 25;
    [Export] public int MessageHoldTimeMS = 2000;
    public override void Enter(SceneTree tree = null, GDpsx_GameObject instigator = null)
    {
        FPS_HeroMovement player = GDpsx_API.GDpsx_Utility.GetPlayer(tree) as FPS_HeroMovement;
        player.messageBox.StartTyping(Message, TypeSpeedMS, MessageHoldTimeMS);
    }
}
