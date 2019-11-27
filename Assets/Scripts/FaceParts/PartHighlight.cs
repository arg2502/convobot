using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartHighlight : MonoBehaviour 
{
    [SerializeField]
    private float highlightStrength = 0.5f;

    [SerializeField]
    private float highlightSpeed = 2f;

    private List<Material> materials;
    private float[] defaultHighlightValues;
    private string propertyName = "_Ambient";

    private bool isHighlighting = false;

	void Start ()
    {
        StoreMaterials();
        StoreDefaultHighlightValues();
	}

    public void StartHighlighting()
    {
        isHighlighting = true;
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
        defaultHighlightValues = new float[materials.Count];

        for (int i = 0; i < materials.Count; i++)
        {
            defaultHighlightValues[i] = materials[i].GetFloat(propertyName);
        }
    }

    private void RestoreDefaultHighlightValues()
    {
        for(int i = 0; i < materials.Count; i++)
        {
            float value = defaultHighlightValues[i];
            materials[i].SetFloat(propertyName, value);
        }
    }

    private void AddToHighlightValues(float value)
    {
        foreach (Material material in materials)
        {
            float currentValue = material.GetFloat(propertyName);
            material.SetFloat(propertyName, currentValue + value);
        }
    }

    private IEnumerator HighlightCoroutine()
    {
        float timer = 0;
        bool increasingHighlight = true;

        while (isHighlighting)
        {
            if(timer >= 1f/highlightSpeed)
            {
                increasingHighlight = !increasingHighlight;
                timer = 0;
            }

            if(increasingHighlight)
            {
                AddToHighlightValues(0.1f * highlightStrength);
            }
            else
            {
                AddToHighlightValues(-0.1f * highlightStrength);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        RestoreDefaultHighlightValues();
    }
}
