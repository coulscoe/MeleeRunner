using UnityEngine;

public class RoomBoundaryBehavior : MonoBehaviour
{
    public BoundaryBoxBehavior boundaryBox;
    public GameObject enterance;
    public GameObject exit;

    #region Unity Functions

    private void Awake()
    {
        // Warnings if missing essencial components
        if (boundaryBox == null) { Debug.LogWarning("Can't Find Boundary Box On: " + name); }
        if (enterance == null) { Debug.LogWarning("Can't Find Enterance On: " + name); }
        if (exit == null) { Debug.LogWarning("Can't Find Exit On: " + name); }
    }

    private void OnValidate()
    {
        // If can find boundary Auto-Populate
        Transform boundaryTransform = transform.Find("Boundary");
        if (boundaryTransform != null && boundaryTransform.TryGetComponent(out BoundaryBoxBehavior boundaryComp))
        {
            boundaryBox = boundaryComp;
        }

        // If can find exit Auto-Populate
        Transform enteranceTransform = transform.Find("Enterance");
        if (enteranceTransform != null)
        {
            enterance = enteranceTransform.gameObject;
        }

        // If can find enterance Auto-Populate
        Transform exitTransform = transform.Find("Exit");
        if (exitTransform != null)
        {
            exit = exitTransform.gameObject;
        }
    }

    #endregion
}
