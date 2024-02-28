using Godot;
using System;

namespace GDpsx_API
{

    
    public partial class GDpsx_Utility : Node
    {


        public static Node3D GetPlayer(SceneTree tree)
        {
            var playerGroup = tree.GetNodesInGroup("Player");
            var player = playerGroup[0] as Node3D;
            return player;
        }

    }

    
    internal interface IDamage
    {
        void TakeDamage(float damage, Node3D instigator);
    }
    internal interface IInteractable
    {
        void EnterInteract();
        void ExitInteract();
        Node3D PerformLookAt();
        string LookAtMessage();
    }

    internal interface IWeapon
    {
        void PrimaryUse();
        void SecondaryUse();

        void Reload();

        void Attack();
    }
}
