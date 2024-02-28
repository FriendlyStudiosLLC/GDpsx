using Godot;
using System;

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
        var currentLocation = GlobalTransform.Origin;
        var next_location = NavAgent.GetNextPathPosition();
        var new_velocity = (next_location - currentLocation).Normalized() * MoveSpeed;
        if(PlayerSeen)
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
        var overlaps = visionCone.GetOverlappingBodies();
        if(overlaps.Count > 0)
        {
            foreach(var overlap in overlaps)
            {
                var overlapGroups = overlap.GetGroups();
                if(overlapGroups.Contains("Player"))
                {
                    var playerPosition = overlap.GlobalTransform.Origin;
                    visionRaycast.LookAt(playerPosition, Vector3.Up);
                    visionRaycast.ForceRaycastUpdate();

                    if(visionRaycast.IsColliding())
                    {
                        var collider = visionRaycast.GetCollider() as Node3D;
                        var colliderGroups = collider.GetGroups();
                        PlayerSeen = colliderGroups.Contains("Player");
                        if(colliderGroups.Contains("Player"))
                        {
                            GD.Print("Player Seen");
                            UpdateTargetLocation(playerPosition);
                        }
                        else
                        {
                            GD.Print("Looking for Player");
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
