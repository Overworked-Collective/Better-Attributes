using System;
using UnityEngine;

namespace BetterAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ObjectPreviewAttribute : PropertyAttribute
    {
        public ObjectPreviewAttribute()
        {

        }
    }
}
