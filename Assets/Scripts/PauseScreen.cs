using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour {

    public GameObject audioScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void OpenScreen()
    {
        gameObject.SetActive(true);
    }

    public void OpenAudio()
    {
        audioScreen.gameObject.SetActive(true);
    }

    public void CloseScreen()
    {
        audioScreen.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

}
