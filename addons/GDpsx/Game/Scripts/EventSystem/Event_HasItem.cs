using Godot;
using System;
using System.Reflection.Metadata;


[Tool][GlobalClass]
public partial class Event_HasItem : GDpsx_InteractionEventBase
{
    [Export] public string ItemName;
    [Export] public int Amount;
    public override void Enter(SceneTree tree = null, GDpsx_GameObject instigator = null)
    {
        FPS_HeroMovement player = GDpsx_API.GDpsx_Utility.GetPlayer(tree) as FPS_HeroMovement;
        if(!player.Inventory.HasItem(ItemName))
        {
            instigator.SetFailedEvent(true);
        }
    }
}
