using UnityEngine;

namespace BetterAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class DevVarAttribute : PropertyAttribute
    {
        public DevVarAttribute()
        {
        }
    }
}
