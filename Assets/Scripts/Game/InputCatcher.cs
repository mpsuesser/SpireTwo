using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCatcher : MonoBehaviour
{
    private Movement.Direction prevDir;

    private float prevYaw = 0.0f;
    private float prevPitch = 0.0f;

    private Dictionary<KeyCode, Ability> hotkeysToAbilities;

    private void Awake() {
        prevDir = Movement.Direction.NONE;

        hotkeysToAbilities = null;
    }

    public void RegisterAbilities(Ability[] _abilities) {
        hotkeysToAbilities = new Dictionary<KeyCode, Ability>();
        for (int i = 0; i < _abilities.Length; i++) {
            hotkeysToAbilities.Add(_abilities[i].Hotkey, _abilities[i]);
        }
    }

    private void Update() {
        // Helper
        bool HeroAvailable = (HeroController.instance != null && HeroController.instance.H != null);

        // Escape toggles the cursor lock.
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CursorManager.instance.ToggleCursorLock();
        }

        // See if any of our ability hotkeys were pressed.
        if (hotkeysToAbilities != null) {
            foreach (KeyCode _kc in hotkeysToAbilities.Keys) {
                if (Input.GetKeyDown(_kc)) {
                    hotkeysToAbilities[_kc].PressedClient();
                }
            }
        }

        // Jump check.
        if (Input.GetKeyDown(KeyCode.Space)) {
            ClientSend.JumpInput();
            if (HeroAvailable) {
                HeroController.instance.H.SetJumping();
            }
        }

        // Gather if any input was given by WASD or mouse.
        Movement.Direction dir = UpdateMovementInput();
        Vector3 eulerAngles = UpdateRotationInput();

        // Update run animation
        if (HeroAvailable) {
            HeroController.instance.H.SetRunning(dir);
        }

        // Send movement input back to the server so that position and rotation can be updated there.
        ClientSend.MovementInput(dir, eulerAngles);

        // Reflect our new rotation locally if a hero is being controlled
        if (HeroAvailable) {
            Vector3 flattenedRotation = new Vector3(0f, eulerAngles.y, 0f);

            HeroController.instance.H.UpdateRotation(flattenedRotation);
        }

        if (CameraController.instance != null) {
            CameraController.instance.UpdateRotation(eulerAngles);
        }
    }

    private Movement.Direction UpdateMovementInput() {
        Movement.Direction inputDirection = Movement.Direction.NONE;

        if (Input.GetKey(KeyCode.W)) inputDirection |= Movement.Direction.FORWARD;
        if (Input.GetKey(KeyCode.D)) inputDirection |= Movement.Direction.RIGHT;
        if (Input.GetKey(KeyCode.A)) inputDirection |= Movement.Direction.LEFT;
        if (Input.GetKey(KeyCode.S)) inputDirection |= Movement.Direction.BACKWARD;

        // Cancel out contradictory inputs
        if (((inputDirection & Movement.Direction.FORWARD) != 0) && ((inputDirection & Movement.Direction.BACKWARD) != 0)) {
            inputDirection &= ~Movement.Direction.FORWARD;
            inputDirection &= ~Movement.Direction.BACKWARD;
        }

        if (((inputDirection & Movement.Direction.RIGHT) != 0) && ((inputDirection & Movement.Direction.LEFT) != 0)) {
            inputDirection &= ~Movement.Direction.RIGHT;
            inputDirection &= ~Movement.Direction.LEFT;
        }

        if (inputDirection != prevDir) {
            prevDir = inputDirection;
        }

        return inputDirection;
    }

    private Vector3 UpdateRotationInput() {
        float yaw = Input.GetAxis("Mouse X");
        float pitch = Input.GetAxis("Mouse Y");

        prevYaw += yaw * Constants.HORIZONTAL_MOUSE_SENSITIVITY;
        prevPitch -= pitch * Constants.VERTICAL_MOUSE_SENSITIVITY;

        return new Vector3(prevPitch, prevYaw);
    }
}