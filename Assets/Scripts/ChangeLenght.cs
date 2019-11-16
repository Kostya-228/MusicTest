using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLenght : MonoBehaviour
{
    public Transform[] lines;
    public Slider slider;
    Transform[] dropZones;
    float lastValue = 0.25f;
    float y = 0;

    private void Start()
    {
        dropZones = new Transform[lines.Length];
        for (int i = 0; i < lines.Length; i++)
            dropZones[i] = lines[i].GetChild(0);
        y = dropZones[0].localScale.y;
    }

    void Update()
    {
        if (slider.value != lastValue)
        {
            lastValue = slider.value;
            foreach(Transform tr in dropZones)
            {
                tr.localScale = new Vector3(2,y, 1 + 8 * slider.value);
            }
        }
    }
}
