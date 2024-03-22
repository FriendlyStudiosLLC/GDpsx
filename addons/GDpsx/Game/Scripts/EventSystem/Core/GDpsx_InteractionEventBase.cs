using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts.EventSystem.Core
{
	[Tool]
	[GlobalClass]
	public partial class GDpsx_InteractionEventBase : Resource
	{
		//Optional, doesn't need to be populated

		[Export] public NodePath Target_Entity_Path { get; set; }


		[Export] public float WaitTime { get; set; }


		public virtual void Enter(SceneTree tree = null, First_Person.GDpsx_GameObject instigator = null)
		{
		} //Scene tree is an optional argument.
	}
}
