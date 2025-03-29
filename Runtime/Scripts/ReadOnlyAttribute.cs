using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute
{
    public ReadOnlyAttribute()
    {
        order = -10;
    }
}
