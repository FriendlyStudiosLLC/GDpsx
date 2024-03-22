using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GDpsx_API.EventSystem
{
	[Tool]
	public partial class GDpsx_ES_Option : GDpsx_ES_Node
	{
		[Export] public LineEdit replyText;
		[Export] public MenuButton functionMenu;
		[Export] public VBoxContainer container;
		[Export] public Button AddRequirementButton;
		[Export] public Button RemoveRequirementButton;
		public bool hasRequirement;
		public string functionName;
		public GDpsx_ES_R_Option resource;


		public void ShowRequirements()
		{
			functionMenu.Visible = true;
			container.Visible = true;
			AddRequirementButton.Visible = false;
			hasRequirement = true;
			RemoveRequirementButton.Visible = true;
		}

		public void HideRequirements()
		{
			hasRequirement = false;
			functionMenu.Visible = false;
			container.Visible = false;
			AddRequirementButton.Visible = true;
			RemoveRequirementButton.Visible = false;
		}

		public override void _Ready()
		{

			functionMenu.GetPopup().IndexPressed += FunctionMenuSelected;
			Type type = typeof(GDpsx_ES_ConditionalNodeLibrary);
			MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			foreach (MethodInfo method in methodInfos)
			{
				functionMenu.GetPopup().AddItem(method.Name);
			}
		}

		public void Init()
		{
			functionMenu.GetPopup().IndexPressed += FunctionMenuSelected;
			Type type = typeof(GDpsx_ES_ConditionalNodeLibrary);
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
			Type type = typeof(GDpsx_ES_ConditionalNodeLibrary);
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
			Type type = typeof(GDpsx_ES_ConditionalNodeLibrary);
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

		public void ConstructOptionData()
		{
			List<Variant> parameterList = new List<Variant>();
			VBoxContainer ParamContainer = container;
			int index = 0;
			foreach (Node child in ParamContainer.GetChildren())
			{
				index += 1;
				foreach (Node paramChild in child.GetChildren())
				{
					if (paramChild.GetType() != typeof(Label))
					{
						Variant value = new Variant();
						switch (paramChild.Name)
						{
							case "System_Int32":
								SpinBox _int = paramChild as SpinBox;
								value = (int)_int.Value;
								parameterList.Add(value);
								break;
							case "System_String":
								TextEdit _string = paramChild as TextEdit;
								value = _string.Text;
								parameterList.Add(_string.Text);
								break;
							case "System_Boolean":
								CheckBox _bool = paramChild as CheckBox;
								value = _bool.ButtonPressed;
								parameterList.Add(value);
								break;
							case "System_Double":
								SpinBox _Double = paramChild as SpinBox;
								value = _Double.Value;
								parameterList.Add(value);
								break;
						}
					}
				}
			}
			if (hasRequirement)
			{
				Array<Variant> godotParameterArray = new Array<Variant>();
				foreach (Variant item in parameterList)
				{
					godotParameterArray.Add(item);
				}
				GDpsx_ES_R_Option resourceData = new GDpsx_ES_R_Option(functionName, godotParameterArray);
				resourceData.hasRequirement = hasRequirement;
				resourceData.OptionText = replyText.Text;
				resource = resourceData;
			}
			else
			{
				GDpsx_ES_R_Option resourceData = new GDpsx_ES_R_Option();
				resourceData.hasRequirement = hasRequirement;
				resourceData.OptionText = replyText.Text;
				resource = resourceData;
			}
		}

		public void ConstructDataFromResource(GDpsx_ES_R_Option node)
		{

			VBoxContainer ParamContainer = container;

			replyText.Text = node.OptionText;
			if (node.hasRequirement)
			{
				int index = MenuButtonIndexByString(node.methodName);
				FunctionMenuSelected(index);

				foreach (Variant parameter in node.parameterList)
				{
					Node hbox = ParamContainer.GetChild(index, true).GetChild(1, false);
					switch (hbox.Name)
					{
						case "System_Int32":
							SpinBox _int = hbox as SpinBox;
							_int.Value = (int)parameter;
							break;
						case "System_String":
							TextEdit _string = hbox as TextEdit;
							_string.Text = parameter.ToString();
							break;
						case "System_Boolean":
							CheckBox _bool = hbox as CheckBox;
							_bool.ButtonPressed = parameter.AsBool();
							break;
						case "System_Double":
							SpinBox _Double = hbox as SpinBox;
							_Double.Value = (double)parameter;
							break;

					}
					// if(.GetType() != typeof(Label))
					// {
					//     GD.Print(paramChild.Name);
					//     switch(paramChild.Name)
					//     {   
					//         case "System_Int32":
					//             var _int = paramChild as SpinBox;
					//             _int.Value = (int)value;
					//             break;
					//         case "System_String":
					//             var _string = paramChild as TextEdit;
					//             _string.Text = value.ToString();
					//             break;
					//         case "System_Boolean":
					//             var _bool = paramChild as CheckBox;
					//             _bool.ButtonPressed = value.AsBool();
					//             break;
					//         case "System_Double":
					//             var _Double = paramChild as SpinBox;
					//             _Double.Value = (double)value;
					//             break;

					//     }
					// }

					index += 1;
				}
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
	}
}
