using UnityEngine;

namespace Weapons
{
    /**
      * Damage: base damage when weapon hits enemy
      * Crit %: chance to land a critical hit
      * Crit damage: multiplier for crit damage
      * Knockback: amount of force applied to hits
      * Durability: number of swings that hit at least one target before weapon is destroyed
      * Toughness %: chance to avoid durability cost on hit
      * Attack speed: rest time between attacks
      * Penetration: number of enemies 
      * Recovery: rest time between combos
      * Combo: number of consecutive attacks
      */
    public class Weapon : MonoBehaviour
    {
        [Header("Weapon Stats")]
        [Tooltip("Base damage of weapon")]
        [SerializeField]
        private float _damage;
        [Tooltip("chance to land a critical hit")]
        [SerializeField]
        private float _critChance;
        [Tooltip("multiplier for crit damage")]
        [SerializeField]
        private float _critDamage;
        [Tooltip("amount of force applied to hits")]
        [SerializeField]
        private float _knockback;
        [Tooltip("number of swings that hit at least one target before weapon is destroyed")]
        [SerializeField]
        private float _durability;
        [Tooltip("chance to avoid durability cost on hit")]
        [SerializeField]
        private float _toughness;
        [Tooltip("rest time between attacks")]
        [SerializeField]
        private float _attackSpeed;
        [Tooltip("number of enemies ")]
        [SerializeField]
        private float _recovery;
        [Tooltip("rest time between combos")]
        [SerializeField]
        private int _penetration;
        [Tooltip("number of consecutive attacks")]
        [SerializeField]
        private int _combo;

        private Animator _animator;

        void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
        }

        public void Attack()
        {
            if (_animator)
            {
                _animator.SetTrigger("attack");
            }
        }

    }
}