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
    [RequireComponent(typeof(WebSocketClient))]
    //[RequireComponent(typeof(Collider))]
    //[RequireComponent(typeof(Rigidbody))]
    public class PiPlayerAgent : Agent {

        protected PlayerInputHandler _playerInputHandler;
        private PlayerAgentMovementController _movementController;
        private WebSocketClient _webSocketClient;
        // private Rigidbody _rigidbody;
        //public Rigidbody Rigidbody { get => _rigidbody; }

        protected Vector3 agentMoveInputDirection;
        protected Vector2 agentLookDir;
        protected bool agentHasJumpInput;

        private Vector3 localPosition;

        public bool controlIRLCar = false;
        public float movementControlPeriod = 0.1f;

        // If true, control through websocketclient, otherwise through playeragentmovementcontroller
        private bool usingHeuristicBehaviors;

        //[SerializeField] private float killHeight = -2;

        protected void Awake() {
            //  _rigidbody = GetComponent<Rigidbody>();
            _playerInputHandler = GetComponent<PlayerInputHandler>();

            _movementController = GetComponent<PlayerAgentMovementController>();
            _movementController.playerAgent = this;

            _webSocketClient = GetComponent<WebSocketClient>();

            usingHeuristicBehaviors = GetComponent<Unity.MLAgents.Policies.BehaviorParameters>().BehaviorType == Unity.MLAgents.Policies.BehaviorType.HeuristicOnly;

            if (controlIRLCar) { _webSocketClient.InitializeWebSocketClient(); }

            InvokeRepeating("ControlAgentMovement", 0, movementControlPeriod);
        }

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
            //  sensor.AddObservation(Rigidbody.velocity);

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
            float reward = -1;
            if (MaxStep > 0)
                reward = -(1.0f / MaxStep);
            AddReward(reward);
        }

        public void AgentReachedGoal() {
            AddReward(1);
            EndEpisode();
        }

        //[SerializeField] private PlayerAgentScriptableObject playerAgentSO;
        public override void Heuristic(in ActionBuffers actionsOut) {
            // 0 = move forward
            // 1 = move right
            // 2 = look right
            // 3 = look up
            // 4 = jump
 /*           var discreteActionsOut = actionsOut.DiscreteActions;
            discreteActionsOut.Clear();

            discreteActionsOut[0] = _playerInputHandler.moveDirection.x > 0.5f ? 1 : 0;
            discreteActionsOut[0] = _playerInputHandler.moveDirection.x < -0.5f ? 2 : discreteActionsOut[0];

            discreteActionsOut[1] = _playerInputHandler.moveDirection.z > 0.5f ? 1 : 0;
            discreteActionsOut[1] = _playerInputHandler.moveDirection.z < -0.5f ? 2 : discreteActionsOut[1];

            discreteActionsOut[2] = _playerInputHandler.lookRight > 0.5f ? 1 : 0;
            discreteActionsOut[2] = _playerInputHandler.lookRight < -0.5f ? 2 : discreteActionsOut[2];

            discreteActionsOut[3] = _playerInputHandler.lookUp > 0.5f ? 1 : 0;
            discreteActionsOut[3] = _playerInputHandler.lookUp < -0.5f ? 2 : discreteActionsOut[3];*/

            // We can map index 0 to the jump input where 1 == has jumping input and 0 == no jumping input 
            //  discreteActionsOut[0] = _playerInputHandler.hasJumpInput ? 1 : 0;
        }

        // Convert output from model into usable variables that can be used to pilot the agent.
        public override void OnActionReceived(ActionBuffers actionBuffers) {
            // Move right
            agentMoveInputDirection.x = 0;
            if (actionBuffers.DiscreteActions[0] == 1)
                agentMoveInputDirection.x = 1;
            else if (actionBuffers.DiscreteActions[0] == 2)
                agentMoveInputDirection.x = -1;

            // Move up
            agentMoveInputDirection.z = 0;
            if (actionBuffers.DiscreteActions[1] == 1)
                agentMoveInputDirection.z = 1;
            else if (actionBuffers.DiscreteActions[1] == 2)
                agentMoveInputDirection.z = -1;

            // Look right
            agentLookDir.x = 0;
            if (actionBuffers.DiscreteActions[2] == 1)
                agentLookDir.x = 1;
            else if (actionBuffers.DiscreteActions[2] == 2)
                agentLookDir.x = -1;

            // Look up
            agentLookDir.y = 0;
            if (actionBuffers.DiscreteActions[3] == 1)
                agentLookDir.y = 1;
            else if (actionBuffers.DiscreteActions[3] == 2)
                agentLookDir.y = -1;

            //  agentHasJumpInput = actionBuffers.DiscreteActions[0] == 1 ? true : false;
        }

        private void ControlAgentMovement() {
            // Connect to our real world car using the web socket client
            if (controlIRLCar) {
                _webSocketClient.SendMessageToServer(
                    _playerInputHandler.hasLEDInput ? "on" : "off",
                    false,
                    false,
                    false,
                    false);
            }

            // Otherwise control our simulated car
            else {
                _movementController.UpdateMovement(Vector3.zero, Vector2.zero, false);
            }
        }
    }
}