using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GDpsx_API.EventSystem
{
	[Tool]
	public partial class GDpsx_ES_Function : GDpsx_ES_Node
	{
		[Export] private GDpsx_Function_Node_Box functionBox;
		[Export] public Dictionary funcData = new Dictionary();
		public GDpsx_ES_R_Function resource;
		[Export] private Array<Variant> parameterArray;
		public override void _Ready()
		{
		}

		public async void ConstructDataFromResource(GDpsx_ES_R_Function node)
		{

			VBoxContainer ParamContainer = functionBox.container;

			int _index = functionBox.MenuButtonIndexByString(node.methodName);

			functionBox.FunctionMenuSelected(_index);
			await Task.Delay(250);
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

		public void ConstructDataFromDictionary(string name)
		{
			Name = name;
			functionBox.FunctionMenuSelected(functionBox.MenuButtonIndexByString(funcData["Function"].AsString()));
			Dictionary parameterDictionary = funcData["Parameters"].AsGodotDictionary();

			VBoxContainer ParamContainer = functionBox.container;
			int index = 0;
			foreach (KeyValuePair<Variant, Variant> parameter in parameterDictionary)
			{


				Variant key = parameter.Key;
				Variant value = parameter.Value;
				string[] key_parts = key.ToString().Split('_');
				string typeName = $"{key_parts[key_parts.Length - 2]}.{key_parts[key_parts.Length - 1]}";
				Type type = Type.GetType(typeName);


				Node hbox = ParamContainer.GetChild(index, true).GetChild(1, false);
				switch (hbox.Name)
				{
					case "System_Int32":
						SpinBox _int = hbox as SpinBox;
						_int.Value = (int)value;
						break;
					case "System_String":
						TextEdit _string = hbox as TextEdit;
						_string.Text = value.ToString();
						break;
					case "System_Boolean":
						CheckBox _bool = hbox as CheckBox;
						_bool.ButtonPressed = value.AsBool();
						break;
					case "System_Double":
						SpinBox _Double = hbox as SpinBox;
						_Double.Value = (double)value;
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

		public void ConstructFunctionData()
		{
			List<Variant> parameterList = new List<Variant>();
			VBoxContainer ParamContainer = functionBox.container;
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

			Array<Variant> godotParameterArray = new Array<Variant>();
			foreach (Variant item in parameterList)
			{
				godotParameterArray.Add(item);
			}
			GDpsx_ES_R_Function resourceData = new GDpsx_ES_R_Function(functionBox.functionName, godotParameterArray);
			resource = resourceData;
		}

		public void ConstructFunctionDictionary()
		{
			funcData = new Dictionary();
			Dictionary funcData_Template = new Dictionary();
			funcData[Name] = funcData_Template;
			funcData_Template["Function"] = functionBox.functionName;
			funcData_Template["Parameters"] = new Dictionary();
			VBoxContainer ParamContainer = functionBox.container;
			int index = 0;
			foreach (Node child in ParamContainer.GetChildren())
			{
				index += 1;
				foreach (Node paramChild in child.GetChildren())
				{
					if (paramChild.GetType() != typeof(Label))
					{
						switch (paramChild.Name)
						{
							case "System_Int32":
								SpinBox _int = paramChild as SpinBox;
								((Dictionary)funcData_Template["Parameters"]).Add(index.ToString() + "_" + paramChild.Name, _int.Value.ToString());
								break;
							case "System_String":
								TextEdit _string = paramChild as TextEdit;
								((Dictionary)funcData_Template["Parameters"]).Add(index.ToString() + "_" + paramChild.Name, _string.Text.ToString());
								break;
							case "System_Boolean":
								CheckBox _bool = paramChild as CheckBox;
								((Dictionary)funcData_Template["Parameters"]).Add(index.ToString() + "_" + paramChild.Name, _bool.ButtonPressed.ToString());
								break;
							case "System_Double":
								SpinBox _Double = paramChild as SpinBox;
								((Dictionary)funcData_Template["Parameters"]).Add(index.ToString() + "_" + paramChild.Name, _Double.Value.ToString());
								break;

						}
					}
				}
			}
			funcData = funcData_Template;
		}
	}
}
