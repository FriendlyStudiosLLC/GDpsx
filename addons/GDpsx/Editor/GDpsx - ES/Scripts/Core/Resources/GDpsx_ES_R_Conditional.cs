using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;

[GlobalClass]
public partial class GDpsx_ES_R_Conditional : GDpsx_ES_R_Node
{
    [Export] public string methodName;
    [Export] public Array<Variant> parameterList;

    public GDpsx_ES_R_Conditional(string MethodName, Array<Variant> ParameterList)
    {
        methodName = MethodName;
        parameterList = ParameterList;
    }


    public void ExecuteMethod()
    {
        Call(methodName, parameterList);
    }
     public GDpsx_ES_R_Conditional()
    {
    }

}
