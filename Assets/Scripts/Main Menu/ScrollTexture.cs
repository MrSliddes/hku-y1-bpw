using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollTexture : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float vOffset = Time.deltaTime * scrollSpeed;
        image.material.SetTextureOffset("_MainTex", new Vector2(vOffset, vOffset));
    }
}
