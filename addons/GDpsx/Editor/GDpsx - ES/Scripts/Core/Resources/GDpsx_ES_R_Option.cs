using GDpsx_API.EventSystem;
using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;

[Tool]
public partial class GDpsx_ES_R_Option : GDpsx_ES_R_Node
{
	[Export] public string OptionText;
	[Export] public bool hasRequirement;
	[Export] public string methodName;
	[Export] public Array<Variant> parameterList;

	public GDpsx_ES_R_Option(string MethodName, Array<Variant> ParameterList)
	{
		methodName = MethodName;
		parameterList = ParameterList;
	}
	public GDpsx_ES_R_Option()
	{
	}


}
