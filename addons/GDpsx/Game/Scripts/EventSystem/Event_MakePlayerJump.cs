using Godot;
using System;
using System.ComponentModel;


[Tool][GlobalClass]
public partial class Event_MakePlayerJump : GDpsx_InteractionEventBase
{
    public override void Enter(SceneTree tree = null, GDpsx_GameObject instigator = null)
    {
        FPS_HeroMovement player = GDpsx_API.GDpsx_Utility.GetPlayer(tree) as FPS_HeroMovement;
        player.JumpAction = true;
    }
}
