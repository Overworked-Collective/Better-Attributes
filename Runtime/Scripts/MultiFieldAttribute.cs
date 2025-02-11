using UnityEngine;

namespace BetterAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class MultiFieldAttribute : PropertyAttribute
    {
        public MultiFieldAttribute()
        {

        }
    }
}
