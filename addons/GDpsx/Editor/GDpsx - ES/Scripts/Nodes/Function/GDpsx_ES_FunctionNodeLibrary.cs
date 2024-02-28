using GDpsx_API.EventSystem;
using Godot;
using System;
using System.Reflection;

[Tool]
public partial class GDpsx_Function_Node_Box : HBoxContainer
{
    [Export] public MenuButton functionMenu;
    public override void _Ready()
    {
        Type type = typeof(GDpsx_ES_FunctionLibrary);
        MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public| BindingFlags.Instance | BindingFlags.DeclaredOnly);
        GD.Print(methodInfos.Length);
        foreach (var method in methodInfos)
        {
            // Start building the string with the method name
            string methodDetails = method.Name + "(";

            // Get parameters of the method
            ParameterInfo[] parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo param = parameters[i];
                // Append parameter type and name
                methodDetails += param.ParameterType.Name + " " + param.Name;

                // If not the last parameter, add a comma and space
                if (i < parameters.Length - 1)
                {
                    methodDetails += ", ";
                }
            }

            // Close the parentheses and print
            methodDetails += ")";
            GD.Print(methodDetails);
        }
    }
}

