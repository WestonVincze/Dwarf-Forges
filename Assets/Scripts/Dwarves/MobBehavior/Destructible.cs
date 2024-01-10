using System;
using UnityEngine;

public class Destructible : DestroyableMonoBehavior
{
    // Set the maximum health and initial health
    public float maxHealth = 100f;
    private float currentHealth;

    // Actions that can be registered to
    public Action<float> OnReceiveDamage;
    public Action<float> OnHeal;
    public Action OnDeath;

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
        OnDeath = null;

        //TODO: Implement death visuals. Either spawn in a new object
        //or delay to the end of the death animation before destroy.
        this.Destroy(3);
    }

  public float Health
  {
    get
    {
      return currentHealth;
    }
  }
}
