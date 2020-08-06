using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScript : MonoBehaviour
{
    [SerializeField]
    private Material Stripes;

    private Color c;

    private float r;

    private float g;

    private float b;
    
    [SerializeField]
    private float emissionStrength;

    private void Start()
    {

        c = Color.red;
        InvokeRepeating("ShiftHue", 0, .01f);
    }

    private void ShiftHue()
    {
        Debug.Log("Shifting Hue");
        r = Random.Range(-.4f, .4f);
        g = Random.Range(-.4f, .4f);
        b = Random.Range(-.4f, .4f);
        c = new Color(c.r + r, c.g + g, c.b + b);

        if (c.r == 0 || c.g == 0 || c.b == 0)
        {
            c = new Color(c.r + .4f, c.g + .4f, c.b + .4f);
        }
        c = c * Mathf.LinearToGammaSpace(emissionStrength);
        Stripes.SetColor("_EmissionColor", c);
        Debug.Log(c);
    }
}
