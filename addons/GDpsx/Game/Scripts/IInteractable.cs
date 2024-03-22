using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts
{
	internal interface IInteractable
	{
		void EnterInteract();
		void ExitInteract();
		Node3D PerformLookAt();
		string LookAtMessage();
	}
}