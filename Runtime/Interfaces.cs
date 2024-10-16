using UnityEngine;
namespace WidgetAttributes
{
    public interface IWidgetLocation
    {
        public Vector3 GetLocationForWidget();

        public void SetLocationFromWidget(Vector3 location);
        
    }
    public interface IWidgetRotation
    {
        public Quaternion GetRotationForWidget();

        public void SetRotationfromWidget(Quaternion rotation);
        
    }
    public interface IWidgetScale
    {
        public Vector3 GetScaleForWidget();

        public void SetScaleFromWidget(Vector3 scale);
    }
    public interface ILabelName
    {
        public string GetNameForLabel();

    }
}