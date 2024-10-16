using UnityEngine;
using WidgetAttributes.Primitives;

namespace WidgetAttributesUtillities
{
    public static class Vector3Extentions
    {
        /// <summary>
        /// Performs component-wise division of Vector3 'a' by Vector3 'b'.
        /// </summary>
        /// <remarks>
        /// This method is intended to be the inverse of Vector3.Scale.
        /// It is useful for unscaling vectors that have previously been scaled by 'b'.
        /// Since division by zero is undefined, any component of 'b' that is zero results in the corresponding component of the output being set to 0, 
        /// as the original value is considered lost.
        /// </remarks>
        /// <param name="a">The Vector3 to be unscaled.</param>
        /// <param name="b">The Vector3 by which 'a' will be divided component-wise.</param>
        /// <returns>A new Vector3 with the result of the component-wise division.</returns>

        public static Vector3 UnScale(Vector3 a, Vector3 b)
        {
            return new Vector3
            (
                b.x != 0 ? a.x / b.x : 0, 
                b.y != 0 ? a.y / b.y : 0, 
                b.z != 0 ? a.z / b.z : 0
            );
        }
    }
    public static class TransformExtentions
    {
        public static void SetLocalData(this Transform transform, TransformData transformData)
        {
            transform.localPosition = transformData.position;
            transform.localRotation = transformData.rotation;
            transform.localScale = transformData.scale;
        }
        public static void SetPositionAndRotation(this Transform transform, TransformData transformData)
        {
            transform.position = transformData.position;
            transform.rotation = transformData.rotation;
        }
        public static void SetLocalPositionAndRotation(this Transform transform, TransformData transformData)
        {
            transform.localPosition = transformData.position;
            transform.localRotation = transformData.rotation;
        }
        public static TransformData GetLocalData(this Transform transform)
        {
            return new TransformData(transform.localPosition,transform.localRotation,transform.localScale);
        }
        public static TransformData GetGlobalData(this Transform transform)
        {
            return new TransformData(transform.position,transform.rotation,transform.lossyScale);
        }
    }
}