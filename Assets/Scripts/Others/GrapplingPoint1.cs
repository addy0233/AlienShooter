using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingPoint1 : MonoBehaviour
{
    public float timeToShow;
    public float currentTimeToShow;

    public Transform GFX;

    void Start()
    {
        GFX.gameObject.SetActive(false);

        currentTimeToShow = 0f;
    }

    void Update()
    {
        currentTimeToShow -= Time.deltaTime * 1f;

        if (currentTimeToShow >= 0)
        {
            GFX.gameObject.SetActive(true);
        }
        else if (currentTimeToShow <= 0)
        {
            GFX.gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        currentTimeToShow = timeToShow;
    }
}
