using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile1 : MonoBehaviour
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

        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);

        TimeToDestroy -= Time.deltaTime * TimeSpeed;

        if (TimeToDestroy <= 0)
        {
            BulletDeath();
        }

        Collider[] PlayerToDamage = Physics.OverlapSphere(transform.position, CollisionRadius, collisionMask);
        for (int i = 0; i < PlayerToDamage.Length; i++)
        {
            EnemyHealth1 target1 = PlayerToDamage[i].GetComponent<EnemyHealth1>();

            if (target1 != null)
            {
                target1.TakeDamage(Damage);
            }

            BulletDeath();
        }
    }

    void CheckCollisions(float moveDistnce)
    {
        if (hasExploded == false)
        {
            RaycastHit ray;
            if (Physics.SphereCast(transform.position, CollisionRadius, transform.forward, out ray, moveDistnce, collisionMask, QueryTriggerInteraction.Collide)) //hit target
            {
                OnHitObject(ray);

                //Setup Moving Platform : 1. Set tags to Moving 2. Set layers to Ground

            }
        }
    }

    void OnHitObject(RaycastHit ray) //doing this after hitting target
    {
        //PlayerHealth1 target1 = ray.transform.GetComponent<PlayerHealth1>();
        //Chest1 target2 = ray.transform.GetComponent<Chest1>();
        EnemyHealth1 target3 = ray.transform.GetComponent<EnemyHealth1>();
        //Turret2Health1 target4 = ray.transform.GetComponent<Turret2Health1>();
        //TNT1 target5 = ray.transform.GetComponent<TNT1>();
        /*
        if (target1 != null)
        {
            if (piercingBoolet == false)
            {
                Explode();

                target1.TakeDamage(Damage);
            }
        }
        else if (target5 != null)
        {
            Explode();

            target5.Explosion2();
        }
        else if (target4 != null)
        {
            if (piercingBoolet == false)
            {
                Explode();
            }

            target4.TakeDamage(Damage);
        }
        else
        {
            BulletDeath();
        }
        */

        if (target3 != null)
        {
            target3.TakeDamage(Damage);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Map")
        {
            BulletDeath();
        }
    }

    void Explode()
    {
        if (SpawnParticle == true)
        {
            if (hasExploded != true)
            {
                GameObject particle = Instantiate(DestroyParticle, firePoint.position, firePoint.rotation);
                Destroy(particle, 6f);
                particle.transform.localScale = new Vector3(ParticleScale2, ParticleScale2, ParticleScale2);
            }
        }

        hasExploded = true;

        Destroy(gameObject);
    }

    void BulletDeath()
    {
        if (SpawnParticle == true)
        {
            if (hasExploded != true)
            {
                GameObject particle = Instantiate(DestroyParticle, firePoint.position,  firePoint.rotation);
                Destroy(particle, 6f);
                particle.transform.localScale = new Vector3(ParticleScale1, ParticleScale1, ParticleScale1);
            }
        }

        hasExploded = true;

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (DrawGizmo == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, CollisionRadius);
        }
    }
}
