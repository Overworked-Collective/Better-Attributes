using UnityEngine;

namespace BetterAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class EndMultiFieldAttribute : PropertyAttribute
    {
        public EndMultiFieldAttribute()
        {

        }
    }
}
