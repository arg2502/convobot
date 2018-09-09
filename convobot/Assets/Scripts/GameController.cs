﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //We will probably need as variables:

    //The robot character game object
    //The other person game object
    //A list of lists of strings for dialog
    //The index of dialog we are on currently
    //The original position of the robot
    //The original position of the other person
    //The zoomed in position of the robot
    //The offscreen position for the other person
    //A counter for the number of chances the player has before discovery
    //More stuff I can't think of at 2AM

    void Start ()
    {
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
    }

    void UpdateIntroStage()
    {
        //Handles any updates needed for the intro stage
    }

    void StartConversationStage()
    {
        //Go through and display any speech bubbles that are needed to to get to the point in the conversation where we need to adjust the facial expression
        //Either have the current bubble disappear on click or after a certain amount of time
        //Assume that we will have some sort of list of a list of strings that we will pull from to supply the conversation for now
        //Once we are at the end of the current list of dialog move to the Move To Facial Adjustment stage
    }

    void UpdateConversationStage()
    {
        //Handles any updates needed for the Conversation stage
    }

    void StartMoveToFacialAdjustmentStage()
    {
        //Move the other person off screen to the left
        //Center the robot and move him closer to the camera
        //Move the hatch off of the face and expose the facial adjustment inputs
        //None of those components should be interactable during this stage
        //Once all the movement is complete move on to the Facial Adjustment stage
    }

    void UpdateMoveToFacialAdjustmentStage()
    {
        //Handles any updates needed for the Move To Facial Adjustment stage
    }

    void StartFacialAdjustmentStage()
    {
        //Mark time that we started this stage
        //All adjustment components are now interactable
        //Have the compontents themselves adjust a set of numeric values so that we can compare them to the correct answers later
        //If the player hits the done button then go to the Move To Conversation stage
    }

    void UpdateFacialAdjustmentStage()
    {
        //Check to see if we have gotten to the end of the timer
        //If we have not then adjust a visable timer
        //If we have gotten to the end of the timer go directly to the Move To Conversation stage
    }

    void StartMoveToConversationStage()
    {
        //Move hatch back onto robot's face
        //Move robot back to original position
        //Move other person back to original position
        //Once all that is done then go to Resolve Scoring stage
    }

    void UpdateMoveToConversationStage()
    {
        //Handles any updates needed for the Move To Conversation stage
    }

    void StartResolveScoringStage()
    {
        //Compare numeric values that were set to the correct answers
        //If enough are correct, lets say 3/5 for now, then play effects for a correct responce then go to Conversation stage if they have more dialog left
        //If the player does not have any dialog left then go directly to End stage
        //If not enough are correct play effects for incorrect responce and take away chance if the player has any, lets say they have 3 chances for now
        //If the player has no more chances left then go directly to the End stage
    }

    void UpdateResolveScoringStage()
    {
        //Handles any updates needed for the Resolve Scoring stage
    }

    void StartEndStage()
    {
        //If the player still has chances then the conversation was a success so play congradulations fanfare
        //If the player does not have any chances left then they failed and have been found out as a robot, play lose fanfare
        //Go back to start screen once this is all done
    }

    void UpdateEndStage()
    {
        //Handles any updates needed for the End stage
    }
}
