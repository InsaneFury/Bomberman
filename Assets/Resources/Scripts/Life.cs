using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    Image image;
    Color originalColor;
    public bool LifeEnabled = true;

    void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    private void Update()
    {
        SetLifeColor();
    }

    void SetLifeColor()
    {
        if (!LifeEnabled)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        else
        {
            image.color = originalColor;
        }   
    }
}
