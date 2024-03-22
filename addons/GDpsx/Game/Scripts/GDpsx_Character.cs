using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts
{

	[GlobalClass]
	[Tool]
	public partial class GDpsx_Character : Resource
	{
		[Export] public string characterName;
		[Export] public int MaxHealth;
		[Export] public int DamagePerAttack;
		[Export] public int AttackRate;
		[Export] public PackedScene CharacterScene;
	}
}
