using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.States
{
    public class PlayerStates
    {
        // Possible states for player's movement
        public enum STATE
        {
            Idle, Stunt, Dash
        };

        // 'Events' - where we are in the running of a STATE.
        public enum EVENT
        {
            ENTER, UPDATE, EXIT
        };

        public STATE Name; 
        protected EVENT _stage; 
        protected PlayerStates _nextState; 
        protected PlayerMovement _playerMove;

        // Constructor for State
        public PlayerStates(PlayerMovement player)
        {
            _playerMove = player;
            _stage = EVENT.ENTER;
        }

        // Phases as you go through the state.
        public virtual void Enter() { _stage = EVENT.UPDATE; } 
        public virtual void Update() {  } 
        public virtual void Exit() { _stage = EVENT.EXIT; } 

        // The method that will get run from outside and progress the state through each of the different stages.
        public PlayerStates Process()
        {
            if (_stage == EVENT.ENTER) Enter();
            if (_stage == EVENT.UPDATE) Update();
            if (_stage == EVENT.EXIT)
            {
                Exit();
                return _nextState; // Notice that this method returns a 'state'.
            }
            return this; // If we're not returning the nextState, then return the same state.
        }
    }
}