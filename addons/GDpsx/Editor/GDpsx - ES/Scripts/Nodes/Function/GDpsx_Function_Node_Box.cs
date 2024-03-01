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
    public string functionName;
    public override void _Ready()
    {
        
        functionMenu.GetPopup().IndexPressed += FunctionMenuSelected;
        Type type = typeof(GDpsx_ES_FunctionNodeLibrary);
        MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        GD.Print($"Number of methods: {methodInfos.Length}");
        foreach (var method in methodInfos)
        {
           functionMenu.GetPopup().AddItem(method.Name);
        }
        
    }

    public void FunctionMenuSelected(long index)
    {
        var i_index = (int)index;
        ClearContainer();
        functionMenu.Text = functionMenu.GetPopup().GetItemText(i_index);
        var method = SelectMethod(i_index);
        functionName = method.Name;
        ExtractParametersFromMethod(method);

    }

    public MethodInfo SelectMethod(int methodIndex)
    {
        Type type = typeof(GDpsx_ES_FunctionNodeLibrary);
        MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        return methodInfos[methodIndex];
    }


    private void ClearContainer()
    {
        foreach(var child in container.GetChildren())
        {
            child.QueueFree();
        }
    }

    public void UpdateTextEdit(TextEdit stringObj)
    {
        var f = stringObj.GetThemeDefaultFont();
        var textSize = new Vector2();
        if(stringObj.Text.Length != 0)
        {
            textSize.X = f.GetStringSize(stringObj.Text).X;
            textSize.Y = stringObj.GetLineCount();
            stringObj.CustomMinimumSize = new Vector2(textSize.X + 32, textSize.Y * 32);
        }
        else
        {
            stringObj.CustomMinimumSize = new Vector2(250, 32);
        }
        
    }


    private HBoxContainer ParameterFactory(string ParameterName, Type type)
    {
        HBoxContainer hBox = new HBoxContainer();
        hBox.Name = "ParameterContainer";
        Label label = new Label();
        label.HorizontalAlignment = HorizontalAlignment.Fill;
        label.Text = ParameterName;
        hBox.AddChild(label);
        hBox.AddChild(DynamicParameter(type));
        return hBox;
    }

    private Control DynamicParameter(Type type)
    {
        switch (type.FullName)
        {
            case "System.Int32":
                SpinBox intObj = new SpinBox();
                intObj.Name = type.FullName;
                intObj.Rounded = true;
                return intObj;
            case "System.String":
                TextEdit stringObj = new TextEdit();
                stringObj.Name = type.FullName;
                stringObj.CustomMinimumSize = new Vector2(250, 32);
                stringObj.TextChanged += () => UpdateTextEdit(stringObj);
                return stringObj;
            case "System.Boolean":
                CheckBox boolObj = new CheckBox();
                boolObj.Name = type.FullName;
                return boolObj;
            case "System.Double":
                SpinBox doubleObj = new SpinBox();
                doubleObj.Name = type.FullName;
                doubleObj.Rounded = false;
                return doubleObj;
        }
        return null;
    }


    public void ExtractParametersFromMethod(MethodInfo method)
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
                    string paramName = i.ToString() + "_" + parameterInfo[i].Name;
                    
                    container.AddChild(ParameterFactory(paramName, paramType));
                    GD.Print($"Parameter {i+1}: {paramName}, Parameter Type: {paramType}");
                }
            }
            else
            {
                GD.Print($"Method: {method.Name} has no parameters");
            }
    }
}