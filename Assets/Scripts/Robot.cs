using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public Eyebrow robotRightEyebrow;
    public Eyelids robotRightEyelids;
    public Eyebrow robotLeftEyebrow;
    public Eyelids robotLeftEyelids;
    public MouthOpen robotMouthOpen;
    public MouthSmile robotMouthSmile;
    public SkinTone robotSkinTone;

    [System.NonSerialized]
    public List<FacePart> faceParts;

    // get placeholder positions within each robot that can be easily altered in the editor
    public GameObject controlsParent;
    public GameObject placeholderParent;
    List<Transform> placeholderPositions;
    public List<Transform> PlaceholderPositions { get { return SetPlaceholders(); } }
    

	void Start ()
    {
        faceParts = new List<FacePart>() { robotLeftEyebrow, robotRightEyebrow, robotLeftEyelids, robotRightEyelids,
        robotMouthOpen, robotMouthSmile, robotSkinTone};
        SetPlaceholders();
	}
	
    List<Transform> SetPlaceholders()
    {
        if (placeholderPositions == null)
        {            
            // get components in children also gets the parent
            // this way ensures that the parent's transform is removed from the list
            var list = new HashSet<Transform>(placeholderParent.GetComponentsInChildren<Transform>());
            list.Remove(placeholderParent.transform);
            placeholderPositions = list.ToList();
        }

        return placeholderPositions;
    }

    /// <summary>
    /// When we don't need the placeholder position objects anymore, disable them.
    /// </summary>
    public void DisablePlaceholders()
    {
        foreach (var p in placeholderPositions)
            p.gameObject.SetActive(false);
    }
}
