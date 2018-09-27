using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGenerator : MonoBehaviour {

    GameObject[] controls;
    public Robot currentRobot;
    List<Control> currentControls;

    private void Start()
    {
        controls = Resources.LoadAll<GameObject>("ControlObjects");
        currentControls = new List<Control>();
        
        CreateControls();
    }

    void CreateControls()
    {
        for(int i = 0; i < currentRobot.PlaceholderPositions.Length; i++)
        {
            var controlObj = GameObject.Instantiate(controls[i], currentRobot.controlsParent.transform);
            controlObj.transform.position = currentRobot.PlaceholderPositions[i].transform.position;

            var control = controlObj.GetComponentInChildren<Control>();
            currentControls.Add(control);
        }
        currentRobot.DisablePlaceholders();
        AssignControls();
    }

    void AssignControls()
    {
        // local list that we can manipulate
        var list = new List<FacePart>(currentRobot.faceParts);

        foreach(var c in currentControls)
        {
            // pick a random face part
            var rnd = Random.Range(0, list.Count);
            var fp = list[rnd];            

            c.AssignControl(fp);

            // remove that facepart from our local list to ensure that each face part is given a control
            list.Remove(fp);

            // check to see if the list is empty, if it is, repopulate
            if (list.Count <= 0)
            {
                list = new List<FacePart>(currentRobot.faceParts);
            }
        }
    }
}
