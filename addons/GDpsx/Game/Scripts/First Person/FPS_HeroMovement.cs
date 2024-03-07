using Godot;
using System;
using GDpsx_API;

public partial class FPS_HeroMovement : GDpsx_HeroMovementBase, IDamage, IInteractable
{
    [ExportCategory("Camera Settings")]
    [Export] public Node3D Head;
    [Export] public Camera3D Eyes;
    private float HeadRotationX;
    private float HeadRotationZ;
    [Export] public Vector2 MaxMinLook = new Vector2(-85, 85);
    [Export] public float CameraRotationLerp = 7f;


    public override void _Ready()
    {
        
        CurrentSpeed = WalkSpeed;
        SetMouseMode(Input.MouseModeEnum.Captured);
    }

    


    public override void _Process(double delta)
    {
        HandleHeadRotation((float)delta);
        CalculateMovementVector((float)delta);
        PerformLookAt();
        HandleCrouch((float)delta);
        HandleSprint((float)delta);
        AddGravity((float)delta);
        HandleJump((float)delta);
        UpdateVelocity((float)delta);
        MoveAndSlide();
    }

    public override void CalculateMovementVector(float delta)
    {
        InputDirection = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");
        MovementDirection = MovementDirection.Lerp((Transform.Basis * new Vector3(InputDirection.X, 0f, InputDirection.Y)).Normalized(), 1.0f - Mathf.Pow(0.5f, (float)delta * LerpSpeed));
        
        if(MovementDirection != Vector3.Zero)
        {
            CalculatedVelocity.X = MovementDirection.X * CurrentSpeed;
            CalculatedVelocity.Z = MovementDirection.Z * CurrentSpeed;
        }
    }
    public override void AddGravity(float delta)
    {
       if (!IsOnFloor())
        {
            CalculatedVelocity.Y -= ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle() * (float)delta;
        }
        
    }


    public override void HandleJump(float delta)
    {
        if(!CanJump) return;
		JumpAction = Input.IsActionPressed("Jump");
    }


    public override void HandleCrouch(float delta)
    {
        CrouchAction = Input.IsActionPressed("Crouch");
    }


    public override void HandleSprint(float delta)
    {
        SprintAction = Input.IsActionPressed("Sprint");
    }


    public override void UpdateVelocity(float delta)
    {
        Velocity = CalculatedVelocity;
    }
    
    public void HandlePlayerRotation(float mouseX, float mouseY)
    {
        RotateY(Mathf.DegToRad(-mouseX * MouseSensitivty));

        HeadRotationX += Mathf.DegToRad(-mouseY * MouseSensitivty);
        HeadRotationX = Mathf.Clamp(HeadRotationX, Mathf.DegToRad(-89f), Mathf.DegToRad(89f));

        HeadRotationZ += Mathf.DegToRad(mouseX * MouseSensitivty * InputDirection.Length());
        HeadRotationZ = Mathf.Clamp(HeadRotationZ, Mathf.DegToRad(MaxMinLook.X), Mathf.DegToRad(MaxMinLook.Y));
    }

    public void HandleHeadRotation(float delta)
    {
        HeadRotationZ = Mathf.Lerp(HeadRotationZ, 0f, delta * CameraRotationLerp);

        Transform3D transform = Eyes.Transform;
        transform.Basis = Basis.Identity;
        Eyes.Transform = transform;

        Eyes.RotateObjectLocal(Vector3.Right, HeadRotationX);
        //Eyes.RotateObjectLocal(Vector3.Forward, HeadRotationZ);
    }

    

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event is InputEventMouseMotion eventMouseMotion)
        {
            HandlePlayerRotation(eventMouseMotion.Relative.X, eventMouseMotion.Relative.Y);
        }

        if(Input.IsActionJustPressed("Pause"))
        {
            if(Inventory.InventoryUI.Visible)
            { 
                Inventory.InventoryUI.ToggleInventory();
                return;
            }
            if(GDpsx_Utility.GetEventSystem(GetTree()) != null && GDpsx_Utility.GetEventSystem(GetTree()).dialog_Box.Visible)
            {
                GDpsx_Utility.GetEventSystem(GetTree()).dialog_Box.HideDialogBox();
                return;
            }
            GetTree().Quit();
        }

        if(Input.IsActionJustPressed("Inventory"))
        {
            Inventory.InventoryUI.ToggleInventory();
        }
    }


    

}
