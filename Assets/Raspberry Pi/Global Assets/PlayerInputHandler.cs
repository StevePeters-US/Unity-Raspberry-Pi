using UnityEngine;

namespace APG {
    public enum MoveDirectionDiscrete {
        none,
        forward,
        backward,
        left,
        right
    }

    public class PlayerInputHandler : MonoBehaviour {
        private PiInputActions playerInputControls;

        private Vector2 inputMove;
        private Vector2 inputLook;
        private bool inputLED;

        // x direction maps to z direction in 3d space
        public float moveForward => inputMove.x;
        public float moveRight => inputMove.y;
        public float moveMagnitude => Vector2.SqrMagnitude(inputMove);
        public Vector3 moveDirection => new Vector3(inputMove.y, 0, inputMove.x);

        public float lookUp => inputLook.y;
        public float lookRight => inputLook.x;
        public Vector2 lookDir => inputLook;

        public bool hasLEDInput => inputLED;


        private void Awake() {
            playerInputControls = new PiInputActions();

            playerInputControls.PiCar.Move.performed += context => inputMove = context.ReadValue<Vector2>();
            playerInputControls.PiCar.Move.canceled += context => inputMove = Vector2.zero;

            playerInputControls.PiCar.LED.performed += context => inputLED = context.ReadValue<float>() > 0.5f;
            playerInputControls.PiCar.LED.canceled += context => inputLED = false;

            playerInputControls.PiCar.Pause.performed += context => PauseApplication();
            playerInputControls.PiCar.Quit.performed += context => QuitApplication();
        }

        private void OnEnable() {
            playerInputControls.PiCar.Enable();
        }
        private void OnDisable() {
            playerInputControls.PiCar.Disable();
        }

        public MoveDirectionDiscrete GetMoveDirectionDiscrete() {
            MoveDirectionDiscrete moveDirectionDiscrete = new MoveDirectionDiscrete();

            if (moveDirection.x > 0.5f) {
                moveDirectionDiscrete = MoveDirectionDiscrete.forward;
            }

            else if (moveDirection.x < -0.5f) {
                moveDirectionDiscrete = MoveDirectionDiscrete.backward;
            }

            else if (moveDirection.z < -0.5f) {
                moveDirectionDiscrete = MoveDirectionDiscrete.left;
            }

            else if (moveDirection.z > 0.5f) {
                moveDirectionDiscrete = MoveDirectionDiscrete.right;
            }

            else {
                moveDirectionDiscrete = MoveDirectionDiscrete.none;
            }

            return moveDirectionDiscrete;
        }

        // Set game pause in the game manager
        private void PauseApplication() {
            //gameManager.TogglePaused();
        }

        private void QuitApplication() { Application.Quit(); }
    }
}