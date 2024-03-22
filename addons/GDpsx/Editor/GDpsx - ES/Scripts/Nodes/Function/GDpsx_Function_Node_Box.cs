using GDpsx_API.EventSystem;
using Godot;
using Godot.Collections;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

[Tool]
public partial class GDpsx_Function_Node_Box : HBoxContainer
{
	[Export] private Array<Control> parameterOptions = new Array<Control>();
	[Export] public VBoxContainer container;
	[Export] public MenuButton functionMenu;
	public string functionName;
	public override void _Ready()
	{
		Init();

	}

	public void Init()
	{
		ClearFunctionMenu();
		functionMenu.GetPopup().IndexPressed += FunctionMenuSelected;
		Type type = typeof(GDpsx_ES_FunctionNodeLibrary);
		MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		foreach (MethodInfo method in methodInfos)
		{
			functionMenu.GetPopup().AddItem(method.Name);
		}
	}

	public void ClearFunctionMenu()
	{

		for (int i = 0; i < functionMenu.ItemCount; i++)
		{
			functionMenu.GetPopup().RemoveItem(i);
		}
	}

	public int MenuButtonIndexByString(string value)
	{
		Type type = typeof(GDpsx_ES_FunctionNodeLibrary);
		MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		for (int i = 0; i < methodInfos.Length; i++)
		{
			if (functionMenu.GetPopup().GetItemText(i) == value)
			{
				return i;
			}
		}
		return -1;
	}

	public void FunctionMenuSelected(long index)
	{
		int i_index = (int)index;
		ClearContainer();
		functionMenu.Text = functionMenu.GetPopup().GetItemText(i_index);
		MethodInfo method = SelectMethod(i_index);
		functionName = method.Name;
		ExtractParametersFromMethod(method);

	}


	public MethodInfo SelectMethod(int methodIndex)
	{
		Type type = typeof(GDpsx_ES_FunctionNodeLibrary);
		MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

		if (methodIndex >= 0 && methodIndex < methodInfos.Length) // Check if index is within bounds
		{
			return methodInfos[methodIndex];
		}
		else
		{
			throw new ArgumentOutOfRangeException(nameof(methodIndex), "The method index is out of range.");
		}
	}


	private void ClearContainer()
	{
		foreach (Node child in container.GetChildren())
		{
			child.QueueFree();
		}
	}

	public void UpdateTextEdit(TextEdit stringObj)
	{
		Font f = stringObj.GetThemeDefaultFont();
		Vector2 textSize = new Vector2();
		if (stringObj.Text.Length != 0)
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

			for (int i = 0; i < paramCount; i++)
			{

				Type paramType = parameterInfo[i].ParameterType;
				string paramName = i.ToString() + "_" + parameterInfo[i].Name;

				container.AddChild(ParameterFactory(paramName, paramType));
			}
		}
	}
}
