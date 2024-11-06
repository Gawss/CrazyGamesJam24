#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class RandomOffsetTool : EditorWindow
{
    private enum Axis { X, Y, Z } // Enum to select the axis
    private Axis selectedAxis = Axis.Y; // Default axis is Y
    private float minOffset = -1f; // Minimum offset
    private float maxOffset = 1f;  // Maximum offset

    [MenuItem("Tools/Randomize Offset")]
    public static void ShowWindow()
    {
        GetWindow<RandomOffsetTool>("Random Offset Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Random Offset Settings", EditorStyles.boldLabel);

        // Dropdown to select the axis
        selectedAxis = (Axis)EditorGUILayout.EnumPopup("Select Axis", selectedAxis);

        // Input fields for min and max offset
        minOffset = EditorGUILayout.FloatField("Min Offset", minOffset);
        maxOffset = EditorGUILayout.FloatField("Max Offset", maxOffset);

        if (GUILayout.Button("Apply Random Offset"))
        {
            ApplyRandomOffset();
        }
    }

    private void ApplyRandomOffset()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            // Record the object's transformation for undo functionality
            Undo.RecordObject(obj.transform, "Random Offset");

            // Generate a random offset within the specified range
            float randomOffset = Random.Range(minOffset, maxOffset);

            // Apply the random offset to the selected axis
            Vector3 currentPosition = obj.transform.position;
            switch (selectedAxis)
            {
                case Axis.X:
                    obj.transform.position = new Vector3(currentPosition.x + randomOffset, currentPosition.y, currentPosition.z);
                    break;
                case Axis.Y:
                    obj.transform.position = new Vector3(currentPosition.x, currentPosition.y + randomOffset, currentPosition.z);
                    break;
                case Axis.Z:
                    obj.transform.position = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + randomOffset);
                    break;
            }
        }
    }
}
#endif