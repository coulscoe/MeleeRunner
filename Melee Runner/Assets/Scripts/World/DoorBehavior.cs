using UnityEngine;
using UnityEngine.Events;

public class DoorBehavior : MonoBehaviour
{

    [SerializeField] private bool opened = false;
    [SerializeField] private bool locked = false;

    public UnityEvent onOpen, onClose, onFastOpen, onLocked, onUnlocked, onLockedOpen;
    
    public void Open()
    {
        Debug.Log("Open");
        if (locked == false && opened == false)
        {
            opened = true;
            onOpen.Invoke();
        } else {
            onLockedOpen.Invoke();
        }
    }

    public void FastOpen()
    {
        if (locked == false && opened == false)
        {
            opened = true;
            onFastOpen.Invoke();
        } else {
            onLockedOpen.Invoke();
        }
    }

    public void Close()
    {
        onClose.Invoke();
    }

    public void Lock()
    {
        onLocked.Invoke();
        locked = true;
        Close();
    }

    public void Unlock()
    {
        onUnlocked.Invoke();   
        locked = false;
    }


}
