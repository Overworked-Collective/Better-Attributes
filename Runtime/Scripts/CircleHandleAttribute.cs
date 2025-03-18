using UnityEngine;

namespace BetterAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class CircleHandleAttribute : PropertyAttribute
    {
        public bool FillCircle;
        public float Min = 0, Max = float.PositiveInfinity;
        public string MinByReference = "", MaxByReference = "";

        public CircleHandleAttribute()
        {
            order = -5;
        }
    }
}
