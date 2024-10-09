using System;
using UnityEngine;

namespace WidgetAttributes
{
    /// <summary>
    /// LineColor Attribute.
    /// </summary>
    /// <remarks>
    /// Use this attribute to set the color of a line. The color is specified using RGBA values.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class LineColorAttribute : PropertyAttribute
    {
        public float red;
        public float green;
        public float blue;
        public float alpha;

        /// <param name="red">The red component of the line color (0-1).</param>
        /// <param name="green">The green component of the line color (0-1).</param>
        /// <param name="blue">The blue component of the line color (0-1).</param>
        /// <param name="alpha">The alpha (transparency) of the line color (0-1), defaults to 1 (fully opaque).</param>
        public LineColorAttribute(float red, float green, float blue, float alpha = 1)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }
        public LineColorAttribute()
        {
            this.red = 1;
            this.green = 0;
            this.blue = 0;
            this.alpha = 1;
        }
    }

    /// <summary>
    /// FillColor Attribute.
    /// </summary>
    /// <remarks>
    /// Use this attribute to set the fill color of a shape. The color is specified using RGBA values.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class FillColorAttribute : PropertyAttribute
    {
        public float red;
        public float green;
        public float blue;
        public float alpha;

        /// <param name="red">The red component of the fill color (0-1).</param>
        /// <param name="green">The green component of the fill color (0-1).</param>
        /// <param name="blue">The blue component of the fill color (0-1).</param>
        /// <param name="alpha">The alpha (transparency) of the fill color (0-1), defaults to 0.25.</param>
        public FillColorAttribute(float red, float green, float blue, float alpha = 0.25f)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }
        public FillColorAttribute()
        {
            this.red = 1;
            this.green = 0;
            this.blue = 0;
            this.alpha = 0.25f;
        }
    }

    /// <summary>
    /// Thickness Attribute.
    /// </summary>
    /// <remarks>
    /// Use this attribute to set the thickness of a line.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class ThicknessAttribute : PropertyAttribute
    {
        public float thickness;

        /// <param name="thickness">The thickness of the line, defaults to 1.</param>
        public ThicknessAttribute(float thickness = 1)
        {
            this.thickness = thickness;
        }
    }

    /// <summary>
    /// LabelBackgroundColor Attribute.
    /// </summary>
    /// <remarks>
    /// Use this attribute to set the background color of a label. The color is specified using RGBA values.
    /// </remarks>
    public class LabelBackgroundColorAttribute : PropertyAttribute
    {
        public float red;
        public float green;
        public float blue;
        public float alpha;

        /// <param name="red">The red component of the background color (0-1).</param>
        /// <param name="green">The green component of the background color (0-1).</param>
        /// <param name="blue">The blue component of the background color (0-1).</param>
        /// <param name="alpha">The alpha (transparency) of the background color (0-1), defaults to 1 (fully opaque).</param>
        public LabelBackgroundColorAttribute(float red, float green, float blue, float alpha = 1)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }
        public LabelBackgroundColorAttribute()
        {
            this.red = 1;
            this.green = 1;
            this.blue = 1;
            this.alpha = 1;
        }
    }

    /// <summary>
    /// LabelTextColor Attribute.
    /// </summary>
    /// <remarks>
    /// Use this attribute to set the text color of a label. The color is specified using RGBA values.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelTextColorAttribute : PropertyAttribute
    {
        public float red;
        public float green;
        public float blue;
        public float alpha;

        /// <param name="red">The red component of the text color (0-1).</param>
        /// <param name="green">The green component of the text color (0-1).</param>
        /// <param name="blue">The blue component of the text color (0-1).</param>
        /// <param name="alpha">The alpha (transparency) of the text color (0-1), defaults to 1 (fully opaque).</param>
        public LabelTextColorAttribute(float red, float green, float blue, float alpha = 1)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }

        public LabelTextColorAttribute()
        {
            this.red = 0;
            this.green = 0;
            this.blue = 0;
            this.alpha = 1;
        }
    }

    /// <summary>
    /// LabelScreenOffset Attribute.
    /// </summary>
    /// <remarks>
    /// Use this attribute to specify the screen offset for a label in 3D space.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelScreenOffsetAttribute : PropertyAttribute
    {
        public float x;
        public float y;
        public float z;

        /// <param name="x">The X offset in screen space.</param>
        /// <param name="y">The Y offset in screen space.</param>
        /// <param name="z">The Z offset in screen space.</param>
        public LabelScreenOffsetAttribute(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public LabelScreenOffsetAttribute()
        {
            y = -0.2f;
        }
    }
}
