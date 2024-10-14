using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using WidgetAttributes;

public class Example_2 : MonoBehaviour
{
    [SerializeField, Widget,ArrowTo(nameof(float3)),Label] 
    private float2 float2 = new float2(0,3);


    [SerializeField, Widget,Label] 
    private float3 float3 = new float3(0,-3,0);


    [SerializeField, Widget,Label,ArrowTo(nameof(localVec2int),false,Space.World,Space.Self)] 
    private Vector3Int worldVec3int = new Vector3Int();


    [SerializeField, Widget(Space.Self),Label] 
    private Vector2Int localVec2int = new Vector2Int();
 
}
