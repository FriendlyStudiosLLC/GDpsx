using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
namespace GDpsx_API.EventSystem
{
    public enum NodeType
    {
        Dialog,
        Conditional,
        Function
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
