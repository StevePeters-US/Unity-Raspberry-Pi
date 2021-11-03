using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace APG {
    [CreateAssetMenu(fileName = "PiCarSO", menuName = "ScriptableObjects/PiCarScriptableObject", order = 1)]
    public class PiCarSO : ScriptableObject {
        public float maxAngularVelocity = 20;
        public float moveForce = 75f;
        public float turnForce = 75f;
    }
}
