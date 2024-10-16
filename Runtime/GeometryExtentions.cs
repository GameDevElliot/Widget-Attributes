using UnityEngine;
namespace WidgetAttributes.Primitives.Geometry
{
    // Maybe I'm getting carried away here.
    public static class PrimitiveGeometryExtentions
    {
        #region Circle
        // Calculates the length of an arc given an angle in radians
        public static float ArcLength(this Circle circle, float angleRadians)
        {
            return angleRadians * circle.radius;
        }
        public static float SliceArea(this Circle circle, float angleRadians)
        {
            return 0.5f * angleRadians * circle.radius * circle.radius;
        }

        // Calculates the chord length of a given angle in radians
        public static float ChordLength(this Circle circle, float angleRadians)
        {
            return 2 * circle.radius * Mathf.Sin(angleRadians / 2);
        }

        // Calculates the sagitta (height of the arc) given an angle in radians
        public static float Sagitta(this Circle circle, float angleRadians)
        {
            float halfChord = circle.ChordLength(angleRadians) / 2;
            return circle.radius - Mathf.Sqrt(circle.radius * circle.radius - halfChord * halfChord);
        }

        // Calculates the segment area (area between the chord and the arc) given an angle in radians
        public static float SegmentArea(this Circle circle, float angleRadians)
        {
            float sliceArea = circle.SliceArea(angleRadians);
            float triangleArea = 0.5f * circle.ChordLength(angleRadians) * circle.Sagitta(angleRadians);
            return sliceArea - triangleArea;
        }
        #endregion
        #region Sphere
        // Calculates the arc length on a great circle given an angle in radians
        public static float ArcLength(this Sphere sphere, float angleRadians)
        {
            return angleRadians * sphere.radius;
        }

        // Calculates the surface area of a spherical cap given the height h of the cap
        public static float SphericalCapSurfaceArea(this Sphere sphere, float height)
        {
            return 2 * Mathf.PI * sphere.radius * height;
        }

        // Calculates the volume of a spherical cap given the height h of the cap
        public static float SphericalCapVolume(this Sphere sphere, float height)
        {
            return (Mathf.PI * Mathf.Pow(height, 2) * (3 * sphere.radius - height)) / 3;
        }

        // Calculates the surface area of a spherical segment (a portion between two parallel planes)
        public static float SphericalSegmentSurfaceArea(this Sphere sphere, float height1, float height2)
        {
            return 2 * Mathf.PI * sphere.radius * Mathf.Abs(height1 - height2);
        }

        // Calculates the volume of a spherical segment (a portion between two parallel planes)
        public static float SphericalSegmentVolume(this Sphere sphere, float height1, float height2)
        {
            float h1 = Mathf.Min(height1, height2);
            float h2 = Mathf.Max(height1, height2);
            return sphere.SphericalCapVolume(h2) - sphere.SphericalCapVolume(h1);
        }

        // Calculates the chord length of a given angle in radians (on a great circle)
        public static float ChordLength(this Sphere sphere, float angleRadians)
        {
            return 2 * sphere.radius * Mathf.Sin(angleRadians / 2);
        }
        // Calculates the length of a parallel circle at a given latitude (angle from the equator in radians)
        public static float ParallelCircleLength(this Sphere sphere, float latitudeRadians)
        {
            return 2 * Mathf.PI * sphere.radius * Mathf.Cos(latitudeRadians);
        }
        #endregion
    }
}