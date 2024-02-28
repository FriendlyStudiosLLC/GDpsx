using Godot;
using Godot.Collections;
using System;

namespace GDpsx_API.StateMachine
{
    public partial class GDpsx_State : Node
    {
        public GDpsx_StateMachine StateMachine;
        public virtual void Enter(){}
        public virtual void Exit(){}
        public virtual void ReadyState(){}
        public virtual void Update(float delta){}
        public virtual void PhysicsUpdate(float delta){}
        public virtual void HandleInput(InputEvent @event){}
    }
}