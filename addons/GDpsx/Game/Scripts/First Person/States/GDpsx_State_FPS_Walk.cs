using GDpsx_API.StateMachine;
using Godot;
using System;

public partial class GDpsx_State_FPS_Walk : GDpsx_State
{
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate(float delta)
    {
        StateMachine.PlayerMovement.CurrentSpeed = Mathf.Lerp(StateMachine.PlayerMovement.CurrentSpeed, StateMachine.PlayerMovement.WalkSpeed, 
            1.0f - Mathf.Pow(0.5f, (float)delta *  StateMachine.PlayerMovement.LerpSpeed));

        if(StateMachine.PlayerMovement.CrouchAction)
        {
            
                StateMachine.TransitionTo("GDpsx State | Crouch");
            
            
        }

        if(StateMachine.PlayerMovement.JumpAction && !StateMachine.PlayerMovement.CrouchAction)
        {
            if(StateMachine.PlayerMovement.CeilingCheckCast.IsColliding()) return;
            StateMachine.TransitionTo("GDpsx State | Jump");
        }

        if(StateMachine.PlayerMovement.SprintAction)
        {
            StateMachine.TransitionTo("GDpsx State | Sprint");
        }
        if(StateMachine.PlayerMovement.InputDirection == Vector2.Zero)
        {
            StateMachine.TransitionTo("GDpsx State | Idle");
        }
        if(!StateMachine.PlayerMovement.IsOnFloor() && Mathf.Abs(StateMachine.PlayerMovement.Velocity.Y) > 0.1f)
        {
            StateMachine.TransitionTo("GDpsx State | Airborn");
        }

    }
}