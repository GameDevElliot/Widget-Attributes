using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using WidgetAttributes;

[System.Serializable]
public struct CustomStructExample : IWidgetLocation, ILabelName, IWidgetRotation
{
    [SerializeField] private float3 location;
    [SerializeField] private string name;
    [SerializeField] private Quaternion rotation;
    public Vector3 GetLocationForWidget()
    {
        return location;
    }
    public void SetLocationFromWidget(Vector3 location)
    {
        this.location = location;
    }

    public string GetNameForLabel()
    {
        return name;
    }

    public Quaternion GetRotationForWidget()
    {
        return rotation;
    }

    public void SetRotationfromWidget(Quaternion rotation)
    {
       this.rotation = rotation;
    }

    public CustomStructExample(float3 location, string name, Quaternion rotation)
    {
        this.location = location;
        this.name = name;
        this.rotation = rotation;
    }
}
