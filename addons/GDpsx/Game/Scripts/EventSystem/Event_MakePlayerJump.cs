using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts.EventSystem
{
	[Tool]
	[GlobalClass]
	public partial class Event_MakePlayerJump : Core.GDpsx_InteractionEventBase
	{
		public override void Enter(SceneTree tree = null, First_Person.GDpsx_GameObject instigator = null)
		{
			First_Person.FPS_HeroMovement player = GDpsx_Utility.GetPlayer(tree) as First_Person.FPS_HeroMovement;
			player.JumpAction = true;
		}
	}
}
