using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenuController : MonoBehaviour
{
    public List<Image> buttonImages;

    public Sprite lockedImage;

    public Sprite activeImage;

    public Sprite perfectImage;

    public GameObject AudioScreen;
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

        if (!PlayerPrefs.HasKey("Level1Perfect"))
        {
            PlayerPrefs.SetInt("Level1Perfect", 0);
        }

        if (!PlayerPrefs.HasKey("Level2Perfect"))
        {
            PlayerPrefs.SetInt("Level2Perfect", 0);
        }

        if (!PlayerPrefs.HasKey("Level3Perfect"))
        {
            PlayerPrefs.SetInt("Level3Perfect", 0);
        }

        setUpButtons();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void setUpButtons()
    {
        for(int i = 0; i < buttonImages.Count; ++i)
        {
            int currentLevel = i + 1;
            if(currentLevel > 1 && PlayerPrefs.GetInt("Level" + i.ToString() + "Complete") == 0)
            {
                buttonImages[i].sprite = lockedImage;
            }
            else
            {
                if(PlayerPrefs.GetInt("Level" + currentLevel.ToString() + "Perfect") == 0)
                {
                    buttonImages[i].sprite = activeImage;
                }
                else
                {
                    buttonImages[i].sprite = perfectImage;
                }
            }
        }
    }

    public void ResetLevels()
    {
        PlayerPrefs.SetInt("Level1Complete", 0);
        PlayerPrefs.SetInt("Level2Complete", 0);
        PlayerPrefs.SetInt("Level3Complete", 0);

        PlayerPrefs.SetInt("Level1Perfect", 0);
        PlayerPrefs.SetInt("Level2Perfect", 0);
        PlayerPrefs.SetInt("Level3Perfect", 0);

        setUpButtons();
    }

    public void OpenAudioScreen()
    {
        AudioScreen.SetActive(true);
    }
}
