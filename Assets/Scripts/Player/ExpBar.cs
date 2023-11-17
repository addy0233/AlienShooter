using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public TextMeshProUGUI text;

    public PlayerController playerController;

    public void SetMaxExp(float exp)
    {
        slider.maxValue = exp;
    }

    public void SetExp(float exp)
    {
        slider.value = exp;

        //text.text = playerController.Exp.ToString();
    }
}
