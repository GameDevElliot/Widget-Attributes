using System;
using UnityEngine;

namespace WidgetAttributes
{
    /// <summary>
    /// Widget Attribute.
    /// </summary>
    /// <remarks>
    /// Use this attribute on a Vector2, Vector3, or Rect field.
    /// It will draw a location handle in the scene at the position of the Vector or Rect.
    /// Use the 'space' parameter to specify whether to draw the handle at the transform's local position or world position.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class WidgetAttribute : PropertyAttribute
    {
        public Space space = Space.World;

        /// <param name="space">Specifies the space for drawing the widget handle (local or world, defaults to World).</param>
        public WidgetAttribute(Space space = Space.World)
        {
            this.space = space;
        }
    }

    /// <summary>
    /// ArrowTo Attribute.
    /// </summary>
    /// <remarks>
    /// Use this attribute to draw an arrow from the Vector3 field to the specified point.
    /// Use "nameof()" for safety when referencing the target.
    /// Use 'space' to specify whether to draw it at the transform's local or world position.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class ArrowToAttribute : PropertyAttribute
    {
        public string endPointName;
        public Space startPointSpace;
        public Space endPointSpace;
        public bool isRelativeEndPoint;

        /// <param name="endPointName">The name of the end point (use "nameof()" for safety).</param>
        /// <param name="isRelativeEndPoint">Indicates whether the end point is relative to the current transform (defaults to false).</param>
        /// <param name="startPointSpace">The coordinate space of the start point (defaults to World).</param>
        /// <param name="endPointSpace">The coordinate space of the end point (defaults to World).</param>
        public ArrowToAttribute(string endPointName, bool isRelativeEndPoint = false, Space startPointSpace = Space.World, Space endPointSpace = Space.World)
        {
            this.endPointName = endPointName;
            this.isRelativeEndPoint = isRelativeEndPoint;
            this.startPointSpace = startPointSpace;
            this.endPointSpace = endPointSpace;
        }
    }

    /// <summary>
    /// Label Attribute.
    /// </summary>
    /// <remarks>
    /// Use this attribute to mark a Vector3 field with a label in the scene.
    /// If 'space' is not specified, it will inherit from a WidgetAttribute or ArrowToAttribute on the same field.
    /// Otherwise, the space defaults to World.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelAttribute : PropertyAttribute
    {
        public string labelName;
        public Space? space;

        /// <param name="space">The coordinate space for the label (optional).</param>
        /// <param name="labelName">The name of the label to display (optional).</param>
        public LabelAttribute(Space space, string labelName = null)
        {
            this.space = space;
            this.labelName = labelName;
        }

        /// <param name="labelName">The name of the label to display (optional).</param>
        public LabelAttribute(string labelName = null)
        {
            this.space = null;
            this.labelName = labelName;
        }
    }
}
