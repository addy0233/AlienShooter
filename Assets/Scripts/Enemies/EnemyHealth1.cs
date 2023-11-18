using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth1 : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Enemy Timer")]
    public float lifeTime = 100f;
    public float currentLifeTime;

    [Header("Object to Destroy")]
    public GameObject ObjectToDestroy;

    [Header("Time Settings")]
    public float TimeSpeed = 1f;

    [Header("Health Bar")]
    public bool HealthBarOn = false;
    public bool AlwaysShowHealthBar = true;
    private bool HealthBarShow = false;
    public float HealthBarShowTime = 16f;
    private float CurrentHealthBarShowTime;
    public EnemyHealthBar1 healthBar;
    public GameObject HealthBar;
    
    [Header("Exp")]
    public float GoldCount = 12f;
    private int currentGoldCount;
    public GameObject Exp;
    public Transform GoldFirePoint;
    
    [Header("Death Particle")]
    public bool SpawnDeathParticle = false;
    public float DeathParticleScale1;
    public float DeathParticleScale2;
    public Transform DeathParticleOrigin;
    public GameObject DeathParticle;

    [Header("Assing Fields")]
    public Enemy1 EnemyController;

    void Start()
    {
        currentHealth = maxHealth;
        
        if (HealthBarOn == true)
        {
            healthBar.SetMaxHealth(maxHealth);
        }

        if (AlwaysShowHealthBar == false)
        {
            CurrentHealthBarShowTime = HealthBarShowTime;

            HealthBar.SetActive(false);

            HealthBarShow = false;
        }

        //currentGoldCount = 0;
        
        currentLifeTime = lifeTime;
    }

    void Update()
    {
        currentLifeTime -= Time.deltaTime * TimeSpeed;

        if (currentLifeTime <= 0)
        {
            if (SpawnDeathParticle == true)
            {
                GameObject DeathParticle1 = Instantiate(DeathParticle, DeathParticleOrigin.position, DeathParticleOrigin.rotation);
                DeathParticle1.transform.localScale = new Vector3(DeathParticleScale2, DeathParticleScale2, DeathParticleScale2);
                Destroy(DeathParticle1, 4f);
            }

            Destroy(ObjectToDestroy, 0.08f);
        }

        if (currentHealth <= 0)
        {
            Death();
        }
        
        if (HealthBarOn == true)
        {
            if (AlwaysShowHealthBar == false)
            {
                if (HealthBarShow == true)
                {
                    CurrentHealthBarShowTime -= Time.deltaTime * TimeSpeed;

                    HealthBar.SetActive(true);

                    if (CurrentHealthBarShowTime <= 0)
                    {
                        HealthBar.SetActive(false);

                        CurrentHealthBarShowTime = HealthBarShowTime;

                        HealthBarShow = false;
                    }
                }
            }
        }
        
    }

    public void TakeDamage(float damage) //float ExplosionDamage
    {
        currentHealth -= damage;
        
        if (HealthBarOn == true)
        {
            healthBar.SetHealth(currentHealth);
        }

        HealthBarShow = true;
    }
    /*
    void GoldSpawn()
    {
        //Gold1 gold = Instantiate(gold1, GoldFirePoint.position, GoldFirePoint.rotation) as Gold1;

        currentGoldCount += 1;

        if (currentGoldCount > GoldCount)
        {
            CancelInvoke("GoldSpawn");
        }
    }
    */
    void Death()
    {
        if (SpawnDeathParticle == true)
        {
            GameObject DeathParticle1 = Instantiate(DeathParticle, DeathParticleOrigin.position, DeathParticleOrigin.rotation);
            DeathParticle1.transform.localScale = new Vector3(DeathParticleScale1, DeathParticleScale1, DeathParticleScale1);
            Destroy(DeathParticle1, 4f);
        }

        for (currentGoldCount = 0; currentGoldCount < GoldCount; currentGoldCount++)
        {
            GameObject exp1 = Instantiate(Exp, GoldFirePoint.transform.position, GoldFirePoint.transform.rotation) as GameObject;
            Destroy(exp1, 20f);
        }

        /*
        if (currentGoldCount < GoldCount)
        {
            InvokeRepeating("GoldSpawn", 0f, 0.002f);
        }
        */
        Destroy(ObjectToDestroy);
    }
}
