using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts
{
	public partial class GDpsx_State : Node
	{
		public GDpsx_StateMachine StateMachine;
		public virtual void Enter()
		{
		}
		public virtual void Exit()
		{
		}
		public virtual void ReadyState()
		{
		}
		public virtual void Update(float delta)
		{
		}
		public virtual void PhysicsUpdate(float delta)
		{
		}
		public virtual void HandleInput(InputEvent @event)
		{
		}
	}
}
