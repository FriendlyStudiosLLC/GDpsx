using Godot;
using Godot.Collections;
using System;
using System.IO;

[GlobalClass][Tool]
public partial class GDpsx_DialogueResource : Resource
{
    [Export]public Array<GDpsx_DialogueNodeResource> dialogues = new Array<GDpsx_DialogueNodeResource>();
}