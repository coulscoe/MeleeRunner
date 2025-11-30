using UnityEngine;
using UnityEngine.Events;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    [Header("Events")]
    public UnityEvent onDeath;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        onDeath.Invoke();
    }
    
}
