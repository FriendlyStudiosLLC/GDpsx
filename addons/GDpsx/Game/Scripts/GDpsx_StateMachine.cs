using Godot;
using Godot.Collections;
using System;

namespace GDpsx_API.StateMachine
{
    public partial class GDpsx_StateMachine : Node
    {
        [Export] public NodePath initialState;
        public GDpsx_HeroMovementBase PlayerMovement;

        private Dictionary<string, GDpsx_State> _states;
        public GDpsx_State _currentState;
        public GDpsx_State _previousState;

        public override void _Ready()
        {
            PlayerMovement = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree()) as GDpsx_HeroMovementBase;
            _states = new Dictionary<string, GDpsx_State>();
            foreach (Node node in GetChildren())
            {
                if (node is GDpsx_State s)
                {
                    _states[node.Name] = s;
                    s.StateMachine = this;
                    s.ReadyState();
                    s.Exit();
                }
            }
            _currentState = GetNode<GDpsx_State>(initialState);
            _currentState.Enter();
        }
        public override void _Process(double delta)
        {
           _currentState.Update((float)delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            _currentState.PhysicsUpdate((float)delta);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            _currentState.HandleInput(@event);
        }

        public void TransitionTo(string stateName)
        {
            if(!_states.ContainsKey(stateName) || _currentState == _states[stateName])
            {
                return;
            } 

            _previousState = _currentState;
            _currentState.Exit();
            _currentState = _states[stateName];
            _currentState.Enter();
        }
    }
}