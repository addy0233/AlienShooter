using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP1 : MonoBehaviour
{
    public float maxForce = 0.08f;
    public float minForce = 0.05f;

    public float DetectRange = 1f;
    public float speed = 2f;

    [Header("Player Detection")]
    public float collisionRange = 0.4f;
    public GameObject DestroyParticle;

    private bool ImpulseUp = false;

    public float maxRandomX = 360f;
    public float minRandomX = -360f;
    public float maxRandomY = 360f;
    public float minRandomY = -360f;
    public Vector3 Direction1;
    public Vector3 Direction2;

    bool goToPlayer = false;

    Transform target;

    public Rigidbody rb;

    void Start()
    {
        goToPlayer = false;
    }

    void Update()
    {
        target = PlayerManager.instance.player.transform;

        if (ImpulseUp == false)
        {
            Invoke("Impulse", 0f);

            ImpulseUp = true;
        }

        float distanceToTarget = Vector3.Distance(target.gameObject.transform.position, transform.position);

        if (distanceToTarget <= DetectRange)
        {
            goToPlayer = true;
        }

        if (goToPlayer == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.gameObject.transform.position, speed * Time.deltaTime);
        }

        if (distanceToTarget <= collisionRange)
        {
            if (target != null)
            {
                PlayerController target1 = GameObject.FindObjectOfType<PlayerController>();

                target1.TakeExp();

                GameObject destroyParticle = Instantiate(DestroyParticle, transform.position, transform.rotation);
                Destroy(destroyParticle, 2f);

                Destroy(gameObject);
            }
        }
    }

    void Impulse()
    {
        float calculatedForce = Random.Range(minForce, maxForce);

        Vector3 calculatedDirection = (Direction1 * 1f) + (Direction2 * 3f);

        rb.AddForce(calculatedDirection * calculatedForce * 1f, ForceMode.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collisionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectRange);
    }
}
