using UnityEngine;

namespace BetterAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class AnimatedToggleAttribute : PropertyAttribute
    {
        public float AnimationSpeed;
        public string Label1, Label2;

        public AnimatedToggleAttribute(string label1, string label2, float animationSpeed = 2.5f)
        {
            AnimationSpeed = animationSpeed;
            Label1 = label1;
            Label2 = label2;
        }
    }
}
