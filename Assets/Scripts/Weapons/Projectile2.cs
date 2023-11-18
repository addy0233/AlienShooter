using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float Damage = 25f;
    public float speed = 100f;

    [Header("Poison Bullet")]
    public bool poisonBullet = false;
    public float poisonBulletsLevel = 0f;
    public float PoisonTime = 3f;
    public float PoisonRate = 0.2f;
    public float PoisonDamage = 20f;
    [Space]
    public bool DrawGizmo = true;

    private bool hasExploded = false;

    [Header("Time Settings")]
    public float TimeSpeed = 1;
    public float TimeToDestroy = 2;

    [Header("PopUpDamage")]
    public bool PopUpDamageOn = false;
    public PopUp1 popup1;
    public float popupScale = 1f;

    [Header("Collisions")]
    public float CollisionRadius = 0.4f;
    public LayerMask collisionMask;

    [Header("Piercing Bullets")]
    public bool piercingBoolet = false;

    [Header("Particles")]
    public bool SpawnParticle = false;
    public GameObject DestroyParticle;
    public float ParticleScale1 = 2f;
    public float ParticleScale2 = 6f;

    [Header("Assing Fields")]
    public Rigidbody rb;
    public Transform firePoint;

    Transform player;

    void Start()
    {
        hasExploded = false;

        player = PlayerManager.instance.player.transform;
    }


    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        TimeToDestroy -= Time.deltaTime * TimeSpeed;

        if (TimeToDestroy <= 0)
        {
            BulletDeath();
        }

        Collider[] PlayerToDamage = Physics.OverlapSphere(transform.position, CollisionRadius, collisionMask);
        for (int i = 0; i < PlayerToDamage.Length; i++)
        {
            EnemyHealth1 target1 = PlayerToDamage[i].transform.GetComponent<EnemyHealth1>();

            if (target1 != null)
            {
                target1.TakeDamage(Damage);

                if (PopUpDamageOn == true)
                {
                    PopUp1 PopUp = Instantiate(popup1, transform.position, transform.rotation) as PopUp1;
                    PopUp.transform.localScale = new Vector3(popupScale, popupScale, popupScale);
                    PopUp.isText = true;
                    PopUp.damage = Damage;
                }
            }

            BulletDeath();
        }
    }

    void BulletDeath()
    {
        if (SpawnParticle == true)
        {
            if (hasExploded != true)
            {
                GameObject particle = Instantiate(DestroyParticle, firePoint.position, firePoint.rotation);
                Destroy(particle, 6f);
                particle.transform.localScale = new Vector3(ParticleScale1, ParticleScale1, ParticleScale1);
            }
        }

        hasExploded = true;

        Destroy(gameObject);
    }


}
