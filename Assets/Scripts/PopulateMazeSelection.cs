using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulateMazeSelection : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TextAsset[] assets;

    public static TextAsset selectedAsset;
    private static int selectedAssetIndex;


    private void Awake()
    {
        if (selectedAsset == null)
        {
            selectedAssetIndex = 0;
            selectedAsset = assets[selectedAssetIndex];
            Debug.Log("Asset null. Setting to first.");
        }
    }

    private void OnEnable()
    {
        List<string> mazeNames = new List<string>(assets.Length);

        foreach (TextAsset asset in assets)
        {
            mazeNames.Add(asset.name);
        }
        dropdown.AddOptions(mazeNames);
        dropdown.value = selectedAssetIndex;
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(DropdownSelectionCallback);
    }

    private void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(DropdownSelectionCallback);
        dropdown.ClearOptions();
    }

    public void DropdownSelectionCallback(int i)
    {
        selectedAssetIndex = i;
        selectedAsset = assets[i];
    }
}
