using System;
using UnityEngine;

namespace WidgetAttributes
{
    // Widget Attribute
    //
    // Use it on a Vector2, Vector3 or Rect field. 
    // It will draw a location handle in the scene, at the location of the Vector or Rect.
    // Use 'space' to specify weather to draw it at the transform's local position or the world position.


    [AttributeUsage(AttributeTargets.Field)]
    public class WidgetAttribute : PropertyAttribute
    {
        public Space space = Space.World;
        public WidgetAttribute(Space space = Space.World)
        {
            this.space = space;
        }
    }
    // ArrowTo Attribute.
    //
    // Use it to draw an arrow from the Vector3 field to the specified point.
    //
    // use "nameof()" for safty.
    // Use 'space' to specify weather to draw it at the transform's local position or the world position.

    [AttributeUsage(AttributeTargets.Field)]
    public class ArrowToAttribute : PropertyAttribute
    {
        public string endPointName;
        public Space startPointSpace;
        public Space endPointSpace;
        public bool isRelativeEndPoint;

        public ArrowToAttribute(string endPointName,bool isRelativeEndPoint = false, Space startPointSpace = Space.World, Space endPointSpace = Space.World)
        {
            this.endPointName = endPointName;
            this.isRelativeEndPoint = isRelativeEndPoint;
            this.startPointSpace = startPointSpace;
            this.endPointSpace = endPointSpace;
        }

    }

    // Label Attribute. Use it to mark A Vector3 with a label
    //
    // If a space is not specified, space will be inherited from a WidgetAttribute or an ArrowAttribute on the same field.
    // Otherwise space defaults to World.

    [AttributeUsage(AttributeTargets.Field)]
    public class LabelAttribute : PropertyAttribute
    {
        public string labelName;
        public Space? space;
        // public float xOffset;
        // public float yOffset;
        // public float zOffset;
        public LabelAttribute(Space space,string labelName=null)
        {
            this.space = space;
            this.labelName = labelName;

        }
        public LabelAttribute(string labelName=null)
        {
            this.space = null;
            this.labelName = labelName;

        }
    }
}