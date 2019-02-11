using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource mainBG;
    public AudioSource robotBG;
    float fadeRate = 1f;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start () {
        robotBG.volume = 0f;

        mainBG.Play();
        robotBG.Play();
        
	}
	
    public void StartRobotMusic()
    {
        StartCoroutine(StartRobot());
    }

    public void StopRobotMusic()
    {
        StartCoroutine(StopRobot());
    }

    IEnumerator StartRobot()
    {
        while(robotBG.volume < 1)
        {
            robotBG.volume += fadeRate * Time.deltaTime;
            yield return null;
        }
        robotBG.volume = 1;
    }

    IEnumerator StopRobot()
    {
        while(robotBG.volume > 0)
        {
            robotBG.volume -= fadeRate * Time.deltaTime;
            yield return null;
        }
        robotBG.volume = 0;
    }

}
