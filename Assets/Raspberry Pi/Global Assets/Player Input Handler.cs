using UnityEngine;

namespace APG {
    public class PlayerInputHandler : MonoBehaviour {
        private PiInputActions playerInputControls;

        private Vector2 inputMove;
        private Vector2 inputLook;
        private bool inputJump;

        // x direction maps to z direction in 3d space
        public float moveForward => inputMove.x;
        public float moveRight => inputMove.y;
        public float moveMagnitude => Vector2.SqrMagnitude(inputMove);
        public Vector3 moveDirection => new Vector3(inputMove.x, 0, inputMove.y);

        public float lookUp => inputLook.y;
        public float lookRight => inputLook.x;
        public Vector2 lookDir => inputLook;

        public bool hasJumpInput => inputJump;


        private void Awake() {
            playerInputControls = new PiInputActions();

            playerInputControls.PiCar.Move.performed += context => inputMove = context.ReadValue<Vector2>();
            playerInputControls.PiCar.Move.canceled += context => inputMove = Vector2.zero;

           // playerInputControls.PiCar.Look.performed += context => inputLook = context.ReadValue<Vector2>();
           // playerInputControls.PiCar.Look.canceled += context => inputLook = Vector2.zero;

          //  playerInputControls.PiCar.Jump.performed += context => inputJump = context.ReadValue<float>() > 0.5f;
           // playerInputControls.PiCar.Jump.canceled += context => inputJump = false;

            playerInputControls.PiCar.Pause.performed += context => PauseApplication();
            playerInputControls.PiCar.Quit.performed += context => QuitApplication();
        }

        private void OnEnable() {
            playerInputControls.PiCar.Enable();
        }
        private void OnDisable() {
            playerInputControls.PiCar.Disable();
        }

        // Set game pause in the game manager
        private void PauseApplication() {
            //gameManager.TogglePaused();
        }

        private void QuitApplication() { Application.Quit(); }
    }
}