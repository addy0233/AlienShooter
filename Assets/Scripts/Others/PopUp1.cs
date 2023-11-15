using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUp1 : MonoBehaviour
{
    [Tooltip("Leave at false to spawn Icon")]
    public bool isText = false;

    public float lifeTime = 1f;
    public float currentLifeTime;
    public float TimeSpeed = 1f;

    public float damage;
    public TextMeshProUGUI text;

    public GameObject text1;
    public GameObject Icon1;

    void Start()
    {
        currentLifeTime = lifeTime;
    }

    void Update()
    {
        currentLifeTime -= Time.deltaTime * lifeTime;

        if (isText == true)
        {
            text1.SetActive(true);
            Icon1.SetActive(false);

            text.text = damage.ToString();
        }
        else if (isText == false)
        {
            text1.SetActive(false);
            Icon1.SetActive(true);
        }

        if (currentLifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
