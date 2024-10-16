
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using WidgetAttributes;


public class Example_3 : MonoBehaviour
{
    [SerializeField, Widget] 
    private Vector3[] vec3Array = new Vector3[]{Vector3.down,Vector3.left,Vector3.up,Vector3.forward,Vector3.back,Vector3.right};


    [SerializeField, Widget(Space.Self)] 
    private List<Vector3> vec3List= new List<Vector3>{Vector3.down,Vector3.left,Vector3.up,Vector3.forward,Vector3.back,Vector3.right};


    [SerializeField, Widget(Space.Self), Label] CustomStructExample customStructExample = new CustomStructExample(float3.zero,"Test",Quaternion.identity);

}
