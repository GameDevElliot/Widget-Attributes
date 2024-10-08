using UnityEngine;

namespace WidgetAttributesUtillities
{
    public static class Vector3Extentions
    {
        // Component-wise devision of Vector3 'a' by Vector3 'b'
        //
        // Intended to be the inverse of Vector3.Scale
        // 
        // Usefull for un-scaling vectors that have previously been scaled by 'b'.
        // Since we cannot divide by 0, any component that you attempt to unscale by 0, the origional value is considered lost and 0 is returned.

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
}