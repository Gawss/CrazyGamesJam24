#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
public class RandomTreeSelector : EditorWindow
{
    private string treeNameFilter = "Tree"; // Default filter
    private int selectionCount = 5; // Default number of trees to randomly select

    [MenuItem("Tools/Random Tree Selector")]
    public static void ShowWindow()
    {
        GetWindow<RandomTreeSelector>("Random Tree Selector");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Random Trees", EditorStyles.boldLabel);

        // Input field for tree name filter
        treeNameFilter = EditorGUILayout.TextField("Tree Name Filter", treeNameFilter);

        // Input field for number of trees to select
        selectionCount = EditorGUILayout.IntField("Number of Trees to Select", selectionCount);

        if (GUILayout.Button("Select Random Trees"))
        {
            SelectRandomTrees();
        }
    }

    private void SelectRandomTrees()
    {
        // Find all objects in the scene that contain the name filter
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> filteredTrees = allObjects
            .Where(obj => obj.name.Contains(treeNameFilter))
            .ToList();

        // Check if there are enough trees to select from
        if (filteredTrees.Count == 0)
        {
            Debug.LogWarning("No trees found with the specified name filter.");
            return;
        }

        // Adjust the selection count if it's higher than available trees
        int actualSelectionCount = Mathf.Min(selectionCount, filteredTrees.Count);

        // Select a random subset of trees
        List<GameObject> selectedTrees = filteredTrees
            .OrderBy(_ => Random.value)
            .Take(actualSelectionCount)
            .ToList();

        // Highlight the selected trees in the editor
        Selection.objects = selectedTrees.ToArray();

        Debug.Log($"Selected {actualSelectionCount} random trees with filter '{treeNameFilter}'.");
    }
}
#endif