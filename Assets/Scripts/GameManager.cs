using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TOOLS FOR DEBUGGING

    //DEBUGGING NOTES

    //-----------------------------------
    public static GameManager instance;

    //GOLD
    public int gold;

    //TIME TRACKING
    [HideInInspector] public int seconds;
    [HideInInspector] public int minutes;
    [HideInInspector] public int hours;

    void Awake()
    {
        if (instance == null) //check if instance exists
        {
            instance = this; //if not set the instance to this
        }
        else if (instance != this) //if it exists but is not this instance
        {
            Destroy(gameObject); //destroy it
        }
        DontDestroyOnLoad(gameObject); //set this to be persistable across scenes

        StartCoroutine(UpdateTime());
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Consistently keeps game time updated
    /// </summary>
    IEnumerator UpdateTime()
    {
        while (hours != 99 && minutes != 60 && seconds != 60)
        {
            yield return new WaitForSecondsRealtime(1);
            seconds++;

            if (seconds == 60)
            {
                minutes++;
                seconds = 0;
            }

            if (minutes == 60)
            {
                hours++;
                minutes = 0;
            }
        }
    }

}