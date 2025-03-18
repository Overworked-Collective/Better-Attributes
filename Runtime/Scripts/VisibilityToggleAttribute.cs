using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class VisibilityToggleAttribute : PropertyAttribute
{
    public string Variable;
    public object Value;

    public VisibilityToggleAttribute(string variable, object value)
    {
        order = -10;
        Variable = variable;
        Value = value;
    }
}
