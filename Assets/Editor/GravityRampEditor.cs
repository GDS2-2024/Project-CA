using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GravityRamp))]
public class GravityRampEditor : Editor
{
    private void OnSceneGUI()
    {
        GravityRamp ramp = (GravityRamp)target;
        Transform rampTransform = ramp.transform;

        // Convert GravityForce from local space to world space
        Vector3 worldForce = rampTransform.TransformPoint(ramp.GravityForce);

        // Create a handle for manipulating GravityForce
        EditorGUI.BeginChangeCheck();
        Vector3 newWorldForce = Handles.PositionHandle(worldForce, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(ramp, "Move Gravity Force Handle");
            ramp.GravityForce = rampTransform.InverseTransformPoint(newWorldForce);
            EditorUtility.SetDirty(ramp);
        }

        // Draw arrow representation of force
        Handles.color = Color.cyan;
        Handles.DrawLine(rampTransform.position, worldForce, 3.0f);
        Handles.ConeHandleCap(0, worldForce, Quaternion.LookRotation(worldForce - rampTransform.position), 1.0f, EventType.Repaint);
    }
}
