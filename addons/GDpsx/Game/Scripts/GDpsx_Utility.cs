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


        public static GDpsx_HeroMovementBase GetPlayer(SceneTree tree)
        {
            var playerGroup = tree.GetNodesInGroup("Player");
            var player = playerGroup[0] as GDpsx_HeroMovementBase;
            return player;
        }

        public static GDpsx_GlobalEventSystem GetEventSystem(SceneTree tree)
        {
            var ES_Group = tree.GetNodesInGroup("EventSystem");
            if(ES_Group.Count != 0)
            {   
                var ES = ES_Group[0] as GDpsx_GlobalEventSystem;
                return ES;
            }
            else
            {
                return null;
            }
            
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
