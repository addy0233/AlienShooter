using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void SetMaxAmmo(float ammo)
    {
        slider.maxValue = ammo;
        slider.value = ammo;
    }

    public void SetAmmo(float ammo)
    {
        slider.value = ammo;
    }
}
