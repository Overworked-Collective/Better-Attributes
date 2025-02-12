using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BetterAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string ButtonText;
        public ButtonAttribute(string buttonText = "")
        {
            ButtonText = buttonText;
        }
    }
}
