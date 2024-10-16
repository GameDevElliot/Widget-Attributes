using UnityEngine;
using UnityEditor;
using System.Reflection;
using WidgetAttributes;
using WidgetAttributesUtillities;
using Unity.Mathematics;
using WidgetAttributes.Primitives;
using System.Collections.Generic;
using System;
using System.Collections;

[CustomEditor(typeof(MonoBehaviour), true)]
public class WidgetAttributesEditor : Editor
{
    MonoBehaviour monoBehaviour;
    Transform transform;
    FieldInfo[] fieldInfos;
    Space space = Space.World;

    #region Style
    private float thickness = 1;
    private Color lineColor = new Color(1,0,0,1);
    private Color fillColor = new Color(1,0,0,0.25f);  
    private Texture2D labelBackgroundTexture;
    private Dictionary<Color,Texture2D> labelBackgroundTextureCache = new Dictionary<Color, Texture2D>();
    private Color labelTextColor = new Color(0,0,0,1);
    private Color handleColor = new Color(1,1,0,1);
    private Vector3 labelScreenOffset = new Vector3(0,-0.25f,0);

    #endregion
    private void OnSceneGUI()
    {
        monoBehaviour = (MonoBehaviour)target;
        transform =  monoBehaviour.transform;
        fieldInfos = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //Apply default style
        lineColor = new Color(1,0,0,1);
        fillColor = new Color(1,0,0,0.25f);
        labelBackgroundTexture = Texture2D.whiteTexture;
        labelTextColor = new Color(0,0,0,1);
        handleColor = new Color(1,1,0,1);
        labelScreenOffset = new Vector3(0,-0.25f,0);

        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            object value = fieldInfo.GetValue(target);
            IEnumerable<Attribute> attributes = fieldInfo.GetCustomAttributes();

            foreach (Attribute attribute in attributes)
            {
                switch(attribute)
                {
                    case ThicknessAttribute thicknessAttribute:
                        thickness = thicknessAttribute.thickness;
                        break;

                    case FillColorAttribute fillColorAttribute:
                        fillColor = new Color(fillColorAttribute.red, fillColorAttribute.green, fillColorAttribute.blue, fillColorAttribute.alpha);
                        break;

                    case LineColorAttribute lineColorAttribute:
                        lineColor = new Color(lineColorAttribute.red, lineColorAttribute.green, lineColorAttribute.blue,lineColorAttribute.alpha);
                        break;

                    case LabelBackgroundColorAttribute labelBackgroundColorAttribute:
                        labelBackgroundTexture = CreateSolidColorTexture(new Color(labelBackgroundColorAttribute.red, labelBackgroundColorAttribute.green, labelBackgroundColorAttribute.blue, labelBackgroundColorAttribute.alpha));
                        break;

                    case LabelTextColorAttribute labelTextColorAttribute:
                        labelTextColor = new Color(labelTextColorAttribute.red, labelTextColorAttribute.green, labelTextColorAttribute.blue, labelTextColorAttribute.alpha);
                        break;

                    case LabelScreenOffsetAttribute labelScreenOffsetAttribute:
                        labelScreenOffset = new Vector3(labelScreenOffsetAttribute.x, labelScreenOffsetAttribute.y, labelScreenOffsetAttribute.z);
                        break;

                    case WidgetAttribute widgetAttribute:
                        space = widgetAttribute.space;
                        Quaternion widgetRotationBase = space == Space.Self && Tools.pivotRotation==PivotRotation.Local? transform.rotation : Quaternion.identity;

                        // if(value is IEnumerable)
                        // {
                        //     foreach(var item in (IEnumerable)value){

                        //     }
                        // }
                        switch(value)
                        {
                            case IWidgetLocation:
                                if(Tools.current==Tool.Move)
                                {
                                    ProcessLocationWidget(fieldInfo,widgetRotationBase);
                                }
                                else if(Tools.current==Tool.Rotate && value is IWidgetRotation)
                                {
                                    ProcessRotationWidget(fieldInfo,widgetRotationBase);
                                }
                                // else if(Tools.current==Tool.Scale && value is IWidgetRotation)
                                // {
                                //     ProcessScaleWidget(fieldInfo,widgetRotation);
                                // }
                                break;
                            
                            case Vector3 or Vector2 or float3 or float2 or Vector3Int or Vector2Int:
                                ProcessLocationWidget(fieldInfo,widgetRotationBase);
                                break;

                            case Rect:
                                ProcessRectWidget(fieldInfo,widgetRotationBase);
                                break;

                            case TransformData:
                                ProcessTransformWidget(fieldInfo);
                                break;

                            case Circle:
                                //ProcessCircleWidget(fieldInfo);
                                break;
                        }
                        break;
                    case ArrowToAttribute arrowToAttribute:
                        if(value is Vector3 or Vector2 or float3 or float2 or Vector3Int or Vector2Int or Rect)
                        {
                            ProcessArrowWidget(arrowToAttribute,fieldInfo);
                        }
                        break;
                    case LabelAttribute labelAttribute:
                        if(value is IWidgetLocation or Vector3 or Vector2 or float3 or float2 or Vector3Int or Vector2Int or Rect)
                        {
                            ProcessLabelWidget(labelAttribute,fieldInfo);
                        }
                        break;
                }
            }
        }
    }

    private void ProcessRotationWidget(FieldInfo fieldInfo, Quaternion widgetRotationBase)
    {
        object value = fieldInfo.GetValue(target);
        Vector3 vectorValue = GetLocationFromField(fieldInfo,target);
        IWidgetRotation _IWidgetRotation = (IWidgetRotation)value;
        Quaternion rotation = _IWidgetRotation.GetRotationForWidget() * widgetRotationBase;
        if(space == Space.Self)
        {
            vectorValue = transform.TransformPoint(vectorValue);
        }
        
        // Draw the position handle
        EditorGUI.BeginChangeCheck();

        Quaternion newRotation = rotation;
        newRotation = Handles.RotationHandle(newRotation,vectorValue) * Quaternion.Inverse(widgetRotationBase);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move Handle");
            _IWidgetRotation.SetRotationfromWidget(newRotation);
            fieldInfo.SetValue(target, _IWidgetRotation);
        }
    }

    private void ProcessTransformWidget(FieldInfo fieldInfo)
    {
        TransformData transformDataValue = (TransformData)fieldInfo.GetValue(target);

        if(space == Space.Self) // Convert from local to global coordinates
        {
            transformDataValue.position = transform.TransformPoint(transformDataValue.position);
            transformDataValue.rotation = transform.rotation * transformDataValue.rotation;
            transformDataValue.scale = Vector3.Scale(transform.lossyScale,transformDataValue.scale);
        }

        EditorGUI.BeginChangeCheck();
        Handles.TransformHandle(ref transformDataValue.position,ref transformDataValue.rotation, ref transformDataValue.scale);

        if(space == Space.Self) // Convert back from global to local coordinates
        {
            transformDataValue.position = transform.InverseTransformPoint(transformDataValue.position);
            transformDataValue.rotation = Quaternion.Inverse(transform.rotation) * transformDataValue.rotation;
            transformDataValue.scale = Vector3Extentions.UnScale(transformDataValue.scale,transform.lossyScale);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move Handle");
            fieldInfo.SetValue(target,transformDataValue);
        }
    }

    private void ProcessLocationWidget(FieldInfo fieldInfo, Quaternion widgetRotation)
    {
        Vector3 vectorValue = GetLocationFromField(fieldInfo,target);
        
        if(space == Space.Self)
        {
            vectorValue = transform.TransformPoint(vectorValue);
        }

        // Draw the position handle
        EditorGUI.BeginChangeCheck();

        Vector3 newVectorValue;
        newVectorValue = Handles.PositionHandle(vectorValue, widgetRotation);



        if(space == Space.Self)
        {
            newVectorValue = transform.InverseTransformPoint(newVectorValue);
        }
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move Handle");
            SetLocationFromVector3(fieldInfo,target,newVectorValue);
        }
    }
    private void ProcessArrowWidget(ArrowToAttribute arrowAttribute, FieldInfo fieldInfo)
    {
        Vector3 startPoint = GetLocationFromField(fieldInfo,target);

        if(arrowAttribute.startPointSpace==Space.Self)
        {
            startPoint = transform.TransformPoint(startPoint);
        }

        // Get the end point using the name provided in the attribute
        FieldInfo endPointField = monoBehaviour.GetType().GetField(arrowAttribute.endPointName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (endPointField == null)
        {
            return;
        }
        Vector3 endPoint = GetLocationFromField(endPointField, target);

        if(arrowAttribute.endPointSpace==Space.Self && !arrowAttribute.isRelativeEndPoint)
        {
            endPoint = transform.TransformPoint(endPoint);
        }
        else if(arrowAttribute.endPointSpace==Space.World && arrowAttribute.isRelativeEndPoint)
        {
            endPoint = startPoint+endPoint;
        }
        else if(arrowAttribute.endPointSpace==Space.Self && arrowAttribute.isRelativeEndPoint)
        {
            endPoint = startPoint+transform.TransformVector(endPoint);
        }
        DrawArrow(startPoint, endPoint);        
    }
    private void ProcessRectWidget(FieldInfo fieldInfo, Quaternion widgetRotation)
    {


        Rect rect = (Rect)fieldInfo.GetValue(target);
        float size = HandleUtility.GetHandleSize(Vector3.zero) / 8;
        Handles.CapFunction capFunction= Handles.CubeHandleCap;
        Vector3 snap = Vector3.zero;
        // Convert 2D rect into 3D points
        Vector3 center = rect.center;
        Vector3 topLeft = new Vector3(rect.xMin, rect.yMax, 0);
        Vector3 topRight = rect.max;
        Vector3 bottomLeft = rect.min;
        Vector3 bottomRight = new Vector3(rect.xMax, rect.yMin, 0);
        Vector3 left = new Vector3(rect.xMin, center.y, 0);
        Vector3 right = new Vector3(rect.xMax, center.y, 0);
        Vector3 top = new Vector3(center.x, rect.yMax, 0);
        Vector3 bottom = new Vector3(center.x, rect.yMin, 0);
        float width =  rect.width;
        float height = rect.height;
        Vector3 scale = new Vector3(width,height,1);

        // Convert to world space if needed.
        if (space == Space.Self)
        {
            center =  transform.TransformPoint(center);
            topLeft = transform.TransformPoint(topLeft);
            topRight = transform.TransformPoint(topRight);
            bottomLeft = transform.TransformPoint(bottomLeft);
            bottomRight = transform.TransformPoint(bottomRight);
            left = transform.TransformPoint(left);
            right = transform.TransformPoint(right);
            top = transform.TransformPoint(top);
            bottom = transform.TransformPoint(bottom);
            width *= transform.localScale.x;
            height *= transform.localScale.y;
        }
        Handles.color = handleColor;
        // Draw and move the handles
        EditorGUI.BeginChangeCheck();
        Vector3 newCenter = Handles.FreeMoveHandle(center, size*2, snap, Handles.CircleHandleCap);
        Vector3 newTopLeft = Handles.FreeMoveHandle(topLeft, size, snap, capFunction);
        Vector3 newTopRight = Handles.FreeMoveHandle(topRight, size, snap, capFunction);
        Vector3 newBottomRight = Handles.FreeMoveHandle(bottomRight, size, snap, capFunction);
        Vector3 newBottomLeft = Handles.FreeMoveHandle(bottomLeft, size, snap, capFunction);
        Vector3 newLeft = Handles.FreeMoveHandle(left, size, snap, capFunction);
        Vector3 newRight = Handles.FreeMoveHandle(right, size, snap, capFunction);
        Vector3 newTop = Handles.FreeMoveHandle(top, size, snap, capFunction);
        Vector3 newBottom = Handles.FreeMoveHandle(bottom, size, snap, capFunction);
        Vector3 newScale = Handles.ScaleHandle(new Vector3(width, height, 1f), center, space == Space.World ? Quaternion.identity : transform.rotation, size * 5f);


        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(monoBehaviour, "Modify Rect");

            if (newCenter != center)
            {
                rect.center = space == Space.World ? newCenter : transform.InverseTransformPoint(newCenter);
            }
            else if (newTopRight != topRight)
            {
                rect.max = space == Space.World ? newTopRight : transform.InverseTransformPoint(newTopRight);
            }
            else if (newBottomLeft != bottomLeft)
            {
                rect.min = space == Space.World ? newBottomLeft : transform.InverseTransformPoint(newBottomLeft);
            }
            else if (newTop != top)
            {
                rect.yMax = space == Space.World ? newTop.y : transform.InverseTransformPoint(newTop).y;
            }
            else if (newBottom != bottom)
            {
                rect.yMin = space == Space.World ? newBottom.y : transform.InverseTransformPoint(newBottom).y;
            }
            else if (newLeft != left)
            {
                rect.xMin = space == Space.World ? newLeft.x : transform.InverseTransformPoint(newLeft).x;
            }
            else if (newRight != right)
            {
                rect.xMax = space == Space.World ? newRight.x : transform.InverseTransformPoint(newRight).x;
            }
            else if (newTopLeft != topLeft)
            {
                Vector3 localTopLeft = space == Space.World ? newTopLeft : transform.InverseTransformPoint(newTopLeft);
                rect.xMin = localTopLeft.x;
                rect.yMax = localTopLeft.y;
            }
            else if (newBottomRight != bottomRight)
            {
                Vector3 localBottomRight = space == Space.World ? newBottomRight : transform.InverseTransformPoint(newBottomRight);
                rect.xMax = localBottomRight.x;
                rect.yMin = localBottomRight.y;
            }
            else if (newScale!= scale)
            {
                Vector2 tempCenter = rect.center;
                rect.height = space == Space.World ? newScale.y : Vector3Extentions.UnScale(newScale,transform.localScale).y;
                rect.width = space == Space.World ? newScale.x : Vector3Extentions.UnScale(newScale, transform.localScale).x;

                rect.center = tempCenter;
            }

            //continue
            fieldInfo.SetValue(target, rect);
        }

        // Draw the rect outline in the scene view
        Handles.color = fillColor;
        Handles.DrawSolidRectangleWithOutline(new Vector3[] { topLeft, topRight, bottomRight, bottomLeft }, Color.white,Color.black);
        
    }
    private void ProcessLabelWidget(LabelAttribute labelAttribute, FieldInfo fieldInfo)
    {
        Space labelSpace = labelAttribute.space?? space;
        Vector3 point = GetLocationFromField(fieldInfo,target);

        if(labelSpace == Space.Self){
            point = transform.TransformPoint(point);
        }

        string name;
        if(labelAttribute.labelName!= null)
        {
            name = labelAttribute.labelName;
        }
        else if(fieldInfo.GetValue(target) is ILabelName field)
        {
            name = field.GetNameForLabel();
        }
        else
        {
            name = fieldInfo.Name;
        }
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.normal.background = labelBackgroundTexture;
        style.normal.textColor = labelTextColor; 
        Handles.Label(point + labelScreenOffset * HandleUtility.GetHandleSize(point), new GUIContent(name), style);
    }

    #region Helper Methods

    private void DrawArrow(Vector3 startPoint, Vector3 endPoint)
    {
        Handles.color = lineColor;
        Vector3 direction = endPoint - startPoint;
        Handles.DrawLine(startPoint, endPoint, thickness);

        // Arrow Head Start Point half way.
        Vector3 arrowHeadBase = startPoint + direction/2;

        Handles.ConeHandleCap(0,arrowHeadBase,Quaternion.LookRotation(direction,Vector3.up),HandleUtility.GetHandleSize(arrowHeadBase)/5,EventType.Repaint);
    }


    private Quaternion GetQuaternionFromField(FieldInfo fieldInfo, object target,bool bottom = false)
    {
        object value = fieldInfo.GetValue(target);

        switch (value)
        {
            case IWidgetRotation field:
                return field.GetRotationForWidget();

            case Quaternion field:
                return field;

            default:
                throw new InvalidOperationException($"Unsupported field type: {value.GetType()} for field {fieldInfo.Name}");
        }
    }
    private void SetFieldFromQuaternion(FieldInfo fieldInfo, object target, Quaternion rotation)
    {
        object value = fieldInfo.GetValue(target);

        switch (value)
        {
            case IWidgetRotation _IWidgetRotation:
                _IWidgetRotation.SetRotationfromWidget(rotation);
                fieldInfo.SetValue(target, _IWidgetRotation);
                break;

            case Quaternion:
                fieldInfo.SetValue(target, rotation);
                break;
            default:
                throw new InvalidOperationException($"Unsupported field type: {value.GetType()} for field {fieldInfo.Name}");
        }
    }
    private Vector3 GetLocationFromField(FieldInfo fieldInfo, object target,bool bottom = false)
    {
        object value = fieldInfo.GetValue(target);

        switch (value)
        {
            case IWidgetLocation field:
                return field.GetLocationForWidget();

            case Vector3 vec3:
                return vec3;

            case Vector2 vec2:
                return (Vector3)vec2;

            case float2 float2:
                return new Vector3(float2.x, float2.y, 0f);

            case float3 float3:
                return (Vector3)float3;

            case Vector3Int vec3int:
                return (Vector3)vec3int;

            case Vector2Int vec2int:
                return (Vector3)(Vector2)vec2int;

            case Rect rect:
                return bottom? new Vector3(rect.x,rect.yMin,0) : (Vector3)rect.center;

            default:
                throw new InvalidOperationException($"Unsupported field type: {value.GetType()} for field {fieldInfo.Name}");
        }
    }
    private void SetLocationFromVector3(FieldInfo fieldInfo, object target, Vector3 vector)
    {
        object value = fieldInfo.GetValue(target);

        switch (value)
        {
            case IWidgetLocation _IWidgtLocation:
                _IWidgtLocation.SetLocationFromWidget(vector);
                fieldInfo.SetValue(target, _IWidgtLocation);
                break;

            case Vector3:
                fieldInfo.SetValue(target, vector);
                break;

            case Vector2:
                fieldInfo.SetValue(target, (Vector2)vector);
                break;

            case float2:
                float2 float2Value = new float2(vector.x, vector.y);
                fieldInfo.SetValue(target, float2Value);
                break;

            case float3:
                fieldInfo.SetValue(target, (float3)vector);
                break;

            case Vector3Int vec3int:
                vec3int.x = Mathf.RoundToInt(vector.x);
                vec3int.y = Mathf.RoundToInt(vector.y);
                vec3int.z = Mathf.RoundToInt(vector.z);
                fieldInfo.SetValue(target, vec3int);
                break;

            case Vector2Int vec2int:
                vec2int.x = Mathf.RoundToInt(vector.x);
                vec2int.y = Mathf.RoundToInt(vector.y);
                fieldInfo.SetValue(target, vec2int);
                break;

            default:
                throw new InvalidOperationException($"Unsupported field type: {value.GetType()} for field {fieldInfo.Name}");
        }
    }

    private Texture2D CreateSolidColorTexture(Color color)
    {
        Texture2D texture;
        if (labelBackgroundTextureCache.TryGetValue(color,out texture))
        {
            return texture;
        }
        else
        {
            texture = new Texture2D(1, 1);
            texture.SetPixel(0,0,color);
            texture.Apply();
            labelBackgroundTextureCache[color] = texture;
            return texture;
        }

    }

    #endregion
}