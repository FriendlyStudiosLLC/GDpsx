using Godot;
using System;
using System.Reflection.Metadata;


[Tool][GlobalClass]
public partial class Event_ToggleLight : GDpsx_InteractionEventBase
{
    [Export] public bool On;
    public override void Enter(SceneTree tree = null, GDpsx_GameObject instigator = null)
    {
        var TargetEntity = instigator.GetNodeFromPath(Target_Entity_Path);
        
        OmniLight3D light = TargetEntity as OmniLight3D;
        if(light != null) light.Visible = On;
    }
}
