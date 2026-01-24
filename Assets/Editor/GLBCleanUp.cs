using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CleanupGLB : EditorWindow
{
    [MenuItem("Tools/Cleanup GLB Materials")]
    public static void ShowWindow()
    {
        GetWindow<CleanupGLB>("GLB Cleaner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Delete objects with specific material", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Clean Selected GLB"))
        {
            CleanSelectedObject();
        }
    }

    private void CleanSelectedObject()
    {
        GameObject target = Selection.activeGameObject;

        if (target == null)
        {
            Debug.LogError("Please select the GLB object in the Hierarchy first.");
            return;
        }

        // We use a list to store objects to delete to avoid 
        // modifying the collection while iterating
        List<GameObject> toDelete = new List<GameObject>();
        MeshRenderer[] renderers = target.GetComponentsInChildren<MeshRenderer>(true);

        foreach (MeshRenderer renderer in renderers)
        {
            foreach (Material mat in renderer.sharedMaterials)
            {
                if (mat != null && mat.name == "edge_color000255")
                {
                    toDelete.Add(renderer.gameObject);
                    break; // Move to next renderer once a match is found
                }
            }
        }

        int count = toDelete.Count;
        foreach (GameObject obj in toDelete)
        {
            // Use DestroyImmediate because this is an Editor script
            Undo.DestroyObjectImmediate(obj);
        }

        Debug.Log($"Cleanup complete. Removed {count} objects.");
    }
}