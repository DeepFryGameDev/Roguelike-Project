// Purpose: Contains combat management vars and methods
// Directions: Simply call CombatManager to reference
// Other notes: Some variables are still to be moved here

using UnityEngine;

public static class CombatManager
{
    public static float enemyDespawnTime = 3f; // after an enemy dies, how many seconds pass before the corpse despawns

    public static float baseEnemySpawnDelay = 10f; // Minimum number of seconds to wait before spawning enemy

    public static float enemySpawnDelayMax = 10f; // Maximum number of seconds to wait before spawning enemy (taken into account after base spawn delay)

    public static float attackResetPeriod = .2f; // Global wait time to wait after attack cooldowns before attack can be fired again

    public static float enemyRandomPathingRange = 15f; // Used to determine distance from the enemy unit's current to target position

    public static float enemyPathingToPlayerStoppingDistance = 3.5f; // used to determine where to stop when pathing to player's position

    public static float enemyPathingToRandomStoppingDistance = 0f;  // used to determine where to stop when pathing to a random position

}
