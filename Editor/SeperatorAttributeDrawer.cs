using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SeperatorAttribute))]
public class SeperatorAttributeDrawer : DecoratorDrawer
{
    SeperatorAttribute _seperator { get { return (SeperatorAttribute)attribute; } }
    static float _padding = 9;

    public override float GetHeight()
    {
        return 4 + _padding*2;
    }

    public override void OnGUI(Rect position)
    {
        Rect lineRect = new Rect(position);
        lineRect.y += _padding;
        lineRect.height = 4;

        Handles.color = Color.white;
        
        switch (_seperator.Type)
        {
            case SeperatorAttribute.LineType.Line:
                Handles.DrawLine(lineRect.position, lineRect.position + new Vector2(lineRect.width, 0));
                break;
            case SeperatorAttribute.LineType.Dashed:
                float dashedLength = 4;
                float xPos = position.x;
                while(xPos < position.x + position.width)
                {
                    Handles.DrawLine(new Vector2(xPos, lineRect.y), new Vector2(xPos + dashedLength, lineRect.y));
                    xPos += dashedLength*2;
                }
                //Handles.DrawDottedLine(lineRect.position, lineRect.position + new Vector2(lineRect.width, 0), 4);
                break;
            case SeperatorAttribute.LineType.Double:
                Handles.DrawLine(lineRect.position, lineRect.position + new Vector2(lineRect.width, 0));
                Handles.DrawLine(lineRect.position + new Vector2(0, lineRect.height), lineRect.position + new Vector2(lineRect.width, lineRect.height));
                break;
            default:
                break;
        }
        base.OnGUI(position);
    }
}
