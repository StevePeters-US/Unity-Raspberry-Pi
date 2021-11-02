using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace APG {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerAgentMovementController : MonoBehaviour {

        [HideInInspector] public PiPlayerAgent playerAgent;

        private Rigidbody _rigidbody;
        [SerializeField] private float maxAcceleration = 15;
        [SerializeField] private float maxVelocity = 35f;
        [SerializeField] private float turnSpeed = 150f;

        [SerializeField] private ScriptableObject botMovementData;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void UpdateMovement(MoveDirectionDiscrete moveDirection) {
            _rigidbody.maxAngularVelocity = 20;
            // These need to be in a scriptable object
            float moveForce = 75f;
            float turnForce = 75f;

            switch (moveDirection) {
                case MoveDirectionDiscrete.none:
                    break;
                case MoveDirectionDiscrete.forward:
                    _rigidbody.AddForce(moveForce * _rigidbody.transform.forward);
                    break;
                case MoveDirectionDiscrete.backward:
                    _rigidbody.AddForce(moveForce * -_rigidbody.transform.forward);
                    break;
                case MoveDirectionDiscrete.left:
                    _rigidbody.AddTorque(turnForce * -_rigidbody.transform.up);
                    break;
                case MoveDirectionDiscrete.right:
                    _rigidbody.AddTorque(turnForce * _rigidbody.transform.up);
                    break;
            }


         /*   Vector3 velocity = _rigidbody.velocity;

            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = transform.right;
            right.y = 0f;
            right.Normalize();
            Vector3 desiredVelocity = (right * inputDir.x + forward * inputDir.z) * maxVelocity;
            desiredVelocity.y = velocity.y;

            float maxSpeedChange = maxAcceleration * Time.fixedDeltaTime;
            velocity = Vector3.MoveTowards(velocity, desiredVelocity, maxSpeedChange);
            _rigidbody.velocity = velocity;

            // Turning
            float angle = lookDir.x * turnSpeed;
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * angle);*/
        }
    }
}
