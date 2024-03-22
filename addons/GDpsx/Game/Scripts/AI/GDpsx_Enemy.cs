using Godot;
using Godot.Collections;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts.AI
{
	public partial class GDpsx_Enemy : CharacterBody3D
	{
		[Export] public Area3D visionCone;
		[Export] public RayCast3D visionRaycast;
		[Export] public NavigationAgent3D NavAgent;
		[Export] public float MoveSpeed = 5f;
		[Export] public bool PlayerSeen = false;
		[Export] public Vector3 PlayerLocation = Vector3.Zero;

		public override void _PhysicsProcess(double delta)
		{
			Vector3 currentLocation = GlobalTransform.Origin;
			Vector3 next_location = NavAgent.GetNextPathPosition();
			Vector3 new_velocity = (next_location - currentLocation).Normalized() * MoveSpeed;
			if (PlayerSeen)
			{
				LookAt(PlayerLocation, Vector3.Up);
			}
			Velocity = new_velocity;
			MoveAndSlide();
		}

		public void UpdateTargetLocation(Vector3 playerLocation)
		{
			PlayerLocation = playerLocation;
			NavAgent.TargetPosition = playerLocation;

		}

		public void VisionTimeOut()
		{
			Array<Node3D> overlaps = visionCone.GetOverlappingBodies();
			if (overlaps.Count > 0)
			{
				foreach (Node3D overlap in overlaps)
				{
					Array<StringName> overlapGroups = overlap.GetGroups();
					if (overlapGroups.Contains("Player"))
					{
						Vector3 playerPosition = overlap.GlobalTransform.Origin;
						visionRaycast.LookAt(playerPosition, Vector3.Up);
						visionRaycast.ForceRaycastUpdate();

						if (visionRaycast.IsColliding())
						{
							Node3D collider = visionRaycast.GetCollider() as Node3D;
							Array<StringName> colliderGroups = collider.GetGroups();
							PlayerSeen = colliderGroups.Contains("Player");
							if (colliderGroups.Contains("Player"))
							{
								UpdateTargetLocation(playerPosition);
							}
							else
							{
							}

						}
					}
					else
					{
						PlayerSeen = false;
					}
				}
			}

		}
	}
}
