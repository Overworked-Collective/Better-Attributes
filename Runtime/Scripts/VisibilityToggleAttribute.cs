using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class VisibilityToggleAttribute : PropertyAttribute
{
    public string Variable;
    public object Value;
    public ValueCondition ValueCondition = ValueCondition.IsGivenValue;

    public VisibilityToggleAttribute(string variable, ValueCondition valueCondition) : this(variable, null, valueCondition) { }

    public VisibilityToggleAttribute(string variable, object value, ValueCondition valueCondition = ValueCondition.IsGivenValue)
    {
        order = -10;
        ValueCondition = valueCondition;
        Variable = variable;
        Value = value;
    }
}

public enum ValueCondition
{
    IsGivenValue,
    IsNotGivenValue,
    IsNull,
    IsDefault,
    IsNotDefault,
    IsNotNull,
}
