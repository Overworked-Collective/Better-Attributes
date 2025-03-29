using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.All)]
public class SeperatorAttribute : PropertyAttribute
{
    public LineType Type;

    public SeperatorAttribute(LineType type = LineType.Line)
    {
        Type = type;
    }
    
    public enum LineType
    {
        Line,
        Dashed,
        Double
    }
}
