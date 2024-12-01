using System;
using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts.First_Person
{


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
			switch (currentState)
			{
				case DoorState.Closed:
					OpenDoor();
					break;
				case DoorState.Open:
					CloseDoor();
					break;
				case DoorState.Locked:
					GDpsx_HeroMovementBase player = GDpsx_Utility.GetPlayer(GetTree()) as GDpsx_HeroMovementBase;
					player.messageBox.StartTyping(lockedMessage);
					break;

			}

			if (UseEventChain) base.EnterInteract();
		}


		public void OpenDoor()
		{

			Vector3 openPosition = GlobalPosition + OpenPosition;
			//RandomSound(OpenSFX);
			switch (doorType)
			{
				case DoorType.Regular:
					Tween doorTween = CreateTween();
					doorTween.TweenProperty(this, "rotation", new Vector3(0, Mathf.DegToRad(openRotationAmount), 0), door_speed);

					doorTween.Play();
					break;
				case DoorType.Sliding:
					Tween SlideTween = CreateTween();
					SlideTween.TweenProperty(this, "position", openPosition, door_speed);

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

			Vector3 closedPosition = GlobalPosition + -OpenPosition;
			//var playClosed = () => { RandomSound(CloseSFX); };
			switch (doorType)
			{
				case DoorType.Regular:
					Tween doorTween = CreateTween();
					doorTween.TweenProperty(this, "rotation", Vector3.Zero, door_speed);

					doorTween.Play();
					//doorTween.Finished += playClosed;
					break;
				case DoorType.Sliding:
					Tween SlideTween = CreateTween();
					SlideTween.TweenProperty(this, "position", closedPosition, door_speed);

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
}
