using Godot;
using System;

namespace GDpsx_API
{
    public class GDpsx_GameObjectID
    {
        private string _value;
        public GDpsx_GameObjectID(string value)
        {
            _value = value;
        }
        public override string ToString()
        {
            return _value;
        }
        // Add other methods as needed to mimic System.String functionality
    }
    
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
        void TakeDamage(double damage, Node3D instigator);
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
