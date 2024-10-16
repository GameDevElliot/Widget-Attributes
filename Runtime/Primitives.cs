using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace WidgetAttributes.Primitives
{
    [Serializable]
    public struct Circle
    {
        public Vector2 centre;
        public float radius;

        public Circle(Vector2 centre, float radius=1)
        {
            this.centre = centre;
            this.radius = radius;
        }
        public float Area => Mathf.PI * radius * radius;

        public float Circumference => 2 * Mathf.PI * radius;
        public Vector2 RandomPointInside => Random.insideUnitCircle * radius + centre;
        public Vector2 RandomPointOnCircumferance
        {
            get
            {
                float angle = Random.Range(0f, Mathf.PI * 2);
                return centre + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            }
        }
        // Checks if a point is inside or on the circle
        public bool Contains(Vector2 point)
        {
            return (centre-point).sqrMagnitude <= radius*radius;
        }
        public bool Intersects(Circle other)
        {
            return (centre - other.centre).sqrMagnitude <= (radius*radius) + (other.radius*other.radius);
        }
        public bool Intersects(Rect other)
        {
            // Find the closest point on the rectangle to the circle's center
            float closestX = Mathf.Clamp(centre.x, other.xMin, other.xMax);
            float closestY = Mathf.Clamp(centre.y, other.yMin, other.yMax);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = centre.x - closestX;
            float distanceY = centre.y - closestY;

            // If the distance is less than or equal to the circle's radius, they intersect
            return (distanceX * distanceX + distanceY * distanceY) <= (radius * radius);
        }

    }

    [Serializable]
    public struct Sphere
    {
        public Vector3 centre;
        public float radius;

        public Sphere(Vector3 centre, float radius = 1)
        {
            this.centre = centre;
            this.radius = radius;
        }
        public Vector2 RandomPointInside => Random.insideUnitSphere * radius + centre;
        public Vector2 RandomPointOnCircumferance => Random.onUnitSphere * radius + centre;
        
        // Checks if a point is inside or on the Sphere
        public bool Contains(Vector3 point)
        {
            return (centre-point).sqrMagnitude <= radius*radius;
        }
        public bool Intersects(Sphere other)
        {
            return (centre - other.centre).sqrMagnitude <= (radius*radius) + (other.radius*other.radius);

        }
        public float Volume => (4f / 3f) * Mathf.PI * Mathf.Pow(radius, 3);

        public float SurfaceArea => 4 * Mathf.PI * Mathf.Pow(radius, 2);
    }

    // A 'Transform' in Unity is more than just a struct containing position, rotation & scale.
    // They are tied to game objects and have parents and children. Does that remind you of someone?
    //
    // A TransformData struct was therefore required to represent the essence of what a transform shoud be.
    // Use it if you want, or not. Up to you.
    //
    // "TransformAccess" had some potential for this role but had some odd limmitations, and is intended for use with Unity Jobs.

    [Serializable]
    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

    }
}