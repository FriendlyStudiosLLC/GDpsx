using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts.EventSystem
{
	[Tool]
	[GlobalClass]
	public partial class Event_DisplayMessage : Core.GDpsx_InteractionEventBase
	{

		[Export(PropertyHint.MultilineText)] public string Message = "The door is locked";
		[Export] public int TypeSpeedMS = 25;
		[Export] public int MessageHoldTimeMS = 2000;
		public override void Enter(SceneTree tree = null, First_Person.GDpsx_GameObject instigator = null)
		{
			First_Person.FPS_HeroMovement player = GDpsx_Utility.GetPlayer(tree) as First_Person.FPS_HeroMovement;
			player.messageBox.StartTyping(Message, TypeSpeedMS, MessageHoldTimeMS);
		}
	}
}
