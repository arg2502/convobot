using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenuController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		if(!PlayerPrefs.HasKey("Level1Complete"))
        {
            PlayerPrefs.SetInt("Level1Complete", 0);
        }

        if (!PlayerPrefs.HasKey("Level2Complete"))
        {
            PlayerPrefs.SetInt("Level2Complete", 0);
        }

        if (!PlayerPrefs.HasKey("Level3Complete"))
        {
            PlayerPrefs.SetInt("Level3Complete", 0);
        }

        if (!PlayerPrefs.HasKey("Level4Complete"))
        {
            PlayerPrefs.SetInt("Level4Complete", 0);
        }

        if (!PlayerPrefs.HasKey("Level5Complete"))
        {
            PlayerPrefs.SetInt("Level5" +
                "Complete", 0);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
