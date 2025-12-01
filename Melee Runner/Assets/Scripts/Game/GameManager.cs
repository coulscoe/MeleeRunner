using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public RoomManager roomManager;

    public UnityEvent onRoundStart, onRoundEnd;

    public void StartRound()
    {
        onRoundStart.Invoke();
    }

    public void EndRound()
    {
        onRoundEnd.Invoke();
    }
}
