using UnityEngine;

public class HazardDamageHandler : MonoBehaviour
{
    public float damageAmount = 10f;

    private void OnTriggerEnter(Collider collider)
    {
        Destructible destructible = collider.gameObject.GetComponent<Destructible>();
        if (destructible)
        {
            destructible.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destructible destructible = collision.gameObject.GetComponent<Destructible>();
        if (destructible)
        {
            destructible.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
