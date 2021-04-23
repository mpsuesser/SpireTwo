using System.Collections.Generic;

public static class Names {
    public static Dictionary<DungeonID, string> Dungeon = new Dictionary<DungeonID, string>() {
        { DungeonID.Coliseum, "The Coliseum" }
    };

    public static Dictionary<Enemies, string> Enemy = new Dictionary<Enemies, string>() {
        { Enemies.GRUNT, "Grunt" },
        { Enemies.SPELLSLINGER, "Spellslinger" },
        { Enemies.LEON, "Leon" },
        { Enemies.FROSTY, "Frosty" }
    };

    public static Dictionary<Heroes, string> Hero = new Dictionary<Heroes, string>() {
        { Heroes.ATLAS, "Atlas" }
    };
}