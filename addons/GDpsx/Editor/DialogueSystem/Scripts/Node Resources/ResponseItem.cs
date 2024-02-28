using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;

[Tool][GlobalClass]
public partial class ResponseItem : Resource
{
    
    [Export]public string responseText;
    [Export]public StringName toNode;
}