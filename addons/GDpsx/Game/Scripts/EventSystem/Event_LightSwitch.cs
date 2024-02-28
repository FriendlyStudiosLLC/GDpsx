using Godot;
using System;
using System.Reflection.Metadata;


[Tool][GlobalClass]
public partial class Event_LightSwitch : GDpsx_InteractionEventBase
{
    private bool State = true;
    public override void Enter(SceneTree tree = null, GDpsx_GameObject instigator = null)
    {
        State = !State;

        var TargetEntity = instigator.GetNodeFromPath(Target_Entity_Path);
        
        OmniLight3D light = TargetEntity as OmniLight3D;
        if(light != null) light.Visible = State;
    }
}
