using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace APG
{
    public class PiGoal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) {
            Debug.Log("Goal Entered");
            PiPlayerAgent playerAgent = other.GetComponent<PiPlayerAgent>();
            if (playerAgent) {
                playerAgent.GoalTriggered();
            }
        }

    }
}
