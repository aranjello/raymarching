using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(shapeControl))]
public class shapeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        shapeControl shape = (shapeControl)target;
        //EditorGUIUtility.labelWidth = 50;
        shape.shapeType = (shapeControl.shapes)EditorGUILayout.EnumPopup("Shape",shape.shapeType);
        shape.operation = (shapeControl.op)EditorGUILayout.EnumPopup("Operation", shape.operation);
        shape.c = EditorGUILayout.ColorField("color",shape.c);
        shape.rad = EditorGUILayout.Vector3Field("rad", shape.rad);
        if(shape.operation == shapeControl.op.blend)
        {
            shape.blendensity = EditorGUILayout.Slider("blend density", shape.blendensity, 0, 100);

        }
        shape.stretch = EditorGUILayout.Vector3Field("stretch", shape.stretch);
        shape.seeThrough = EditorGUILayout.Toggle("glass", shape.seeThrough);
        if (shape.seeThrough)
        {
            shape.refractive = EditorGUILayout.Slider("refraction", shape.refractive, 0, 1);
        }
        else
        {
            shape.roundness = EditorGUILayout.Slider("roundness", shape.roundness, 0, 10);
        }
        shape.repeat = EditorGUILayout.Toggle("repeating", shape.repeat);
        if (shape.repeat)
        {
            shape.xRep = EditorGUILayout.IntField("xRep", shape.xRep);
            shape.yRep = EditorGUILayout.IntField("yRep", shape.yRep);
            shape.zRep = EditorGUILayout.IntField("zRep", shape.zRep);

            shape.repLength = EditorGUILayout.FloatField("rep length", shape.repLength);
        }
        if (shape.transform.childCount> 0)
        {
            shape.locked = EditorGUILayout.Toggle("Lock Children", shape.locked);
        }
        shape.shell = EditorGUILayout.Toggle("Shell",shape.shell);
        shape.Update();
    }
}
