using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace APG {
    public class PiPlayerAgent : PlayerAgent {
        public override void Heuristic(in ActionBuffers actionsOut) {
            // 0 = move forward
            // 1 = move right
            // 2 = look right
            // 3 = look up
            // 4 = jump
            var discreteActionsOut = actionsOut.DiscreteActions;
            discreteActionsOut.Clear();

            discreteActionsOut[0] = _playerInputHandler.moveDirection.x > 0.5f ? 1 : 0;
            discreteActionsOut[0] = _playerInputHandler.moveDirection.x < -0.5f ? 2 : discreteActionsOut[0];

            discreteActionsOut[1] = _playerInputHandler.moveDirection.z > 0.5f ? 1 : 0;
            discreteActionsOut[1] = _playerInputHandler.moveDirection.z < -0.5f ? 2 : discreteActionsOut[1];

            discreteActionsOut[2] = _playerInputHandler.lookRight > 0.5f ? 1 : 0;
            discreteActionsOut[2] = _playerInputHandler.lookRight < -0.5f ? 2 : discreteActionsOut[2];

            discreteActionsOut[3] = _playerInputHandler.lookUp > 0.5f ? 1 : 0;
            discreteActionsOut[3] = _playerInputHandler.lookUp < -0.5f ? 2 : discreteActionsOut[3];

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
    }
}