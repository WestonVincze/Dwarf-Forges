using System;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    // Set the maximum health and initial health
    public float maxHealth = 100f;
    private float currentHealth;

    // Actions that can be registered to
    public event Action<float> OnReceiveDamage;
    public event Action<float> OnHeal;
    public event Action OnDeath;

    private void Start()
    {
        // Initialize currentHealth with maxHealth at the start
        currentHealth = maxHealth;
    }

    // Function for handling damage
    public void TakeDamage(float damage)
    {
        // Reduce health
        currentHealth -= damage;
        OnReceiveDamage?.Invoke(damage);

        // Check if health drops below 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Function for handling healing
    public void Heal(float healingAmount)
    {
        currentHealth += healingAmount;
        // Ensure health doesn't exceed maxHealth
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHeal?.Invoke(healingAmount);
    }

    // Function for handling death
    private void Die()
    {
        OnDeath?.Invoke();
        // Here you can add additional logic for what happens when the object dies,
        // like playing an animation or destroying the game object.
        Destroy(gameObject);
    }
}
