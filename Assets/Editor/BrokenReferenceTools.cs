using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BrokenReferenceTools
{
    [MenuItem("Tools/Broken References/Report In Open Scene")]
    public static void ReportInOpenScene()
    {
        var scene = SceneManager.GetActiveScene();
        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogError("No loaded scene. Open your scene first.");
            return;
        }

        int missingScripts = 0;
        int brokenRefs = 0;

        foreach (var go in scene.GetRootGameObjects())
        {
            foreach (var t in go.GetComponentsInChildren<Transform>(true))
            {
                var comps = t.GetComponents<Component>();
                foreach (var comp in comps)
                {
                    if (comp == null)
                    {
                        missingScripts++;
                        Debug.LogWarning($"Missing script on: {GetHierarchyPath(t.gameObject)}");
                        continue;
                    }

                    var so = new SerializedObject(comp);
                    var sp = so.GetIterator();
                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            // Broken ref: ID stored, but no object resolved
                            if (sp.objectReferenceInstanceIDValue != 0 && sp.objectReferenceValue == null)
                            {
                                brokenRefs++;
                                Debug.LogWarning($"Broken reference on {GetHierarchyPath(t.gameObject)} -> {comp.GetType().Name}.{sp.displayName}");
                            }
                        }
                    }
                }
            }
        }

        Debug.Log($"Report complete. Missing scripts: {missingScripts}, broken object fields: {brokenRefs}.");
    }

    [MenuItem("Tools/Broken References/Auto-Fix In Open Scene")]
    public static void AutoFixInOpenScene()
    {
        var scene = SceneManager.GetActiveScene();
        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogError("No loaded scene. Open your scene first.");
            return;
        }

        int removedMissing = 0;
        int clearedRefs = 0;

        foreach (var go in scene.GetRootGameObjects())
        {
            foreach (var t in go.GetComponentsInChildren<Transform>(true))
            {
                // Remove missing scripts
                int before = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject);
                if (before > 0)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(t.gameObject);
                    removedMissing += before;
                }

                // Clear broken object references
                var comps = t.GetComponents<Component>();
                foreach (var comp in comps)
                {
                    if (comp == null) continue;

                    var so = new SerializedObject(comp);
                    bool modified = false;

                    var sp = so.GetIterator();
                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            if (sp.objectReferenceInstanceIDValue != 0 && sp.objectReferenceValue == null)
                            {
                                sp.objectReferenceValue = null; // clear broken PPtr
                                clearedRefs++;
                                modified = true;
                            }
                        }
                    }

                    if (modified)
                    {
                        so.ApplyModifiedPropertiesWithoutUndo();
                        EditorUtility.SetDirty(comp);
                    }
                }
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log($"Auto-fix complete. Removed missing scripts: {removedMissing}, cleared broken fields: {clearedRefs}.");
    }

    private static string GetHierarchyPath(GameObject go)
    {
        var stack = new Stack<string>();
        var t = go.transform;
        while (t != null)
        {
            stack.Push(t.name);
            t = t.parent;
        }
        return string.Join("/", stack);
    }
}