using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource mainBG;
    public AudioSource robotBG;
    public List<AudioSource> sfxSources;
    float fadeRate = 1f;

    public static AudioManager instance;

    // SFX
    public AudioClip testSFX;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start () {
        robotBG.volume = 0f;
        mainBG.volume = 1f;

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
            mainBG.volume -= fadeRate * Time.deltaTime;
            yield return null;
        }
        robotBG.volume = 1;
        mainBG.volume = 0;
    }

    IEnumerator StopRobot()
    {
        while(robotBG.volume > 0)
        {
            robotBG.volume -= fadeRate * Time.deltaTime;
            mainBG.volume += fadeRate * Time.deltaTime;
            yield return null;
        }
        robotBG.volume = 0;
        mainBG.volume = 1;
    }


    public void PlaySFX(AudioClip clip = null, bool randomPitch = true)
    {
        // TEMP FOR TESTING
        if (clip == null)
            clip = testSFX;

        var pitchRandom = 1f;
        if (randomPitch)
        {
            // randomize pitch a little
            var low = 0.9f;
            var high = 1.1f;
            pitchRandom = Random.Range(low, high);
        }

        // find an empty source
        int sourceIndex = -1;

        // first, check if any sources already have the same clip
        for (int i = 0; i < sfxSources.Count; i++)
        {
            if (sfxSources[i].clip == clip) //!sources[i].isPlaying)
            {
                sourceIndex = i;
                break;
            }
        }

        // if no match, now check for any empty sources
        if (sourceIndex < 0)
        {
            for (int i = 0; i < sfxSources.Count; i++)
            {
                if (sfxSources[i].clip == null)
                {
                    sourceIndex = i;
                    break;
                }
            }
        }

        // if still no match, find one that is not paused (time == 0)
        if (sourceIndex < 0)
        {
            for (int i = 0; i < sfxSources.Count; i++)
            {
                if (sfxSources[i].time == 0)
                {
                    sourceIndex = i;
                    break;
                }
            }
        }

        // default to first just in case
        if (sfxSources[sourceIndex] == null)
            sourceIndex = 0;

        sfxSources[sourceIndex].pitch = pitchRandom;

        sfxSources[sourceIndex].clip = clip;
        sfxSources[sourceIndex].loop = false;
        sfxSources[sourceIndex].volume = 1f;
        //if (fade)
        //{
        //    StartCoroutine(FadeIn(sources[sourceIndex], pause));
        //}
        //else
        //{
        sfxSources[sourceIndex].Play();
        //}
    }
}
