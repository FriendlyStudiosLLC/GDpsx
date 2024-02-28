using GDpsx_API;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[GlobalClass][Tool]
public partial class GDpsx_Item : Resource
{
    [Export] public string itemName;
    [Export(PropertyHint.MultilineText)] public string itemDescription;
    [Export] public int maxStackSize;
    [Export] public Texture2D itemIcon;
    [Export] public PackedScene pickupScene;
}