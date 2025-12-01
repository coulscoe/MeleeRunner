using UnityEngine;

[CreateAssetMenu(fileName = "FloatData", menuName = "Scriptable Objects/Data/FloatDataSO")]
public class FloatDataSO : ScriptableObject
{
    public float value;

    public void setValue(float amount)
    {
        value = amount;
    }

    public void addValue(float amount)
    {
        value += amount;
    }
}
