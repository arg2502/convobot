using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGenerator : MonoBehaviour {

    GameObject[] controls;
    public Robot currentRobot;
    public float controlsScaleAdjust;
    List<GameObject> currentControlsObj;

    List<Control> currentSwitches;
    List<Control> currentRest;

    private void Start()
    {
        controls = Resources.LoadAll<GameObject>("ControlObjects");
        currentControlsObj = new List<GameObject>();
        currentSwitches = new List<Control>();
        currentRest = new List<Control>();

        CreateControls();
    }

    void ShuffleArray<T>(List<T> array)
    {
        // Knuth (Fisher-Yates) shuffle algorithm
        for(int i = 0; i < array.Count; i++)
        {
            var tmp = array[i];
            var rnd = Random.Range(i, array.Count);
            array[i] = array[rnd];
            array[rnd] = tmp;
        }
    }
    
    void ShuffleArray<T>(T[] array)
    {
        // Knuth (Fisher-Yates) shuffle algorithm
        for (int i = 0; i < array.Length; i++)
        {
            var tmp = array[i];
            var rnd = Random.Range(i, array.Length);
            array[i] = array[rnd];
            array[rnd] = tmp;
        }
    }

    void CreateControls()
    {
        // shuffle controls so it's a random assortment everytime
        ShuffleArray(controls);

        // find out the max num of switches we can have
        int maxSwitches = 0;
        for(int i = 0; i < currentRobot.faceParts.Count; i++)
        {
            if (currentRobot.faceParts[i].NumOfStates == 3)
                maxSwitches++;
        }

        // Now that we have the max number of switches, let's create 2 separate lists for controls:
        // One for strictly switches; One for all the rest.
        // We'll also remove any excess switches so that we'll only have the amount that we need.
        var switchList = new List<GameObject>();
        var restList = new List<GameObject>();
        for(int i = 0; i < currentRobot.faceParts.Count; i++)
        {
            if(controls[i].GetComponent<Switch>() && switchList.Count < maxSwitches)
            {
                switchList.Add(controls[i]);
            }
            else
            { 
                restList.Add(controls[i]);
            }
        }

        // randomize placeholder position order
        ShuffleArray(currentRobot.PlaceholderPositions);

        // Now that we know exactly how many switches we need, we'll create those first
        // Let's pick random placeholderPositions, create the switches there, 
        // then remove those positions as options when we go to create the rest of the controls
        foreach(var s in switchList)
        {
            // create obj
            var controlObj = GameObject.Instantiate(s);
            controlObj.transform.parent = currentRobot.controlsParent.transform;
            currentControlsObj.Add(controlObj);

            //adjust obj scale
            controlObj.transform.localScale *= controlsScaleAdjust;

            //adjust obj rotation
            controlObj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            // find random pos and place it there
            var rnd = Random.Range(0, currentRobot.PlaceholderPositions.Count);
            controlObj.transform.position = currentRobot.PlaceholderPositions[rnd].transform.position;

            // store object reference in connector at this position
            AssignConnector(controlObj, currentRobot.PlaceholderPositions[rnd].gameObject);

            // add control to current list
            var control = controlObj.GetComponentInChildren<Control>();
            currentSwitches.Add(control);

            // disable object and remove that position from consideration of other placements
            currentRobot.PlaceholderPositions[rnd].gameObject.SetActive(false);
            currentRobot.PlaceholderPositions.RemoveAt(rnd);
        }

        // now loop throught the rest of the placeholder positions and create the rest of the controls   
        for (int i = 0; i < currentRobot.PlaceholderPositions.Count; i++)
        {
            var controlObj = GameObject.Instantiate(restList[i]);
            controlObj.transform.parent = currentRobot.controlsParent.transform;
            currentControlsObj.Add(controlObj);
            controlObj.transform.position = currentRobot.PlaceholderPositions[i].transform.position;

            //adjust obj scale
            controlObj.transform.localScale *= controlsScaleAdjust;

            //adjust obj rotation
            controlObj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            // store object reference in connector at this position
            AssignConnector(controlObj, currentRobot.PlaceholderPositions[i].gameObject);

            var control = controlObj.GetComponentInChildren<Control>();
            currentRest.Add(control);
        }
        currentRobot.DisablePlaceholders();
        AssignControls();
    }

    void AssignControls()
    {        
        // loop through faceparts and assign controls appropriately
        // if we come in contact with a 3-state facepart, check if we have any switches left to assign,
        // otherwise, any control will do
        foreach(var fp in currentRobot.faceParts)
        {
            int rnd = 0;
            if(fp.NumOfStates == 3 && currentSwitches.Count > 0)
            {
                // get random switch
                rnd = Random.Range(0, currentSwitches.Count);
                currentSwitches[rnd].GetComponent<Control>().AssignControl(fp);

                currentSwitches.RemoveAt(rnd);
                continue;            
            }

            rnd = Random.Range(0, currentRest.Count);
            
            currentRest[rnd].GetComponentInChildren<Control>().AssignControl(fp);

            currentRest.RemoveAt(rnd);
        }

        ToggleControls(false);
    }

    void AssignConnector(GameObject controlObject, GameObject placeholderObject)
    {
        string positionName = placeholderObject.name;
        string positionNumber =  positionName.Substring(positionName.Length-1, 1);

        int positionInt = int.Parse(positionNumber);
            
        currentRobot.connectorController.StoreControlObject(controlObject, positionInt-1);
    }

    // temporary function
    public void ToggleControls(bool activate)
    {
        foreach (var control in currentControlsObj)
        {
            control.gameObject.SetActive(activate);
        }
    }

    public void ToggleControlsCanMove(bool _canMove)
    {
        foreach(var control in currentControlsObj)
        {
            control.GetComponentInChildren<Control>().ToggleCanMove(_canMove);
        }
    }
}
