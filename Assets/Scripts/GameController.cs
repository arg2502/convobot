﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private enum GameStages { IntroStage,
                              ConversationStage,
                              MoveToFacialAdjustmentStage,
                              FacialAdjustmentStage,
                              MoveToConversationStage,
                              ResolveScoringStage,
                              EndStage };

    private GameStages currentStage;

    public enum Emotions
    {
        Sad,
        Scared,
        Happy,
        Angry,
        Amused,
        Nervous,
        Annoyed,
        Romantic,
        Confused,
        Surprised,
        Disgusted
    };

    [System.Serializable]
    public struct LevelData
    {
        public List<string> conversationBits;
        public Emotions emotion;
    }

    
    public GameObject dialogWindow;
    public TextMeshPro dialogText;
    int conversationIndex = 0;
    int dialogIndex = 0;
    public GameObject robotCharacter;
    public Robot robotComponents;
    public GameObject otherCharacter;
    public GameObject robotHatch;
    public Vector3 robotOriginalPosition;
    public Vector3 otherCharacterOriginalPosition;
    public Vector3 robotZoomedPosition;
    public Vector3 otherCharacterOffscreenPosition;
    public Vector3 hatchOriginalPosition;
    public Vector3 hatchOffPosition;
    public GameObject timerWindow;
    public TextMeshPro timerText;
    System.DateTime adjustmentTimer;
    public int timerCooldown = 30;
    public int timerCooldownDecay = 5;
    public TextMeshPro resultText;
    bool isCorrect = false;
    public int chances = 3;
    public List<LevelData> levelDatas;
    void Start ()
    {
        robotComponents = robotCharacter.GetComponent<Robot>();
        ChangeStage(GameStages.IntroStage);
	}
	
	void Update ()
    {
        switch (currentStage)
        {
            case GameStages.IntroStage:
            {
                UpdateIntroStage();
                break;
            }
            case GameStages.ConversationStage:
            {
                UpdateConversationStage();
                break;
            }
            case GameStages.MoveToFacialAdjustmentStage:
            {
                UpdateMoveToFacialAdjustmentStage();
                break;
            }
            case GameStages.FacialAdjustmentStage:
            {
                UpdateFacialAdjustmentStage();
                break;
            }
            case GameStages.MoveToConversationStage:
            {
                UpdateMoveToConversationStage();
                break;
            }
            case GameStages.ResolveScoringStage:
            {
                UpdateResolveScoringStage();
                break;
            }
            case GameStages.EndStage:
            {
                UpdateEndStage();
                break;
            }
            default:
            {
                break;
            }
        }
    }

    void ChangeStage(GameStages stage)
    {
        currentStage = stage;
        StartStage();
    }

    void StartStage()
    {
        switch(currentStage)
        {
            case GameStages.IntroStage:
            {
                StartIntroStage();
                break;
            }
            case GameStages.ConversationStage:
            {
                StartConversationStage();
                break;
            }
            case GameStages.MoveToFacialAdjustmentStage:
            {
                StartMoveToFacialAdjustmentStage();
                break;
            }
            case GameStages.FacialAdjustmentStage:
            {
                StartFacialAdjustmentStage();
                break;
            }
            case GameStages.MoveToConversationStage:
            {
                StartMoveToConversationStage();
                break;
            }
            case GameStages.ResolveScoringStage:
            {
                StartResolveScoringStage();
                break;
            }
            case GameStages.EndStage:
            {
                StartEndStage();
                break;
            }
            default:
            {
                break;
            }
        }
    }

    void StartIntroStage()
    {
        //Do whatever movements or coroutines it takes to get the characters in their starting position
        //Once that is done move to the Conversation stage 
        ChangeStage(GameStages.ConversationStage);
    }

    void UpdateIntroStage()
    {
        //Handles any updates needed for the intro stage
    }

    void StartConversationStage()
    {
        if (conversationIndex >= levelDatas.Count)
        {
            ChangeStage(GameStages.EndStage);
        }
        else
        {
            dialogWindow.SetActive(true);
            dialogIndex = 0;
            List<string> dialog = levelDatas[conversationIndex].conversationBits;
            dialogText.text = dialog[dialogIndex];
        }
    }

    void UpdateConversationStage()
    {
        if(Input.GetMouseButtonUp(0))
        {
            List<string> dialog = levelDatas[conversationIndex].conversationBits;
            ++dialogIndex;
            if (dialogIndex >= dialog.Count)
            {
                dialogWindow.SetActive(false);
                ChangeStage(GameStages.MoveToFacialAdjustmentStage);
            }
            else
            {
                dialogText.text = dialog[dialogIndex];
            }
        }
    }

    void StartMoveToFacialAdjustmentStage()
    {
        StartCoroutine(MoveToFacialAdjustment());
    }

    IEnumerator MoveToFacialAdjustment()
    {
        yield return StartCoroutine(MoveOtherPersonAway());
        yield return StartCoroutine(MoveRobotUp());
        hatchOffPosition.y = robotHatch.transform.position.y;
        hatchOffPosition.z = robotHatch.transform.position.z;
        hatchOriginalPosition.y = robotHatch.transform.position.y;
        hatchOriginalPosition.z = robotHatch.transform.position.z;
        yield return StartCoroutine(MoveHatchOff());
        ChangeStage(GameStages.FacialAdjustmentStage);
    }

    IEnumerator MoveOtherPersonAway()
    {
        while (Vector3.Distance(otherCharacter.transform.position, otherCharacterOffscreenPosition) > 0.5f)
        {
            otherCharacter.transform.position = Vector3.MoveTowards(otherCharacter.transform.position, otherCharacterOffscreenPosition, 5 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoveRobotUp()
    {
        while (Vector3.Distance(robotCharacter.transform.position, robotZoomedPosition) > 0.5f)
        {
            robotCharacter.transform.position = Vector3.MoveTowards(robotCharacter.transform.position, robotZoomedPosition, 5 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoveHatchOff()
    {
        while (Vector3.Distance(robotHatch.transform.position, hatchOffPosition) > 0.01f)
        {
            robotHatch.transform.position = Vector3.MoveTowards(robotHatch.transform.position, hatchOffPosition, 5 * Time.deltaTime);
            yield return null;
        }
    }



    void UpdateMoveToFacialAdjustmentStage()
    {
    }

    void StartFacialAdjustmentStage()
    {
        //Mark time that we started this stage
        //All adjustment components are now interactable
        //Have the compontents themselves adjust a set of numeric values so that we can compare them to the correct answers later
        //If the player hits the done button then go to the Move To Conversation stage
        adjustmentTimer = System.DateTime.Now;
        timerWindow.SetActive(true);
    }

    void UpdateFacialAdjustmentStage()
    {
        System.TimeSpan ts = System.DateTime.Now - adjustmentTimer;
        if(ts.Seconds >= timerCooldown)
        {
            timerCooldown -= timerCooldownDecay;
            ChangeStage(GameStages.MoveToConversationStage);
        }
        else
        {
            int timeStamp = timerCooldown - ts.Seconds;
            timerText.text = timeStamp.ToString() + "s";
        }
    }

    void StartMoveToConversationStage()
    {
        timerWindow.SetActive(false);
        StartCoroutine(MoveToConversation());
    }

    void UpdateMoveToConversationStage()
    {
        //Handles any updates needed for the Move To Conversation stage
    }

    IEnumerator MoveToConversation()
    {
        yield return StartCoroutine(MoveHatchOn());
        yield return StartCoroutine(MoveRobotBack());
        yield return StartCoroutine(MoveOtherPersonBack());
        ChangeStage(GameStages.ResolveScoringStage);
    }

    IEnumerator MoveOtherPersonBack()
    {
        while (Vector3.Distance(otherCharacter.transform.position, otherCharacterOriginalPosition) > 0.5f)
        {
            otherCharacter.transform.position = Vector3.MoveTowards(otherCharacter.transform.position, otherCharacterOriginalPosition, 5 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoveRobotBack()
    {
        while (Vector3.Distance(robotCharacter.transform.position, robotOriginalPosition) > 0.5f)
        {
            robotCharacter.transform.position = Vector3.MoveTowards(robotCharacter.transform.position, robotOriginalPosition, 5 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoveHatchOn()
    {
        while (Vector3.Distance(robotHatch.transform.position, hatchOriginalPosition) > 0.01f)
        {
            robotHatch.transform.position = Vector3.MoveTowards(robotHatch.transform.position, hatchOriginalPosition, 5 * Time.deltaTime);
            yield return null;
        }
    }

    void StartResolveScoringStage()
    {
        //Compare numeric values that were set to the correct answers
        //If enough are correct, lets say 3/5 for now, then play effects for a correct responce then go to Conversation stage if they have more dialog left
        //If the player does not have any dialog left then go directly to End stage
        //If not enough are correct play effects for incorrect responce and take away chance if the player has any, lets say they have 3 chances for now
        //If the player has no more chances left then go directly to the End stage
        isCorrect = IsFaceCorrectEnough();
        ++conversationIndex;
        if (isCorrect)
        {
            StartCoroutine(CorrectSequence());
        }
        else
        {
            --chances;
            if(chances > 0)
            {
                StartCoroutine(IncorrectSequence());
            }
            else
            {
                ChangeStage(GameStages.EndStage);
            }
        }
    }

    bool IsFaceCorrectEnough()
    {
        Emotions currentEmotion = levelDatas[conversationIndex].emotion;
        switch(currentEmotion)
        {
            case Emotions.Amused:
                {
                    int correctCounter = 0;
                    if(robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if(robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if(robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if(robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if(robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if(robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.SMILE)
                    {
                        ++correctCounter;
                    }

                    if(correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Angry:
                {
                    int correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.FURIOUS)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.FROWN)
                    {
                        ++correctCounter;
                    }

                    if (correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Annoyed:
                {
                    int correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.SLIGHTFROWN)
                    {
                        ++correctCounter;
                    }

                    if (correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Confused:
                {
                    int correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.AJAR)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Disgusted:
                {
                    int correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.PALE)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.FROWN)
                    {
                        ++correctCounter;
                    }

                    if (correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Happy:
                {
                    int correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.SMILE)
                    {
                        ++correctCounter;
                    }

                    if (correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Nervous:
                {
                    int correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.PALE)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Romantic:
                {
                    int correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.SMIRK)
                    {
                        ++correctCounter;
                    }

                    if (correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Sad:
                {
                    int correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.CLOSED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.CLOSED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.FROWN)
                    {
                        ++correctCounter;
                    }

                    if (correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Scared:
                {
                    int correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.PALE)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.FROWN)
                    {
                        ++correctCounter;
                    }

                    if (correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Surprised:
                {
                    int correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.BLUSHING)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.OPEN)
                    {
                        ++correctCounter;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.NEUTRAL)
                    {
                        ++correctCounter;
                    }

                    if (correctCounter >= 4)
                    {
                        return true;
                    }
                    break;
                }
        }
        return false;
    }

    IEnumerator CorrectSequence()
    {
        resultText.gameObject.SetActive(true);
        resultText.text = "Correct!";
        resultText.color = new Color32(20, 106, 20, 255);
        yield return new WaitForSeconds(5);
        resultText.gameObject.SetActive(false);
        ChangeStage(GameStages.ConversationStage);
    }

    IEnumerator IncorrectSequence()
    {
        resultText.gameObject.SetActive(true);
        resultText.text = "Karen is suspicious...";
        resultText.color = new Color32(248, 8, 8, 255);
        yield return new WaitForSeconds(5);
        resultText.gameObject.SetActive(false);
        ChangeStage(GameStages.ConversationStage);
    }

    void UpdateResolveScoringStage()
    {
        //Handles any updates needed for the Resolve Scoring stage
    }

    void StartEndStage()
    {
        if(chances <= 0)
        {
            StartCoroutine(LoseSequence());
        }
        else
        {
            StartCoroutine(WinSequence());
        }
    }

    IEnumerator WinSequence()
    {
        resultText.gameObject.SetActive(true);
        resultText.text = "Success!";
        resultText.color = new Color32(20, 106, 20, 255);
        yield return new WaitForSeconds(5);
        resultText.gameObject.SetActive(false);
        SceneManager.LoadScene("PlayScene");
    }

    IEnumerator LoseSequence()
    {
        resultText.gameObject.SetActive(true);
        resultText.text = "YOUR A ROBOT!!!!!";
        resultText.color = new Color32(248, 8, 8, 255);
        yield return new WaitForSeconds(5);
        resultText.gameObject.SetActive(false);
        SceneManager.LoadScene("PlayScene");
    }

    void UpdateEndStage()
    {
        //Handles any updates needed for the End stage
    }
}
