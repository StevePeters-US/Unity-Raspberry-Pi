using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace APG {
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerAgentMovementController))]
    [RequireComponent(typeof(DecisionRequester))]
    [RequireComponent(typeof(WebSocketClient))]
    public class PiPlayerAgent : Agent {

        protected PlayerInputHandler _playerInputHandler;
        private PlayerAgentMovementController _movementController;
        private WebSocketClient _webSocketClient;
        public bool controlIRLCar = false;
        public float movementControlPeriod = 0.1f;
        MoveDirectionDiscrete currentMoveDirection;
        public Transform Target;
        private StatsRecorder stats;

        #region initialize
        public override void Initialize() {
            stats = Academy.Instance.StatsRecorder;
        }

        protected void Awake() {
            _playerInputHandler = GetComponent<PlayerInputHandler>();

            _movementController = GetComponent<PlayerAgentMovementController>();
            _movementController.playerAgent = this;

            _webSocketClient = GetComponent<WebSocketClient>();

            if (controlIRLCar) { _webSocketClient.InitializeWebSocketClient(); }

            InvokeRepeating("ControlAgentMovement", 0, movementControlPeriod);
        }

        #endregion

        // A player agent has entered the goal, ask the game state what to do
        public void GoalTriggered() {
            Debug.Log("Goal Triggered");
            AddReward(1);
            EndEpisode();
        }

        public override void OnEpisodeBegin() {

            Target.localPosition = new Vector3(Random.value * 1,
                                        0.14f,
                                        Random.value * 1 + 1);

            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(0f, 0.1f, 0f);
            transform.rotation = new Quaternion();
        }

        // These are the observations that are fed to the model on decision request (defaults to every 5th fixed frame in the decision requester component attached to the agent).
        // The number of observations needs to match the number of vector observations in the behavior parameters on the player agent
        public override void CollectObservations(VectorSensor sensor) {
            // The camera is currently the only source of information for the agent
        }

        private void FixedUpdate() {
            // Existential penalty to encourage agent to move towards the goal quickly
            float reward = -1;
            if (MaxStep > 0)
                reward = -(1.0f / MaxStep);
            AddReward(reward);

            stats.Add("Distance from start", System.Convert.ToInt32(Vector3.Distance(transform.position, Vector3.zero)));
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