using Godot;
using System;
using GDpsx_API;
using Godot.Collections;
using System.Threading.Tasks;

public enum DoorType
{
    Regular,
    Sliding
}

public enum DoorState
{
    Closed,
    Open,
    Locked
}


public partial class GDpsx_Door : GDpsx_GameObject, IInteractable
{   
    [Export] private DoorType doorType;
    [Export] private DoorState currentState;
    [Export] private Vector3 OpenPosition;
    
    private Vector3 openRotation;
    private Vector3 closedRotation;
    [Export] private float openRotationAmount = 90f;
    [Export] private float door_speed = 1f;
    [Export(PropertyHint.MultilineText)] public string lockedMessage = "The door is locked";
    
    [Export] public bool OpenOnUnlock = false;
    [Export] public bool UseEventChain = false;



    public void EnterInteract()
    {
        switch(currentState)
        {
            case DoorState.Closed:
                OpenDoor();
                break;
            case DoorState.Open:
                CloseDoor();
                break;
            case DoorState.Locked:
                var player = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree()) as GDpsx_HeroMovementBase;
                player.messageBox.StartTyping(lockedMessage);
                break;

        }

        if(UseEventChain) base.EnterInteract();
    }




    public void OpenDoor()
    {
        
        var openPosition = GlobalPosition + OpenPosition;
        //RandomSound(OpenSFX);
        switch (doorType)
        {
            case DoorType.Regular:
                var doorTween = CreateTween();
                doorTween.TweenProperty(this, "rotation", new Vector3(0, Mathf.DegToRad(openRotationAmount), 0), door_speed);
                GD.Print("Opening Door");
                doorTween.Play();
                break;
            case DoorType.Sliding:
                var SlideTween = CreateTween();
                SlideTween.TweenProperty(this, "position", openPosition, door_speed);
                GD.Print("Opening Door");
                SlideTween.Play();
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
        currentState = DoorState.Open;
        return;
    }

    
    public void CloseDoor()
    {
       
        var closedPosition = GlobalPosition + -OpenPosition;
        //var playClosed = () => { RandomSound(CloseSFX); };
        switch (doorType)
        {
            case DoorType.Regular:
                var doorTween = CreateTween();
                doorTween.TweenProperty(this, "rotation", Vector3.Zero, door_speed);
                GD.Print("Closing Door");
                doorTween.Play();
                //doorTween.Finished += playClosed;
                break;
            case DoorType.Sliding:
                var SlideTween = CreateTween();
                SlideTween.TweenProperty(this, "position", closedPosition, door_speed);
                GD.Print("Closing Door");
                SlideTween.Play();
                //SlideTween.Finished += playClosed;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        currentState = DoorState.Closed;
        return;
    }
}