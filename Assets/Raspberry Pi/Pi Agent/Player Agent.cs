using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace APG {
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerAgentMovementController))]
    [RequireComponent(typeof(DecisionRequester))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerAgent : Agent {
        protected PlayerInputHandler _playerInputHandler;
        private PlayerAgentMovementController _movementController;
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody { get => _rigidbody; }

        protected Vector3 agentMoveInputDirection;
        protected Vector2 agentLookDir;
        protected bool agentHasJumpInput;

        //   private EnvironmentManager _envManager;
        //   public EnvironmentManager envManager { get => _envManager; }
        //   private Transform _envManagerTransform;

        private Vector3 localPosition;

        [SerializeField] private float killHeight = -2;

        //[SerializeField] private PlayerAgentScriptableObject playerAgentSO;

        protected void Awake() {
            /*    if (!UpdateEnvironmentManager())
                    Debug.Log("No environment manager found", this);*/

            _rigidbody = GetComponent<Rigidbody>();
            _playerInputHandler = GetComponent<PlayerInputHandler>();
            _movementController = GetComponent<PlayerAgentMovementController>();
           //!!!! _movementController.playerAgent = this;

            //    _envManagerTransform = envManager.transform;

            //SetAgentBehaviorTypeHeuristic(true);
        }
        /*     public bool UpdateEnvironmentManager()
             {
                 _envManager = GetComponentInParent<EnvironmentManager>();
                 return _envManager;
             }*/

        // I'm leaving this is in for now to give the player the option of manually controlling the agent or allowing the ai to take over
        private void SetAgentBehaviorTypeHeuristic(bool usesHeursiticBehaviors) {
            if (usesHeursiticBehaviors) {
                GetComponent<Unity.MLAgents.Policies.BehaviorParameters>().BehaviorType = Unity.MLAgents.Policies.BehaviorType.HeuristicOnly;
            }

            else {
                GetComponent<Unity.MLAgents.Policies.BehaviorParameters>().BehaviorType = Unity.MLAgents.Policies.BehaviorType.InferenceOnly;
            }
        }

        public override void OnEpisodeBegin() {
            // _envManager.ResetEnvironment();
        }

        // These are the observations that are fed to the model on decision request (defaults to every 5th fixed frame in the decision requester component attached to the agent).
        // The number of observations needs to match the number of vector observations in the behavior parameters on the player agent
        public override void CollectObservations(VectorSensor sensor) {
            // 6 observations
            sensor.AddObservation(localPosition);
            sensor.AddObservation(Rigidbody.velocity);

            // 4 Observations
            sensor.AddObservation(transform.rotation);
        }

        private void FixedUpdate() {
            /*  localPosition = _envManagerTransform.InverseTransformPoint(transform.position);

              // Check if agent is withing valid bounds
              if (localPosition.y < killHeight)
              {
                  SetReward(-1.0f);
                  EndEpisode();
              }*/

          //  _movementController.UpdateMovement(agentMoveInputDirection, agentLookDir, agentHasJumpInput);

            // Existential penalty to encourage agent to move towards the goal quickly
            AddReward(-(1.0f / MaxStep));
        }

        public void AgentReachedGoal() {
            AddReward(1);
            EndEpisode();
        }
    }
}