using Godot;
using System;
using System.IO;



public partial class GDpsx_NodeResource : Resource
{
    [Export] public string nodeID;
    [Export] public int waitTimeMS; 
    [Export] public int fromSlotIndex;
    [Export] public StringName fromSlot;
    [Export] public int toSlotIndex;
    [Export] public StringName toSlot;
    [Export] public Vector2 graphPosition;
}
