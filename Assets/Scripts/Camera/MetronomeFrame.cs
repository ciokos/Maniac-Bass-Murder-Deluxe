using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MetronomeFrame : MonoBehaviour
{

    private Conductor conductor;
    private GameObject[] images;
    private Color color1; 
    private Color color2;
    private bool color = true;
    // Start is called before the first frame update
    void Start()
    {
        conductor = (Conductor)GameObject.FindObjectOfType<Conductor>();
        conductor.Beat.AddListener(onBeat);
        images = GameObject.FindGameObjectsWithTag("Frame");
        color1 = new Color(0.280226f, 0.1004449f, 0.9339623f, 1f);
        color2 = new Color(0.7830189f, 0.1433072f, 0.2682463f, 1f);
    }

    private void onBeat()
    {
        foreach (GameObject i in images)
        {
            if (color)
                i.GetComponent<Image>().color = color2;
            else
                i.GetComponent<Image>().color = color1;
        }
        color = !color;
    }

}