using GDpsx_API.EventSystem;
using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;

[Tool]
public partial class GDpsx_ES_R_StartNode : GDpsx_ES_R_Node
{
    [Export] public GraphType _graphType;
    public GDpsx_ES_R_StartNode(GraphType graphType)
    {
        _graphType = graphType;
    }
    public GDpsx_ES_R_StartNode(){}
   
}
