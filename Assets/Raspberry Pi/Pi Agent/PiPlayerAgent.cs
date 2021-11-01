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

        public bool controlIRLCar = false;
        public float movementControlPeriod = 0.1f;

        private PiGoal piGoal;

        // If true, control through websocketclient, otherwise through playeragentmovementcontroller
        // private bool usingHeuristicBehaviors;

        MoveDirectionDiscrete currentMoveDirection;
        //[SerializeField] private float killHeight = -2;

        protected void Awake() {
            //  _rigidbody = GetComponent<Rigidbody>();
            _playerInputHandler = GetComponent<PlayerInputHandler>();

            _movementController = GetComponent<PlayerAgentMovementController>();
            _movementController.playerAgent = this;

            _webSocketClient = GetComponent<WebSocketClient>();

            // usingHeuristicBehaviors = GetComponent<Unity.MLAgents.Policies.BehaviorParameters>().BehaviorType == Unity.MLAgents.Policies.BehaviorType.HeuristicOnly;

            if (controlIRLCar) { _webSocketClient.InitializeWebSocketClient(); }

            InvokeRepeating("ControlAgentMovement", 0, movementControlPeriod);
        }

        public void SubscribeToGoal(PiGoal newPiGoal) {
            if (piGoal)
                piGoal.OnGoalTriggered -= GoalTriggered;

            piGoal = newPiGoal;
            piGoal.OnGoalTriggered += GoalTriggered;
        }

        // A player agent has entered the goal, ask the game state what to do
        public void GoalTriggered() {
            Debug.Log("Goal Triggered");
            AddReward(1);
            EndEpisode();
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
            //sensor.AddObservation(localPosition);
            //  sensor.AddObservation(Rigidbody.velocity);

            // 4 Observations
            //sensor.AddObservation(transform.rotation);
        }

        private void FixedUpdate() {
            // Existential penalty to encourage agent to move towards the goal quickly
            float reward = -1;
            if (MaxStep > 0)
                reward = -(1.0f / MaxStep);
            AddReward(reward);
        }


        public override void Heuristic(in ActionBuffers actionsOut) {
            // 0 = none
            // 1 = move forward
            // 2 = move backwards
            // 3 = turn left
            // 4 = turn right

            var discreteActionsOut = actionsOut.DiscreteActions;
            discreteActionsOut.Clear();

            discreteActionsOut[0] = (int)_playerInputHandler.GetMoveDirectionDiscrete();
        }

        // Convert output from model into usable variables that can be used to pilot the agent.
        public override void OnActionReceived(ActionBuffers actionBuffers) {
            currentMoveDirection = (MoveDirectionDiscrete)actionBuffers.DiscreteActions[0];
        }

        /// <summary>
        /// This is called at a fixed rate, independent of the agent so that we don't overwhelm the network.
        /// </summary>
        private void ControlAgentMovement() {

            // Connect to our real world car using the web socket client
            if (controlIRLCar) {
                _webSocketClient.SendMessageToServer(
                    _playerInputHandler.hasLEDInput ? "on" : "off",
                    currentMoveDirection == MoveDirectionDiscrete.forward,
                    currentMoveDirection == MoveDirectionDiscrete.backward,
                    currentMoveDirection == MoveDirectionDiscrete.left,
                    currentMoveDirection == MoveDirectionDiscrete.right);
            }

            // Otherwise control our simulated car
            else {
                _movementController.UpdateMovement(currentMoveDirection);
            }
        }
    }
}