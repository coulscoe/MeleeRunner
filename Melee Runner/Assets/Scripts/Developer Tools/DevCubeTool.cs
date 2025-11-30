using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

[ExecuteInEditMode]
public class DevCubeTool : MonoBehaviour
{
    public enum GenerationMode { SixPoint, TwoCorners }

    [Header("Mode")]
    public GenerationMode mode = GenerationMode.SixPoint;

    [Header("Six-point mode (min/max per axis)")]
    public GameObject xmin, xmax, ymin, ymax, zmin, zmax;

    [Header("Two-corner mode (opposite corners)")]
    public GameObject cornerA, cornerB;

    public void GenerateCube()
    {
        GameObject targetParent = null;

        Vector3 center = Vector3.zero;
        Vector3 size = Vector3.zero;

        if (mode == GenerationMode.SixPoint)
        {
            // Validate
            if (xmin == null || xmax == null || ymin == null || ymax == null || zmin == null || zmax == null)
            {
                Debug.LogError("SixPoint mode requires xmin, xmax, ymin, ymax, zmin, and zmax to be assigned.");
                return;
            }

            center = new Vector3(
                (xmin.transform.position.x + xmax.transform.position.x) / 2,
                (ymin.transform.position.y + ymax.transform.position.y) / 2,
                (zmin.transform.position.z + zmax.transform.position.z) / 2
            );

            size = new Vector3(
                Mathf.Abs(xmax.transform.position.x - xmin.transform.position.x),
                Mathf.Abs(ymax.transform.position.y - ymin.transform.position.y),
                Mathf.Abs(zmax.transform.position.z - zmin.transform.position.z)
            );
        }
        else // TwoCorners
        {
            if (cornerA == null || cornerB == null)
            {
                Debug.LogError("TwoCorners mode requires cornerA and cornerB to be assigned.");
                return;
            }

            Vector3 a = cornerA.transform.position;
            Vector3 b = cornerB.transform.position;

            center = (a + b) * 0.5f;
            Vector3 delta = b - a;
            size = new Vector3(Mathf.Abs(delta.x), Mathf.Abs(delta.y), Mathf.Abs(delta.z));
        }

        #if UNITY_EDITOR

        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null)
        {
            // We are in Prefab Mode → spawn inside the prefab root
            targetParent = prefabStage.prefabContentsRoot;
        }

        #endif
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.position = center;
        cube.transform.localScale = size;
        // Set parent if we found a targetParent (in prefab mode). Otherwise leave it unparented in the scene.
        if (targetParent != null)
        {
            cube.transform.SetParent(targetParent.transform, false);
        }

        Debug.Log("Cube Created");
    }
}
