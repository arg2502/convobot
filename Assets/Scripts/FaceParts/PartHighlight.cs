using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartHighlight : MonoBehaviour 
{
    [SerializeField]
    private float highlightStrength = 0.5f;

    [SerializeField]
    private float highlightSpeed = 0.2f;

    [SerializeField]
    private Color correctColor = Color.white;

    [SerializeField]
    private Color incorrectColor = Color.red;

    private List<Material> materials;
    private Color[] defaultHighlightValues;

    private bool isHighlighting = false;
    private Color highlightColor = Color.black;

	void Start ()
    {
        StoreMaterials();
        StoreDefaultHighlightValues();
	}

    public void StartCorrectHighlighting()
    {
        highlightColor = correctColor;
        StartCoroutine(HighlightCoroutine());
    }

    public void StartIncorrectHighlighting()
    {
        highlightColor = incorrectColor;
        StartCoroutine(HighlightCoroutine());
    }

    public void StopHighlighting()
    {
        isHighlighting = false;
    }

    private void StoreMaterials()
    {
        materials = new List<Material>();
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();        

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            Material[] thisMRMaterials = meshRenderers[i].materials;

            for (int j = 0; j < thisMRMaterials.Length; j++)
            {
                materials.Add(thisMRMaterials[j]);
            }
        }

        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            Material[] thisSMRMaterials = skinnedMeshRenderers[i].materials;

            for (int j = 0; j < thisSMRMaterials.Length; j++)
            {
                materials.Add(thisSMRMaterials[j]);
            }
        }
    }

    private void StoreDefaultHighlightValues()
    {
        defaultHighlightValues = new Color[materials.Count];

        for (int i = 0; i < materials.Count; i++)
        {
            defaultHighlightValues[i] = materials[i].color;
        }
    }

    private void RestoreDefaultHighlightValues()
    {
        for(int i = 0; i < materials.Count; i++)
        {
            Color value = defaultHighlightValues[i];
            materials[i].color = value;
        }
    }

    private IEnumerator HighlightCoroutine()
    {
        isHighlighting = true;

        while (isHighlighting)
        {
            float t = Mathf.PingPong(Time.time * highlightSpeed, highlightStrength);
            UpdateHighlights(t);
            yield return null;
        }

        RestoreDefaultHighlightValues();
    }

    private void UpdateHighlights(float lerpStep)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            Color defaultValue = defaultHighlightValues[i];
            materials[i].color = Color.Lerp(defaultValue, highlightColor, lerpStep);
        }
    }
}
