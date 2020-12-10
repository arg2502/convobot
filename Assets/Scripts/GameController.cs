using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

public class GameController : MonoBehaviour
{
    private enum GameStages { IntroStage,
                              ConversationStage,
                              MoveToFacialAdjustmentStage,
                              FacialAdjustmentStage,
                              MoveToConversationStage,
                              ResolveScoringStage,
                              EndStage,
                              PauseStage };

    private GameStages currentStage, prevStage;

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
        public GameObject emotionEmoji;
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
    public GameObject correctBanner;
    public GameObject incorrectBanner;
    public GameObject sucessBanner;
    public GameObject failBanner;
    bool isCorrect = false;
    public int chances = 3;
    public List<LevelData> levelDatas;
    public List<GameObject> allTheEmoji;
    public ControlGenerator controlGenerator;
    public ConnectorController connectorController;
    public PlayableDirector timeline;
    public PlayableAsset zoomIn;
    public PlayableAsset zoomOut;
    public static int levelNumber = 1;
    public PauseScreen pauseScreen;
    public bool isPauseDown;
    public string levelCompleteString;
    public string levelPerfectString;

    private PartHighlight[] partHighlightComponenets;

    void Start ()
    {
        levelNumber = GetLevelNumber();
        robotComponents = robotCharacter.GetComponent<Robot>();
        partHighlightComponenets = GameObject.FindObjectsOfType<PartHighlight>();

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
            case GameStages.PauseStage:
            {
                UpdatePauseStage();
                break;
            }
            default:
            {
                break;
            }
        }

        if (!isPauseDown && Input.GetKeyDown(KeyCode.Escape))
        {
            PressPause();            
            isPauseDown = true;
        }
        if (isPauseDown && Input.GetKeyUp(KeyCode.Escape))
            isPauseDown = false;
    }

    void ChangeStage(GameStages stage)
    {
        currentStage = stage;
        StartStage();
    }

    void ResumeStage(GameStages stage)
    {
        currentStage = stage;
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
            case GameStages.PauseStage:
            {
                StartPauseStage();
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

            foreach(GameObject emoji in allTheEmoji)
            {
                emoji.SetActive(false);
            }

            levelDatas[conversationIndex].emotionEmoji.SetActive(true);
        }

        foreach (PartHighlight part in partHighlightComponenets)
        {
            part.StopHighlighting();
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
        AudioManager.instance.StartRobotMusic();
        StartCoroutine(MoveToFacialAdjustment());
    }

    IEnumerator MoveToFacialAdjustment()
    {
        timeline.playableAsset = zoomIn;
        timeline.Play();
        yield return new WaitForSeconds(2.0f);
        //hatchOffPosition.y = robotHatch.transform.position.y;
        //hatchOffPosition.z = robotHatch.transform.position.z;
        //hatchOriginalPosition.y = robotHatch.transform.position.y;
        //hatchOriginalPosition.z = robotHatch.transform.position.z;
        //yield return StartCoroutine(MoveHatchOff());
        controlGenerator.ToggleControls(activate: true);
        connectorController.OpenControls();
        ChangeStage(GameStages.FacialAdjustmentStage);
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
            timerText.text = timeStamp.ToString();
        }
    }

    void StartMoveToConversationStage()
    {
        timerWindow.SetActive(false);
        controlGenerator.ToggleControlsCanMove(false);
        AudioManager.instance.StopRobotMusic();
        StartCoroutine(MoveToConversation());
    }

    void UpdateMoveToConversationStage()
    {
        //Handles any updates needed for the Move To Conversation stage
    }

    IEnumerator MoveToConversation()
    {
        connectorController.CloseControls();
        yield return new WaitForSeconds(1f / connectorController.transitionSpeed);
        controlGenerator.ToggleControls(activate : false);
        timeline.playableAsset = zoomOut;
        timeline.Play();
        yield return new WaitForSeconds(2.0f);
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
        foreach(PartHighlight part in partHighlightComponenets)
        {
            part.StartIncorrectHighlighting();
        }

        //No highlighting for robot skin tone, just doesn't read as intuitively as smaller face parts do
        robotComponents.robotSkinTone.GetComponent<PartHighlight>().StopHighlighting();

        Emotions currentEmotion = levelDatas[conversationIndex].emotion;
        switch(currentEmotion)
        {
            case Emotions.Amused:
                {
                    float correctCounter = 0;
                    if(robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if(robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if(robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if(robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if(robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.OPEN)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if(robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.SMILE)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if(correctCounter >= 3)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Angry:
                {
                    float correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.FURIOUS)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.FROWN)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (correctCounter >= 3)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Annoyed:
                {
                    float correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.SLIGHTFROWN)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (correctCounter >= 3)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Confused:
                {
                    float correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.AJAR)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.NEUTRAL)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (correctCounter >= 3)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Disgusted:
                {
                    float correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.LOWERED)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.PALE)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.OPEN)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.FROWN)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (correctCounter >= 3)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Happy:
                {
                    float correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.SMILE)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (correctCounter >= 3)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Nervous:
                {
                    float correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.PALE)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.NEUTRAL)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (correctCounter >= 3)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Romantic:
                {
                    float correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.SQUINTING)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.SMIRK)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (correctCounter >= 3)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Sad:
                {
                    float correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.NEUTRAL)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.CLOSED)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.CLOSED)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.NEUTRAL)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.CLOSED)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.FROWN)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (correctCounter >= 3)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Scared:
                {
                    float correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.PALE)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.OPEN)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.FROWN)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (correctCounter >= 3)
                    {
                        return true;
                    }
                    break;
                }
            case Emotions.Surprised:
                {
                    float correctCounter = 0;
                    if (robotComponents.robotLeftEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        robotComponents.robotLeftEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyebrow.eyebrowState == Eyebrow.EyebrowState.RAISED)
                    {
                        robotComponents.robotRightEyebrow.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotRightEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        robotComponents.robotRightEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotLeftEyelids.eyelidState == Eyelids.EyelidState.OPEN)
                    {
                        robotComponents.robotLeftEyelids.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 1.0f;
                    }

                    if (robotComponents.robotSkinTone.skinState == SkinTone.SkinState.BLUSHING)
                    {
                        //robotComponents.robotSkinTone.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 0.5f;
                    }

                    if (robotComponents.robotMouthOpen.mouthOpenState == MouthOpen.MouthOpenState.OPEN)
                    {
                        robotComponents.robotMouthOpen.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (robotComponents.robotMouthSmile.mouthSmileState == MouthSmile.MouthSmileState.NEUTRAL)
                    {
                        robotComponents.robotMouthSmile.GetComponent<PartHighlight>().StartCorrectHighlighting();
                        correctCounter += 2.0f;
                    }

                    if (correctCounter >= 3)
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
        correctBanner.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        correctBanner.gameObject.SetActive(false);
        ChangeStage(GameStages.ConversationStage);
    }

    IEnumerator IncorrectSequence()
    {
        incorrectBanner.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        incorrectBanner.gameObject.SetActive(false);
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
        sucessBanner.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        SetLevelWin(true);
        sucessBanner.gameObject.SetActive(false);
        SceneManager.LoadScene("PlayScene");
    }

    IEnumerator LoseSequence()
    {
        failBanner.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        SetLevelWin(false);
        failBanner.gameObject.SetActive(false);
        SceneManager.LoadScene("PlayScene");
    }

    void SetLevelWin(bool didWin)
    {
        int didYouWin = (didWin) ? 1 : 0;

        switch(levelNumber)
        {
            case 1:
            {
                if(PlayerPrefs.HasKey(levelCompleteString))
                {
                    PlayerPrefs.SetInt(levelCompleteString, didYouWin);
                }

                if (chances >= 3)
                {
                    if (PlayerPrefs.HasKey(levelPerfectString))
                    {
                        PlayerPrefs.SetInt(levelPerfectString, 1);
                    }
                }
                break;
            }
        }
    }

    void UpdateEndStage()
    {
        //Handles any updates needed for the End stage
    }

    void PressPause()
    {
        if(currentStage != GameStages.PauseStage)
        {
            prevStage = currentStage;
            ChangeStage(GameStages.PauseStage);
        }
        else
        {
            pauseScreen.CloseScreen();
            controlGenerator.PauseControls(false);
            ResumeStage(prevStage);
        }

    }

    void StartPauseStage()
    {        
        controlGenerator.ToggleControlsCanMove(false);
        controlGenerator.PauseControls(true);
        pauseScreen.OpenScreen();
    }

    void UpdatePauseStage()
    {
        
    }

    private int GetLevelNumber()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "TutorialScene": return 0;
            case "GameScene": return 1;
            case "InterviewScene": return 2;
            case "InterogationScene": return 3;
            case "Credits": return 999;
            default: return 1;
        }
    }
}
