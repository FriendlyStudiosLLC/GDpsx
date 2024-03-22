using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts
{
	internal interface IDamage
	{
		void TakeDamage(double damage, Node3D instigator);
	}
}