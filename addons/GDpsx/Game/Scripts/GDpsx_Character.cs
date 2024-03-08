using GDpsx_API;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public enum CharacterType
{
    None,
    NPC,
    Ped,
    Enemy,
    Prop,
}

[GlobalClass][Tool]
public partial class GDpsx_Character : Resource
{
    [Export] public string characterName;
    [Export] public int MaxHealth;
    [Export] public int DamagePerAttack;
    [Export] public int AttackRate;
    [Export] public PackedScene CharacterScene;
}