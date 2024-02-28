using GDpsx_API.StateMachine;
using Godot;
using System;

public partial class GDpsx_State_FPS_Crouch : GDpsx_State
{
    public override void Enter()
    {
        StateMachine.PlayerMovement.AnimPlayer.Play("Crouching", -1.0, (StateMachine.PlayerMovement.LerpSpeed/12));
    }
    public override void Exit()
    {
        
        StateMachine.PlayerMovement.AnimPlayer.Play("Crouching", -1.0, (-StateMachine.PlayerMovement.LerpSpeed/12) * 1.5f, true);
        base.Exit();
    }

    public override void PhysicsUpdate(float delta)
    {
        StateMachine.PlayerMovement.CurrentSpeed = Mathf.Lerp(StateMachine.PlayerMovement.CurrentSpeed, StateMachine.PlayerMovement.CrouchSpeed, 
            1.0f - Mathf.Pow(0.5f, (float)delta *  StateMachine.PlayerMovement.LerpSpeed));

        if(!StateMachine.PlayerMovement.CrouchAction && StateMachine.PlayerMovement.CeilingCheckCast.IsColliding() == false)
        {
            StateMachine.TransitionTo("GDpsx State | Idle");
        }
        if(!StateMachine.PlayerMovement.IsOnFloor() && Mathf.Abs(StateMachine.PlayerMovement.Velocity.Y) > 0.1f)
        {
            StateMachine.TransitionTo("GDpsx State | Airborn");
        }
    }
}