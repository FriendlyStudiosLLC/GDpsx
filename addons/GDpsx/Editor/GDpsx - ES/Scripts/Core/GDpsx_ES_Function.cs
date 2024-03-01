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
                        GD.Print(index);
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
            GD.Print(funcData.ToString());
        }
    }
}