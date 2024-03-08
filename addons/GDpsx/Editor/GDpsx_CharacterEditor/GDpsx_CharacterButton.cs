using Godot;
using System;

[Tool]
public partial class GDpsx_CharacterButton : Button
{
    public GDpsx_Character character;
    [Export] public GDpsx_CharacterEditor characterEditor;
}
