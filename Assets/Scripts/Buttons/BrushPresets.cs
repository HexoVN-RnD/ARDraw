using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushPresets : MonoBehaviour
{
    [SerializeField] private GameObject brushPresetsPanel;
    [SerializeField] private GameObject brushPresetButtonPrefab;
    [SerializeField] private Material[] brushMaterials;
    [SerializeField] private Material defaultBrushMaterial;
    [SerializeField] private LineSettings lineSettings;

    //get the button component of brush presets, on click set the brush material to the material of the button
    private void Start()
    {
        for (int i = 0; i < brushMaterials.Length; i++)
        {
            int index = i;
            GameObject brushPresetButton = Instantiate(brushPresetButtonPrefab, brushPresetsPanel.transform);

            // Get the main texture from the material
            Texture2D texture2D = brushMaterials[index].mainTexture as Texture2D;

            if (texture2D != null)
            {
                // Create a sprite from the texture
                Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                // Set the sprite of the Image component
                brushPresetButton.GetComponent<Image>().sprite = sprite;

                brushPresetButton.GetComponent<Button>().onClick.AddListener(() => SetBrushMaterial(index));
            }
            else
            {
                Debug.Log("Texture is null for material at index: " + index);
            }
        }
    }

    private void SetBrushMaterial(int index)
    {
        lineSettings.lineMaterial = brushMaterials[index];
        lineSettings.SaveMaterial();
    }

    public void SetDefaultBrushMaterial()
    {
        lineSettings.lineMaterial = defaultBrushMaterial;
        lineSettings.SaveMaterial();
    }
}
