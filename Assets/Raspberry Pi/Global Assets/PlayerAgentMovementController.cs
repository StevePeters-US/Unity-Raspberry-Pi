using UnityEngine;

namespace APG {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerAgentMovementController : MonoBehaviour {

        [HideInInspector] public PiPlayerAgent playerAgent;

        private Rigidbody _rigidbody;
        [SerializeField] private PiCarSO piCarSO;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void UpdateMovement(MoveDirectionDiscrete moveDirection) {
            _rigidbody.maxAngularVelocity = piCarSO.maxAngularVelocity;

            switch (moveDirection) {
                case MoveDirectionDiscrete.none:
                    break;
                case MoveDirectionDiscrete.forward:
                    _rigidbody.AddForce(piCarSO.moveForce * _rigidbody.transform.forward);
                    break;
                case MoveDirectionDiscrete.backward:
                    _rigidbody.AddForce(piCarSO.moveForce * -_rigidbody.transform.forward);
                    break;
                case MoveDirectionDiscrete.left:
                    _rigidbody.AddTorque(piCarSO.turnForce * -_rigidbody.transform.up);
                    break;
                case MoveDirectionDiscrete.right:
                    _rigidbody.AddTorque(piCarSO.turnForce * _rigidbody.transform.up);
                    break;
            }
        }
    }
}
