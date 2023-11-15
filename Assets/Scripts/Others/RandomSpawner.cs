using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("Time Settings")]
    public float SpawnTime = 10f;
    public float currentSpawnTime;
    public float timeSpeed = 1f;

    [Header("ButtonSpawn")]
    public bool ButtonSpawn = false;

    [Header("Buttons")]
    public KeyCode Bot1SpawnKey = KeyCode.C;
    public KeyCode Bot2SpawnKey = KeyCode.V;

    [Header("Scatter")]
    public bool ScatterOn = false;

    public int MaxScatterValue = 4;
    public int MinScatterValue = 2;

    public Transform firePoint;

    public LayerMask GroundMask;

    public GameObject ObjectToSpawn;
    public GameObject ObjectToSpawn1;
    public GameObject ObjectToSpawn2;

    void Start()
    {
        currentSpawnTime = SpawnTime;
    }

    void Update()
    {
        currentSpawnTime -= Time.deltaTime * timeSpeed;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, Mathf.Infinity, GroundMask))
        {
            Debug.DrawRay(transform.position, firePoint.forward * hit.distance, Color.red);

            if (ButtonSpawn == false)
            {
                if (currentSpawnTime <= 0)
                {
                    currentSpawnTime = SpawnTime;

                    Instantiate(ObjectToSpawn, hit.point, Quaternion.identity);
                }
            }
            else if (ButtonSpawn == true)
            {
                if (Input.GetKey(Bot1SpawnKey) && currentSpawnTime <= 0)
                {
                    currentSpawnTime = SpawnTime;

                    Instantiate(ObjectToSpawn1, hit.point, Quaternion.identity);
                }
                else if (Input.GetKey(Bot2SpawnKey) && currentSpawnTime <= 0)
                {
                    currentSpawnTime = SpawnTime;

                    Instantiate(ObjectToSpawn2, hit.point, Quaternion.identity);
                }
            }
        }

        if (ScatterOn == true)
        {
            //Gun Scatter Values
            int MaxRandomScatterValueY = Random.Range(MaxScatterValue, -MaxScatterValue);
            int MinRandomScatterValueY = Random.Range(MinScatterValue, -MinScatterValue);
            int MaxRandomScatterValueX = Random.Range(MaxScatterValue, -MaxScatterValue);
            int MinRandomScatterValueX = Random.Range(MinScatterValue, -MinScatterValue);

            int CalculatedY = Random.Range(MinRandomScatterValueY, MaxRandomScatterValueY);
            int CalculatedX = Random.Range(MinRandomScatterValueX, MaxRandomScatterValueX);

            firePoint.transform.localEulerAngles = new Vector3(CalculatedX, CalculatedY, 0f);
        }
    }
}
