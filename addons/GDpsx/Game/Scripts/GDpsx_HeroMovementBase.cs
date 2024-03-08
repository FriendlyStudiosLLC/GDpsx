using Godot;
using System;
using GDpsx_API;
using GDpsx_API.StateMachine;

public partial class GDpsx_HeroMovementBase : CharacterBody3D, IInteractable, IDamage
{
    [ExportCategory("Health Settings")]
    [Export] public float MaxHealth = 100;
    [Export] public float CurrentHealth = 100;
    [Export] public float HealthRegenRate = 5f;


    [ExportCategory("Movement Variables")]
    [Export] public float WalkSpeed = 5f;
    [Export] public float SprintSpeed = 10f;
    [Export] public float CrouchSpeed = 2.5f;
    [Export] public float JumpVelocity = 3f;
    public float CurrentSpeed;
    [Export] public float MouseSensitivty = 0.02f;
    [Export] public bool CanCrouch = true;
    [Export] public bool CanJump = true;
    [Export] public bool CanSprint = true;
    public bool SprintAction = false;
    public bool CrouchAction = false;
    public bool JumpAction = false;
    public bool InteractAction = false;
    public bool PrimaryUseAction = false;
    public bool SecondaryUseAction = false;

    

    [ExportCategory("Interaction Settings")]
    [Export] public RayCast3D LookAtCast;
    public Node3D LookedAtObject = null;



    [ExportCategory("Misc Settings")]
    [Export] public float LerpSpeed { private set; get; } = 10.0f;
    [Export] public ShapeCast3D CeilingCheckCast;
    [Export] public GDpsx_StateMachine StateMachine;
    [Export] public AnimationPlayer AnimPlayer;
    [Export] public GDpsx_UI_Template UI;
    [Export] public GDpsx_Inventory Inventory;
    [Export] public GDpsx_MessageBox messageBox;
    [Export] public GDpsx_dialog_box dialogBox;
    

    //Calculation Variables
    public Vector2 InputDirection = Vector2.Zero;
    public Vector3 MovementDirection = Vector3.Zero;
    public Vector3 CalculatedVelocity = Vector3.Zero;
    public float AirTime;




    public virtual void CalculateMovementVector(float delta){}
    public virtual void AddGravity(float delta){}
    public virtual void HandleJump(float delta){}
    public virtual void HandleCrouch(float delta){}
    public virtual void HandleSprint(float delta){}
    public virtual void UpdateVelocity(float delta){}

    public void TakeDamage(double damage, Node3D instigator)
    {
        CurrentHealth = CurrentHealth - (float)damage;
        UI.UpdateHealthBar(CurrentHealth);
    }

    public void EnterInteract()
    {
        throw new NotImplementedException();
    }

    public void ExitInteract()
    {
        throw new NotImplementedException();
    }


    public Node3D PerformLookAt()
    {
            if(LookAtCast.IsColliding())
            {
                if(LookAtCast.GetCollider() is IInteractable)
                {
                    
                    UI.SetCrosshair(true);
                    LookedAtObject = LookAtCast.GetCollider() as Node3D;
                    if(Input.IsActionJustPressed("Interact"))
                    {
                        LookedAtObject.Call("EnterInteract");
                    }
                    if(LookedAtObject is GDpsx_GameObject)
                    {
                        var LookAtGameObject = LookedAtObject as GDpsx_GameObject;
                        UI.SetLabelText(LookAtGameObject.LookAtMessage);
                        
                        return (Node3D)LookAtCast.GetCollider();
                    }
                }
                else
                {
                    UI.SetCrosshair(false);
                    UI.SetLabelText("");
                }
                
            }
            else
                {
                    UI.SetCrosshair(false);
                    UI.SetLabelText("");
                }

            return null;
    }

    public string LookAtMessage()
    {
        throw new NotImplementedException();
    }

    public void SetMouseMode(Input.MouseModeEnum mode)
    {
        Input.MouseMode = mode;
    }
}