using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeInPanel : MonoBehaviour
{
    public Image img;
    float alpha = 0;
    void FixedUpdate()
    {
        Color color = img.color;
        alpha += 0.008f;
        color.a = alpha;
        img.color = color;
    }
}
