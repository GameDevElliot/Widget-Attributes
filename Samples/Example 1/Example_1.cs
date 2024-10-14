using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WidgetAttributes;

public class Example_1 : MonoBehaviour
{
    [SerializeField, Widget] 
    private Vector2 vec2 = new Vector2(0,6);
    
    [SerializeField, Widget,Label,ArrowTo(nameof(end))]
    private Vector3 startPoint = new Vector3(0,0,0);


    [SerializeField, Widget,Label("Custom Label Name")] 
    private Vector3 end = new Vector3(4,4,0);



    [SerializeField, Widget(Space.Self),Label("Local Point")]
    private Vector3 localPoint = new Vector3(0,0,0);



    [SerializeField, Widget] 
    private Rect rect = new Rect(0,0,6,4);


    [FillColor(0.9f,0.9f,0.9f,0.25f)]
    [SerializeField, Widget(Space.Self)]
    private Rect rect2 = new Rect(5,5,6,4);
 
}
