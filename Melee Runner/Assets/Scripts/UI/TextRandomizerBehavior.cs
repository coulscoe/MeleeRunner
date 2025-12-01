using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextRandomizerBehavior : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI textGui;
    
    [TextArea]
    [SerializeField] private List<String> textList = new List<string>();

    
    public void RandomizeText()
    {
        int index = UnityEngine.Random.Range(0, textList.Count);
        textGui.text = textList[index];
    }

}
