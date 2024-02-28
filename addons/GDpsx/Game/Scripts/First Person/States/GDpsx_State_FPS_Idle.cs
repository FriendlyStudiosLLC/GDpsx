using GDpsx_API.StateMachine;
using Godot;
using System;

public partial class GDpsx_State_FPS_Idle : GDpsx_State
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
        StateMachine.PlayerMovement.CalculatedVelocity.X = Mathf.MoveToward(StateMachine.PlayerMovement.Velocity.X, 0, StateMachine.PlayerMovement.CurrentSpeed);
        StateMachine.PlayerMovement.CalculatedVelocity.Z = Mathf.MoveToward(StateMachine.PlayerMovement.Velocity.Z, 0, StateMachine.PlayerMovement.CurrentSpeed);

        if(StateMachine.PlayerMovement.CrouchAction && StateMachine.PlayerMovement.CanCrouch)
        {
            
                StateMachine.TransitionTo("GDpsx State | Crouch");
            
            
        }
        

        if(StateMachine.PlayerMovement.JumpAction && !StateMachine.PlayerMovement.CrouchAction)
        {
            if(StateMachine.PlayerMovement.CeilingCheckCast.IsColliding()) return;
            StateMachine.TransitionTo("GDpsx State | Jump");
        }

        if(StateMachine.PlayerMovement.InputDirection != Vector2.Zero)
        {
            StateMachine.TransitionTo("GDpsx State | Walk");
        }
        if(!StateMachine.PlayerMovement.IsOnFloor() && Mathf.Abs(StateMachine.PlayerMovement.Velocity.Y) > 0.1f)
        {
            StateMachine.TransitionTo("GDpsx State | Airborn");
        }

    }
}