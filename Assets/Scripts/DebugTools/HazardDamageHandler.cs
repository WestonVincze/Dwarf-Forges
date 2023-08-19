using UnityEngine;

public class HazardDamageHandler : MonoBehaviour
{
    public float damageAmount = 10f;

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
