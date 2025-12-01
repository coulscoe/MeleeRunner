using UnityEngine;

public class PitBehavior : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBehavior player))
        {
            player.TakeDamage(2142069);
        }
    }
}
