using Player.StateMachines.Combat;
using Player.StateMachines.Interaction;
using Player.StateMachines.Movement;
using UnityEngine;

namespace Player.StateMachines {
    public class StateMachineManager : MonoBehaviour {
        [SerializeField] private MovementStateMachine movementStateMachine;
        [SerializeField] private CombatStateMachine combatStateMachine;
        [SerializeField] private InteractionStateMachine interactionStateMachine;

        public string GetMovementCurrentStateName() {
            return movementStateMachine.CurrentState.name;
        }

        public string GetCombatCurrentStateName() {
            return combatStateMachine.CurrentState.name;
        }

        public string GetInteractionCurrentStateName() {
            return interactionStateMachine.CurrentState.name;
        }
    }
}