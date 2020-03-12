using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shift hue of an image
/// </summary>
public class HueShifter : MonoBehaviour
{
    public float Speed = 1;
    private Image rend;

    void Start()
    {
        rend = GetComponent<Image>();
    }

    void Update()
    {
        rend.material.SetColor("_Color", HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * Speed, 1), 1, 1)));
    }
}

