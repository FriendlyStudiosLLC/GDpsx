using GDpsx_API.EventSystem;
using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;

[GlobalClass]
public partial class GDpsx_ES_R_Node : Resource
{
    [Export] public string NodeName;
    [Export] public NodeType nodeType;
    [Export] public Vector2 graphPosition;
    [Export] public Godot.Collections.Array GotoNodes = new Godot.Collections.Array();
    [Export] public Godot.Collections.Array FromPorts = new Godot.Collections.Array();
     public GDpsx_ES_R_Node()
    {
    }

}
