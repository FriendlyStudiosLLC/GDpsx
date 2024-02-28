using Godot;
using System;
using System.IO;

public enum NodeType
{
    Start,
    Conditional,
    Dialogue,

}

public partial class GDpsx_NodeResource : Resource
{
    [Export] public string nodeID;
    [Export] public int waitTimeMS; 
    [Export] public NodeType nodeType;
    [Export] public int fromSlotIndex;
    [Export] public StringName fromSlot;
    [Export] public int toSlotIndex;
    [Export] public StringName toSlot;
    [Export] public Vector2 graphPosition;
}
