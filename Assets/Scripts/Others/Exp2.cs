using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp2 : MonoBehaviour
{
    public float DetectRange = 1f;
    public float speed = 2f;

    [Header("Player Detection")]
    public float collisionRange = 0.4f;
    public GameObject DestroyParticle;

    bool goToPlayer = false;

    Transform target;

    void Start()
    {
        target = PlayerManager.instance.player.transform;

        goToPlayer = false;
    }

    void Update()
    {
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
            PlayerController target1 = GameObject.FindObjectOfType<PlayerController>();

            target1.TakeExp();

            GameObject destroyParticle = Instantiate(DestroyParticle, transform.position, transform.rotation);
            Destroy(destroyParticle, 2f);

            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collisionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectRange);
    }
}
