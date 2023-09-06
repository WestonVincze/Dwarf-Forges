using UnityEngine;

public class HazardDamageHandler : DestroyableMonoBehavior
{
    public float damageAmount = 10f;
    private void OnTriggerEnter(Collider collider)
    {
        Destructible destructible = collider.gameObject.GetComponent<Destructible>();
        if (destructible)
        {
            destructible.TakeDamage(damageAmount);
            this.Destroy();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destructible destructible = collision.gameObject.GetComponent<Destructible>();
        if (destructible)
        {
            destructible.TakeDamage(damageAmount);
            this.Destroy();
        }
    }
}
