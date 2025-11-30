using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwordBehavior : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float damage = 100f;
    [SerializeField] private float swingCooldown = .75f;
    [SerializeField] private float comboCooldown = 2f;
    [SerializeField] private float swingBuffer = .15f;

    [Header("Info")]
    [SerializeField] private float swingTimer = 0f;
    [SerializeField] private float comboTimer = 0f;
    [SerializeField] private float bufferTimer = 0f;
    [SerializeField] private float combo = 0;

    public BoundaryBoxBehavior boundaryBoxBehavior;

    public UnityEvent onSwing, onSwing2;

    public void Update()
    {
        if (swingTimer > 0f)
        {
            swingTimer -= Time.deltaTime;
        }

        if (bufferTimer > 0f)
        {
            bufferTimer -= Time.deltaTime;

            if (swingTimer <= 0)
            {
                Swing();
            }
        }

        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
        } else
        {
            combo = 0;
        }
    }

    public void QueueSwing()
    {
        bufferTimer = swingBuffer;
    }

    public void Swing()
    {

        Debug.Log(swingTimer);
        swingTimer = swingCooldown;
        comboTimer = comboCooldown;

        if (combo % 2 >= 1) // on odd swings
        {
            onSwing.Invoke();
        } else {
            onSwing2.Invoke();
        }

        combo += 1;

    }
    
    public void SwordHit()
    {
        LayerMask layerMask = LayerMask.GetMask("Default");

        List<GameObject> objects = boundaryBoxBehavior.getOverlappingObjects(layerMask, "Breakable");
        foreach (GameObject gameObject in objects)
        {
            if (gameObject.TryGetComponent(out BreakableBehavior breakable))
            {
                breakable.TakeDamage(damage);
            }
        }
    }

    public void HandleToss()
    {
        
    }


}
