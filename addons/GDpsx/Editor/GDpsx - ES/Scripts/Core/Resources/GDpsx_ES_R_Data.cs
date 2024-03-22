using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;

[Tool]
[Icon("res://addons/GDpsx/Editor/Icons/InventoryComponent.png")]
public partial class GDpsx_ES_R_Data : Resource
{

	[Export] public Array<GDpsx_ES_R_Node> nodes;

	public GDpsx_ES_R_Data()
	{
	}

	public GDpsx_ES_R_Data(Array<GDpsx_ES_R_Node> Nodes)
	{
		nodes = Nodes;
	}


}
