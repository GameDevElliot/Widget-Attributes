using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WidgetAttributes;

public class Example_1 : MonoBehaviour
{
    [SerializeField, Widget] private Rect rect = new Rect(0,0,6,4);

    [ArrowTo(nameof(end))]
    [SerializeField, Widget,Label("Custom Label")] private Vector3 start = new Vector2(0,0);
    [SerializeField, Widget,Label] private Vector3 end = new Vector2(4,4);
    
}
