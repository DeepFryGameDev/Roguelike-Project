using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Purpose: An old script that may not be needed
// Directions: 
// Other notes:

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Begins scene transition animation when changing scenes
    /// </summary>
    /// <param name="sceneName">Name of scene to be loaded</param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(SceneTransition(sceneName));
    }

    /// <summary>
    /// Begins scene transition animation when loading a battle scene
    /// </summary>
    /// <param name="sceneName">Name of battle scene to be loaded</param>
    public void LoadBattle(string sceneName)
    {
        BattleTransition(sceneName);
    }

    /// <summary>
    /// Coroutine.  Facilitates processing scene transition animation
    /// </summary>
    /// <param name="sceneName">Name of scene to be loaded after animation</param>
    IEnumerator SceneTransition(string sceneName)
    {
        /*GameObject.Find("Player").GetComponent<PlayerController2D>().enabled = false;

        crossfadeTransition.SetTrigger("Start");

        yield return new WaitForSeconds(crossfadeTransitionTime);
        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(.1f);
        GameObject.Find("Player").GetComponent<PlayerController2D>().enabled = false;
        */
        yield return new WaitForSeconds(1.5f);

        //GameObject.Find("Player").GetComponent<PlayerController2D>().enabled = true;
    }

    /// <summary>
    /// Facilitates processing scene transition animation for loading battle
    /// </summary>
    /// <param name="sceneName">Name of battle scene to be loaded</param>
    void BattleTransition(string sceneName)
    {
        /*
        int sceneIndex = sceneIndexFromName(sceneName);

        var battleBlur = new BattleBlurTransition()
        {
            nextScene = sceneIndex,
            duration = 2.0f,
            blurMax = .04f
        };
        TransitionKit.instance.transitionWithDelegate(battleBlur);*/
    }


    //-----Tools for above methods-----

    /// <summary>
    /// Returns index from given scene name
    /// </summary>
    /// <param name="sceneName">Name of scene to gather index</param>
    private int sceneIndexFromName(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string testedScreen = NameFromIndex(i);
            //print("sceneIndexFromName: i: " + i + " sceneName = " + testedScreen);
            if (testedScreen == sceneName)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Returns name of scene from given index - checks scenes from path so the loaded scenes issue can be worked around
    /// </summary>
    /// <param name="BuildIndex">Index of scene to gather name</param>
    private static string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

}