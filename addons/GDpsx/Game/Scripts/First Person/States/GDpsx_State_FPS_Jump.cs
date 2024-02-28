using GDpsx_API.StateMachine;
using Godot;
using System;

public partial class GDpsx_State_FPS_Jump : GDpsx_State
{
    public override void Enter()
    {
        StateMachine.PlayerMovement.CalculatedVelocity.Y = StateMachine.PlayerMovement.JumpVelocity;
        StateMachine.PlayerMovement.AnimPlayer.Play("JumpStart", -1.0, 2);
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate(float delta)
    {
        if(StateMachine.PlayerMovement.IsOnFloor())
        {
            StateMachine.PlayerMovement.AnimPlayer.Play("JumpStart", -1.0, -2 * 1.5f, true);
            StateMachine.TransitionTo("GDpsx State | Idle");
        }
    }
}