using Godot;
using System;

[Tool]
public partial class GDpsx_CharacterButton : Button
{
	public GDpsx_Project.addons.GDpsx.Game.Scripts.GDpsx_Character character;
	[Export] public GDpsx_CharacterEditor characterEditor;
}
