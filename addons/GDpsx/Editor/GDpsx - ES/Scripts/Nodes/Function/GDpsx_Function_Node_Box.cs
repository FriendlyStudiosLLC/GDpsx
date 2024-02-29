using GDpsx_API.EventSystem;
using Godot;
using Godot.Collections;
using System;
using System.Reflection;

[Tool]
public partial class GDpsx_Function_Node_Box : HBoxContainer
{
    [Export] Array<Control> parameterOptions = new Array<Control>();
    [Export] public MenuButton functionMenu;
    public override void _Ready()
    {
        Type type = typeof(GDpsx_ES_FunctionNodeLibrary);
        MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        GD.Print($"Number of methods: {methodInfos.Length}");
        foreach (var method in methodInfos)
        {
            ParameterInfo[] parameterInfo = method.GetParameters();
            int paramCount = parameterInfo.Length;
            if (paramCount > 0)
            {
                string methodName = method.Name;
                
                GD.Print($"Method: {methodName}, Number of Parameters: {paramCount}");
                for (int i = 0; i < paramCount; i++)
                {
                    var paramType = parameterInfo[i].ParameterType;
                    string paramName = parameterInfo[i].Name;
                    GD.Print($"Parameter {i+1}: {paramName}, Parameter Type: {paramType}");
                }
            }
            else
            {
                GD.Print($"Method: {method.Name} has no parameters");
            }
        }
    }
}