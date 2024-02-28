using Godot;
using System;
using System.Reflection.Metadata;


[Tool][GlobalClass]
public partial class Event_DamagePlayer : GDpsx_InteractionEventBase
{
    [Export] public string ItemName;
    [Export] public int Damage_To_Apply;
    public override void Enter(SceneTree tree = null, GDpsx_GameObject instigator = null)
    {
        FPS_HeroMovement player = GDpsx_API.GDpsx_Utility.GetPlayer(tree) as FPS_HeroMovement;
        player.TakeDamage(Damage_To_Apply, (Node3D)player);
    }
}
