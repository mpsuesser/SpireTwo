using UnityEngine;

public static class Constants {
    public static readonly string VERSION = "0.0.1";

    public static readonly int SERVER_LISTEN_PORT = 26950;
    public static readonly int CLIENT_LISTEN_PORT = 26951;

    public static readonly int GROUND_LAYER_MASK = 1 << 3;

    public static readonly float SERVER_FIXED_UPDATE_FREQUENCY = 0.0333333f;

    public static readonly float ENEMY_CANVAS_DISTANCE = 20f;
    public static readonly float ENEMY_ROTATION_180_LERP_TIME = 2f;

    // TODO: Turn these into settings.
    public static readonly float VERTICAL_MOUSE_SENSITIVITY = 2.0f;
    public static readonly float HORIZONTAL_MOUSE_SENSITIVITY = 2.0f;
    public static readonly KeyCode[] ABILITY_HOTKEYS = new KeyCode[] { 
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Q,
        KeyCode.E,
        KeyCode.R,
        KeyCode.F,
        KeyCode.T
    };

    public static readonly float GCD_TIME = 1.0f;

    public static readonly float ENEMY_DEATH_DESTROY_TIME = 5.0f;
}
