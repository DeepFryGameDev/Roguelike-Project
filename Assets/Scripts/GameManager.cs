// Purpose: Contains values to be held over the lifetime of the game
// Directions: Simply call GameManager as it is a static class
// Other notes:

public static class GameManager
{
    //TOOLS FOR DEBUGGING

    //DEBUGGING NOTES

    //-----------------------------------

    //GOLD
    static int gold; // Currency to buy equipment
    public static int GetGold() { return gold; }

    static bool gameSet; // When all required values to start the game have been set, this is turned to true.
    public static bool GetGameSet() { return gameSet; }
    public static void SetGameSet(bool setGameSet) { gameSet = setGameSet; }

    /// <summary>
    /// Updates the player's gold count
    /// </summary>
    /// <param name="goldToChange">Increases gold count with a positive value, or decreases gold count with a negative value</param>
    public static void ChangeGold(int goldToChange) 
    { 
        gold += goldToChange;
    }
}