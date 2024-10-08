using System;
using UnityEngine;
namespace WidgetAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LineColorAttribute : PropertyAttribute
    {
        public float  red;
        public float  green;
        public float  blue;
        public float  alpha;

        public LineColorAttribute(float red,float green,float blue,float alpha=1)
        {
            this.red=red;
            this.green=green;
            this.blue=blue;
            this.alpha=alpha;
        }
        // Empty constructor returns the default value.
        public LineColorAttribute()
        {
            this.red=1;
            this.green=0;
            this.blue=0;
            this.alpha=1;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class FillColorAttribute : PropertyAttribute
    {
        public float  red;
        public float  green;
        public float  blue;
        public float  alpha;

        public FillColorAttribute(float red,float green,float blue,float alpha=0.25f)
        {
            this.red=red;
            this.green=green;
            this.blue=blue;
            this.alpha=alpha;
        }
        // Empty constructor returns the default value.
        public FillColorAttribute()
        {
            this.red=1;
            this.green=0;
            this.blue=0;
            this.alpha=0.25f;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class ThicknessAttribute : PropertyAttribute
    {
        public float thickness;
        public ThicknessAttribute(float thickness = 1)
        {
            this.thickness=thickness;
        }
    }
    public class LabelBackgroundColorAttribute : PropertyAttribute
    {
        public float red;
        public float green;
        public float blue;
        public float alpha;
        public LabelBackgroundColorAttribute(float red,float green,float blue,float alpha = 1)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }
        // Empty constructor returns the default value.
        public LabelBackgroundColorAttribute()
        {
            this.red = 1;
            this.green = 1;
            this.blue = 1;
            this.alpha = 1;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelTextColorAttribute : PropertyAttribute
    {
        public float red;
        public float green;
        public float blue;
        public float alpha;
        public LabelTextColorAttribute(float red,float green,float blue,float alpha=1)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }
        // Empty constructor returns the default value.
        public LabelTextColorAttribute()
        {
            this.red = 0;
            this.green = 0;
            this.blue = 0;
            this.alpha = 1;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelScreenOffsetAttribute : PropertyAttribute
    {
        public float x;
        public float y;
        public float z;
        public LabelScreenOffsetAttribute(float x,float y,float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        // Empty constructor returns the default value.
        public LabelScreenOffsetAttribute()
        {
            y = -0.2f;
        }
    }
}