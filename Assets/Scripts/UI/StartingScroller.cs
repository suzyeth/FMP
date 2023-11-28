using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingScroller : MonoBehaviour
{
    [SerializeField] private RawImage img;
    [SerializeField] private float x,y;

    // Update is called once per frame
    void Update()
    {
        img.uvRect = new Rect(img.uvRect.position + new Vector2(0, y) * Time.deltaTime, img.uvRect.size);
    }
}
