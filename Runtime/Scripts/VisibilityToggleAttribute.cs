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
    public ValueCondition ValueCondition = ValueCondition.None;

    public VisibilityToggleAttribute(string variable, ValueCondition valueCondition) : this(variable, null)
    {
        ValueCondition = valueCondition;
    }

    public VisibilityToggleAttribute(string variable, object value)
    {
        order = -10;
        Variable = variable;
        Value = value;
    }
}

public enum ValueCondition
{
    None,
    IsNull,
    IsDefault,
    IsNotDefault,
    IsNotNull,
} 
