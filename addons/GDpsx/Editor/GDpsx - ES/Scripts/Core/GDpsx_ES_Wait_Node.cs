using GDpsx_API.EventSystem;
using Godot;
using System;

[Tool]
public partial class GDpsx_ES_Wait_Node : GDpsx_ES_Node
{
    public int WaitTime_MS = 5000;
    [Export] public SpinBox spinBox;
}
