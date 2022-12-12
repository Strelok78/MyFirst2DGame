using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour
{
    public HeartContainer next;

    [Range(0, 1)] float fill;
    [SerializeField] Image heartImage;

    public void SetHeart(float count)
    {
        fill = count;
        heartImage.fillAmount= fill;
        count--;
        if (next != null) { 
        next.SetHeart(count);
        }
    }
}
