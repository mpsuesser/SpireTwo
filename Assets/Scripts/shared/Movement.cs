public static class Movement {
    public enum Direction {
        NONE = 0,
        FORWARD = 1 << 0,
        BACKWARD = 1 << 1,
        RIGHT = 1 << 2,
        LEFT = 1 << 3
    }
}
