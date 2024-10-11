using UnityEngine;
using UnityEditor;
using System.Reflection;
using WidgetAttributes;
using WidgetAttributesUtillities;
using System;

[CustomEditor(typeof(MonoBehaviour), true)]
public class WidgetAttributesEditor : Editor
{
    MonoBehaviour monoBehaviour;
    FieldInfo[] fieldInfos;
    Space space = Space.World;

    #region Style
    private float thickness = 1;
    private Color lineColor = new Color(1,0,0,1);
    private Color fillColor = new Color(1,0,0,0.25f);  
    private Color labelBackgroundColor = new Color(1,1,1,1);
    private Color labelTextColor = new Color(0,0,0,1);
    private Color handleColor = new Color(1,1,0,1);
    private Vector3 labelScreenOffset = new Vector3(0,-0.25f,0);

    #endregion
    private void OnSceneGUI()
    {
        //Apply default style
        lineColor = new Color(1,0,0,1);
        fillColor = new Color(1,0,0,0.25f);
        labelBackgroundColor = new Color(1,1,1,1);
        labelTextColor = new Color(0,0,0,1);
        handleColor = new Color(1,1,0,1);
        labelScreenOffset = new Vector3(0,-0.25f,0);


        // Get all fields in the MonoBehaviour type
        fieldInfos = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        monoBehaviour = (MonoBehaviour)target;
        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            space = Space.World;
            ProcessStyleAttributes(fieldInfo);
            ProcessFieldAttributes(fieldInfo);
        }
    }

    private void ProcessStyleAttributes(FieldInfo fieldInfo)
    {
        ThicknessAttribute thicknessAttribute = fieldInfo.GetCustomAttribute<ThicknessAttribute>();
        if (thicknessAttribute != null)
        {
            thickness = thicknessAttribute.thickness;
        }
        FillColorAttribute fillColorAttribute = fieldInfo.GetCustomAttribute<FillColorAttribute>();
        if (fillColorAttribute != null)
        {
            fillColor = new Color(fillColorAttribute.red, fillColorAttribute.green, fillColorAttribute.blue, fillColorAttribute.alpha);
        }
        LineColorAttribute lineColorAttribute = fieldInfo.GetCustomAttribute<LineColorAttribute>();
        if (lineColorAttribute != null)
        {
            lineColor = new Color(lineColorAttribute.red, lineColorAttribute.green, lineColorAttribute.blue, lineColorAttribute.alpha);
        }
        LabelBackgroundColorAttribute labelBackgroundColorAttribute = fieldInfo.GetCustomAttribute<LabelBackgroundColorAttribute>();
        if (labelBackgroundColorAttribute != null)
        {
            labelBackgroundColor = new Color(labelBackgroundColorAttribute.red, labelBackgroundColorAttribute.green, labelBackgroundColorAttribute.blue, labelBackgroundColorAttribute.alpha);
        }
        LabelTextColorAttribute labelTextColorAttribute = fieldInfo.GetCustomAttribute<LabelTextColorAttribute>();
        if (labelTextColorAttribute != null)
        {
            labelTextColor = new Color(labelTextColorAttribute.red, labelTextColorAttribute.green, labelTextColorAttribute.blue, labelTextColorAttribute.alpha);
        }
        LabelScreenOffsetAttribute labelScreenOffsetAttribute = fieldInfo.GetCustomAttribute<LabelScreenOffsetAttribute>();
        if (labelScreenOffsetAttribute != null)
        {
            labelScreenOffset = new Vector3(labelScreenOffsetAttribute.x, labelScreenOffsetAttribute.y, labelScreenOffsetAttribute.z);
        }
    }
    private void ProcessFieldAttributes(FieldInfo fieldInfo)
    {
        
        WidgetAttribute widgetAttribute = fieldInfo.GetCustomAttribute<WidgetAttribute>();
        space = widgetAttribute.space;
        Transform transform =  (target as MonoBehaviour)?.transform;
        Quaternion widgetRotation = space == Space.Self && Tools.pivotRotation==PivotRotation.Local? transform.rotation : Quaternion.identity;

        if(widgetAttribute != null)
        {
            if(fieldInfo.FieldType == typeof(Vector3) || fieldInfo.FieldType == typeof(Vector2))
            {
                ProcessLocationWidget(fieldInfo,widgetRotation);
            } 

            else if(fieldInfo.FieldType == typeof(Rect))
            {
                ProcessRectWidget(fieldInfo,widgetRotation);
            }
        }
        ArrowToAttribute arrowAttribute = fieldInfo.GetCustomAttribute<ArrowToAttribute>();
        if (arrowAttribute != null)
        {
            if(fieldInfo.FieldType == typeof(Vector3))
            {
                ProcessArrowWidget(arrowAttribute,fieldInfo);
            }
        }
        LabelAttribute labelAttribute = fieldInfo.GetCustomAttribute<LabelAttribute>();
        if (labelAttribute != null)
        {
            if(fieldInfo.FieldType == typeof(Vector3))
            {
                ProcessLabelWidget(labelAttribute,fieldInfo);
            }
        }        

    }
  

    private void ProcessLocationWidget(FieldInfo fieldInfo, Quaternion widgetRotation)
    {
        Vector3 vectorValue;
        if(fieldInfo.FieldType == typeof(Vector3))
        {
            vectorValue = (Vector3)fieldInfo.GetValue(target);
        } 
        else if(fieldInfo.FieldType == typeof(Vector2))
        {
            vectorValue = (Vector2)fieldInfo.GetValue(target);
        } 
        else 
        {
            return;
        }
        Transform transform =  (target as MonoBehaviour)?.transform;
        
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
            newVectorValue = (target as MonoBehaviour).transform.InverseTransformPoint(newVectorValue);
        }
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move Handle");
            if(fieldInfo.FieldType == typeof(Vector3))
            {
                fieldInfo.SetValue(target, newVectorValue);
            } 
            else if(fieldInfo.FieldType == typeof(Vector2))
            {
                fieldInfo.SetValue(target, (Vector2)newVectorValue);
            }
        }
    }
    private void ProcessArrowWidget(ArrowToAttribute arrowAttribute, FieldInfo fieldInfo)
    {
        Vector3 startPoint = (Vector3)fieldInfo.GetValue(monoBehaviour);
        if(arrowAttribute.startPointSpace==Space.Self)
        {
            startPoint = monoBehaviour.transform.TransformPoint(startPoint);
        }

        // Get the end point using the name provided in the attribute
        FieldInfo endPointField = monoBehaviour.GetType().GetField(arrowAttribute.endPointName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (endPointField == null || endPointField.FieldType != typeof(Vector3))
        {
            return;
        }
        Vector3 endPoint = (Vector3)endPointField.GetValue(monoBehaviour);
        if(arrowAttribute.endPointSpace==Space.Self && !arrowAttribute.isRelativeEndPoint)
        {
            endPoint = monoBehaviour.transform.TransformPoint(endPoint);
        }
        else if(arrowAttribute.endPointSpace==Space.World && arrowAttribute.isRelativeEndPoint)
        {
            endPoint = startPoint+endPoint;
        }
        else if(arrowAttribute.endPointSpace==Space.Self && arrowAttribute.isRelativeEndPoint)
        {
            endPoint = startPoint+monoBehaviour.transform.TransformVector(endPoint);
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
            center =  monoBehaviour.transform.TransformPoint(center);
            topLeft = monoBehaviour.transform.TransformPoint(topLeft);
            topRight = monoBehaviour.transform.TransformPoint(topRight);
            bottomLeft = monoBehaviour.transform.TransformPoint(bottomLeft);
            bottomRight = monoBehaviour.transform.TransformPoint(bottomRight);
            left = monoBehaviour.transform.TransformPoint(left);
            right = monoBehaviour.transform.TransformPoint(right);
            top = monoBehaviour.transform.TransformPoint(top);
            bottom = monoBehaviour.transform.TransformPoint(bottom);
            width *= monoBehaviour.transform.localScale.x;
            height *= monoBehaviour.transform.localScale.y;
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
        Vector3 newScale = Handles.ScaleHandle(new Vector3(width, height, 1f), center, space == Space.World ? Quaternion.identity : monoBehaviour.transform.rotation, size * 5f);


        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(monoBehaviour, "Modify Rect");

            if (newCenter != center)
            {
                rect.center = space == Space.World ? newCenter : monoBehaviour.transform.InverseTransformPoint(newCenter);
            }
            else if (newTopRight != topRight)
            {
                rect.max = space == Space.World ? newTopRight : monoBehaviour.transform.InverseTransformPoint(newTopRight);
            }
            else if (newBottomLeft != bottomLeft)
            {
                rect.min = space == Space.World ? newBottomLeft : monoBehaviour.transform.InverseTransformPoint(newBottomLeft);
            }
            else if (newTop != top)
            {
                rect.yMax = space == Space.World ? newTop.y : monoBehaviour.transform.InverseTransformPoint(newTop).y;
            }
            else if (newBottom != bottom)
            {
                rect.yMin = space == Space.World ? newBottom.y : monoBehaviour.transform.InverseTransformPoint(newBottom).y;
            }
            else if (newLeft != left)
            {
                rect.xMin = space == Space.World ? newLeft.x : monoBehaviour.transform.InverseTransformPoint(newLeft).x;
            }
            else if (newRight != right)
            {
                rect.xMax = space == Space.World ? newRight.x : monoBehaviour.transform.InverseTransformPoint(newRight).x;
            }
            else if (newTopLeft != topLeft)
            {
                Vector3 localTopLeft = space == Space.World ? newTopLeft : monoBehaviour.transform.InverseTransformPoint(newTopLeft);
                rect.xMin = localTopLeft.x;
                rect.yMax = localTopLeft.y;
            }
            else if (newBottomRight != bottomRight)
            {
                Vector3 localBottomRight = space == Space.World ? newBottomRight : monoBehaviour.transform.InverseTransformPoint(newBottomRight);
                rect.xMax = localBottomRight.x;
                rect.yMin = localBottomRight.y;
            }
            else if (newScale!= scale)
            {
                Vector2 tempCenter = rect.center;
                rect.height = space == Space.World ? newScale.y : Vector3Extentions.UnScale(newScale,monoBehaviour.transform.localScale).y;
                rect.width = space == Space.World ? newScale.x : Vector3Extentions.UnScale(newScale, monoBehaviour.transform.localScale).x;

                rect.center = tempCenter;
            }

            //continue
            fieldInfo.SetValue(monoBehaviour, rect);
        }

        // Draw the rect outline in the scene view
        Handles.color = fillColor;
        Handles.DrawSolidRectangleWithOutline(new Vector3[] { topLeft, topRight, bottomRight, bottomLeft }, Color.white,Color.black);
        
    }
    private void ProcessLabelWidget(LabelAttribute labelAttribute, FieldInfo fieldInfo)
    {
        Space labelSpace = labelAttribute.space?? space;
        Vector3 point = (Vector3)fieldInfo.GetValue(target);

        if(labelSpace == Space.Self){
            point = monoBehaviour.transform.TransformPoint(point);
        }

        string name = labelAttribute.labelName?? fieldInfo.Name;
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.normal.background = Texture2D.whiteTexture;
        style.normal.textColor = Color.black; 
        Handles.Label(point + labelScreenOffset * HandleUtility.GetHandleSize(point), new GUIContent(name), style);
    }
    
    private void DrawArrow(Vector3 startPoint, Vector3 endPoint)
    {
        Handles.color = lineColor;
        Vector3 direction = endPoint - startPoint;
        Handles.DrawLine(startPoint, endPoint, thickness);

        // Arrow Head Start Point half way.
        Vector3 arrowHeadBase = startPoint + direction/2;
        float sizeModifier = HandleUtility.GetHandleSize(arrowHeadBase)/3;
        
        // Arrow Head End Points
        Vector3 arrowHeadLeft = Quaternion.Euler(0, 0, 135) * direction.normalized * sizeModifier;
        Vector3 arrowHeadRight = Quaternion.Euler(0, 0, 225) * direction.normalized * sizeModifier;

        // Draw arrowhead
        Handles.DrawLine(arrowHeadBase, arrowHeadBase + arrowHeadLeft, thickness);
        Handles.DrawLine(arrowHeadBase, arrowHeadBase + arrowHeadRight, thickness);
    }
}