using GDpsx_API.EventSystem;
using Godot;
using Godot.Collections;
using System;
using System.Reflection;

[Tool]
public partial class GDpsx_Function_Node_Box : HBoxContainer
{
    [Export] Array<Control> parameterOptions = new Array<Control>();
    [Export] public VBoxContainer container;
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
                    
                    container.AddChild(SpinBoxFactory(paramName));
                    GD.Print($"Parameter {i+1}: {paramName}, Parameter Type: {paramType}");
                }
            }
            else
            {
                GD.Print($"Method: {method.Name} has no parameters");
            }
        }
    }


    private HBoxContainer ParameterFactory(string ParameterName, Type type)
    {
        HBoxContainer hBox = new HBoxContainer();
        Label label = new Label();
        label.HorizontalAlignment = HorizontalAlignment.Fill;
        label.Text = ParameterName;
        SpinBox spinBox = new SpinBox();
        spinBox.GrowHorizontal = GrowDirection.End; 
        hBox.AddChild(label);
        hBox.AddChild(spinBox);
        return hBox;
    }

    private Control DynamicParameter(Type type)
    {
        switch (type.FullName)
        {
            case "System.Int32":
                SpinBox intObj = new SpinBox();
                intObj.Rounded = true;
                return intObj;

            case "System.String":
                LineEdit stringObj = new LineEdit();
                return stringObj;
            case "System.Boolean":
                CheckBox boolObj = new CheckBox();
                return boolObj;
            case "System.Double":
                CheckBox boolObj = new CheckBox();
                return boolObj;
        }
    }
}