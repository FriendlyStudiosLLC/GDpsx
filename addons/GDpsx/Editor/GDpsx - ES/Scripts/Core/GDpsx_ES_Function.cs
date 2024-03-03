using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GDpsx_API.EventSystem
{
    public partial class GDpsx_ES_Function : GDpsx_ES_Node
    {
        [Export] GDpsx_Function_Node_Box functionBox;
        [Export] public Dictionary funcData = new Dictionary();
        public GDpsx_ES_R_Function resource;
        [Export] Array<Variant> parameterArray;
        public override void _Ready()
        {
            nodeType = NodeType.Function;
        }

        public void ConstructDataFromResource(GDpsx_ES_R_Function node)
        {
            
            var ParamContainer = functionBox.container;
            functionBox.FunctionMenuSelected(functionBox.MenuButtonIndexByString(node.methodName));
            foreach(var parameter in node.parameterList)
            {
                var hbox = ParamContainer.GetChild(index, true).GetChild(1, false);
                    switch(hbox.Name)
                    {   
                        case "System_Int32":
                            var _int = hbox as SpinBox;
                            _int.Value = (int)parameter;
                            break;
                        case "System_String":
                            var _string = hbox as TextEdit;
                            _string.Text = parameter.ToString();
                            break;
                        case "System_Boolean":
                            var _bool = hbox as CheckBox;
                            _bool.ButtonPressed = parameter.AsBool();
                            break;
                        case "System_Double":
                            var _Double = hbox as SpinBox;
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
            var parameterDictionary = funcData["Parameters"].AsGodotDictionary();
            
            var ParamContainer = functionBox.container;
            var index = 0;
            foreach(var parameter in parameterDictionary)
            {   
                
               
                var key = parameter.Key;
                var value = parameter.Value;
                var key_parts = key.ToString().Split('_');
                string typeName = $"{key_parts[key_parts.Length - 2]}.{key_parts[key_parts.Length - 1]}";
                Type type = Type.GetType(typeName);
                
                
                
                    var hbox = ParamContainer.GetChild(index, true).GetChild(1, false);
                    switch(hbox.Name)
                    {   
                        case "System_Int32":
                            var _int = hbox as SpinBox;
                            _int.Value = (int)value;
                            break;
                        case "System_String":
                            var _string = hbox as TextEdit;
                            _string.Text = value.ToString();
                            break;
                        case "System_Boolean":
                            var _bool = hbox as CheckBox;
                            _bool.ButtonPressed = value.AsBool();
                            break;
                        case "System_Double":
                            var _Double = hbox as SpinBox;
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
            var parameterList = new List<Variant>(); 
            var ParamContainer = functionBox.container;
            var index = 0;
            foreach (var child in ParamContainer.GetChildren())
            {
                index += 1;
                foreach (var paramChild in child.GetChildren())
                {
                    if (paramChild.GetType() != typeof(Label))
                    {
                        Variant value = new Variant();
                        switch (paramChild.Name)
                        {
                            case "System_Int32":
                                var _int = paramChild as SpinBox;
                                value = (int)_int.Value;
                                parameterList.Add(value);
                                break;
                            case "System_String":
                                var _string = paramChild as TextEdit;
                                value = _string.Text;
                                parameterList.Add(_string.Text); 
                                break;
                            case "System_Boolean":
                                var _bool = paramChild as CheckBox;
                                value = _bool.ButtonPressed;
                                parameterList.Add(value); 
                                break;
                            case "System_Double":
                                var _Double = paramChild as SpinBox;
                                value = _Double.Value;
                                parameterList.Add(value); 
                                break;
                        }
                    }
                }
            }

            Godot.Collections.Array<Variant> godotParameterArray = new Godot.Collections.Array<Variant>();
            foreach (var item in parameterList)
            {
                godotParameterArray.Add(item);
            }
            var resourceData = new GDpsx_ES_R_Function(functionBox.functionName, godotParameterArray);
            resource = resourceData;
        }

        public void ConstructFunctionDictionary()
        {
            funcData = new Dictionary();
            var funcData_Template = new Dictionary();
            funcData[Name] = funcData_Template;
            funcData_Template["Function"] = functionBox.functionName;
            funcData_Template["Parameters"] = new Dictionary();
            var ParamContainer = functionBox.container;
            var index = 0;
            foreach(var child in ParamContainer.GetChildren())
            {
                index += 1;
                foreach(var paramChild in child.GetChildren())
                {
                    if(paramChild.GetType() != typeof(Label))
                    {   
                        switch(paramChild.Name)
                        {   
                            case "System_Int32":
                                var _int = paramChild as SpinBox;
                                ((Dictionary)funcData_Template["Parameters"]).Add(index.ToString() + "_" + paramChild.Name, _int.Value.ToString());
                                break;
                            case "System_String":
                                var _string = paramChild as TextEdit;
                                ((Dictionary)funcData_Template["Parameters"]).Add(index.ToString() + "_" + paramChild.Name, _string.Text.ToString());
                                break;
                            case "System_Boolean":
                                var _bool = paramChild as CheckBox;
                                ((Dictionary)funcData_Template["Parameters"]).Add(index.ToString() + "_" + paramChild.Name, _bool.ButtonPressed.ToString());
                                break;
                            case "System_Double":
                                var _Double = paramChild as SpinBox;
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