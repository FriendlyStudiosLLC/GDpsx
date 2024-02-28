using Godot;
using System;
using System.ComponentModel;

[Tool][GlobalClass]
public partial class GDpsx_InteractionEventBase : Resource
{
    //Optional, doesn't need to be populated
    
    [Export] public NodePath Target_Entity_Path { get; set; }
    
    
    [Export] public float WaitTime { get; set; }
    

    public virtual void Enter(SceneTree tree = null, GDpsx_GameObject instigator = null){} //Scene tree is an optional argument.
}