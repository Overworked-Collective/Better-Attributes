using UnityEngine;

namespace BetterAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class VisualTooltipAttribute : PropertyAttribute
    {
        public string Tooltip;

        public VisualTooltipAttribute(string tooltip)
        {
            Tooltip = tooltip;
        }
    }
}
