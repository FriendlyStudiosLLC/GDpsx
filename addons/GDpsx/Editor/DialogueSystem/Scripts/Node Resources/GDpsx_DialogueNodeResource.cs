using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;

[GlobalClass][Tool]
public partial class GDpsx_DialogueNodeResource : GDpsx_NodeResource
{
    [Export] public string speakerName;
    [Export] public string message;
    [Export] public Array<ResponseItem> responses = new Array<ResponseItem>();
}
