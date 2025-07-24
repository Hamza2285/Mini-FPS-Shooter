using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Target))]
public class TargetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Rename local variable to avoid conflict with inherited 'target'
        Target tgt = (Target)target;

        if (GUILayout.Button("Auto-Fill Ragdoll Parts"))
        {
            tgt.ragdollBodies = tgt.GetComponentsInChildren<Rigidbody>();
            tgt.ragdollColliders = tgt.GetComponentsInChildren<Collider>();

            EditorUtility.SetDirty(tgt);
        }
    }
}
