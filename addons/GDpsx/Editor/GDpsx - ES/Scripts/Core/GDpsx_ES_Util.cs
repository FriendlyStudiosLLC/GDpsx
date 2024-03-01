using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
namespace GDpsx_API.EventSystem
{
    public enum NodeType
    {
        None,
        Dialog,
        Conditional,
        Function
    }

    public class ESFunctionDetails
    {
        Variant methodName;
        Variant parameter1Type;
        Variant parameter1;
        Variant parameter2Type;
        Variant parameter2;

        public ESFunctionDetails(Variant MethodName, Variant Parameter1, Variant Parameter1Type,Variant Parameter2, Variant Parameter2Type)
        {
            methodName = MethodName;
            parameter1Type = Parameter1Type;
            parameter1 = Parameter1;
            parameter2Type = Parameter2Type;
            parameter2 = Parameter2;
        }

    }

    public class ConnectionDetails
    {
        public StringName From { get; set; }
        public StringName To { get; set; }
        public int FromPort { get; set; }
        public int ToSlot { get; set; }
    }
    public partial class GDpsx_ES_Util : Node
    {
        public void Save(string path, Dictionary data)
        {
            var file = FileAccess.Open(path,FileAccess.ModeFlags.Write);
            file.StoreLine(Json.Stringify(data));
            file.Close();
        }
    }
}
