using GDpsx_API.EventSystem;
using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;

[GlobalClass]
public partial class GDpsx_ES_R_WaitNode : GDpsx_ES_R_Node
{
   [Export] public int waitTimeMS;
}
