using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts.EventSystem
{
	[Tool]
	[GlobalClass]
	public partial class Event_LightSwitch : Core.GDpsx_InteractionEventBase
	{
		private bool State = true;
		public override void Enter(SceneTree tree = null, First_Person.GDpsx_GameObject instigator = null)
		{
			State = !State;

			Node TargetEntity = instigator.GetNodeFromPath(Target_Entity_Path);

			OmniLight3D light = TargetEntity as OmniLight3D;
			if (light != null) light.Visible = State;
		}
	}
}
