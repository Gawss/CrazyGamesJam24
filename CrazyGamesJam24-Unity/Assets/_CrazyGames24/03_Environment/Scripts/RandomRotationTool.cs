#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class RandomRotationTool : EditorWindow
{
    private float minYRotation = 0f;
    private float maxYRotation = 360f;

    [MenuItem("Tools/Randomize Tree Rotation")]
    public static void ShowWindow()
    {
        GetWindow<RandomRotationTool>("Random Rotation Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Random Rotation Settings", EditorStyles.boldLabel);

        minYRotation = EditorGUILayout.FloatField("Min Y Rotation", minYRotation);
        maxYRotation = EditorGUILayout.FloatField("Max Y Rotation", maxYRotation);

        if (GUILayout.Button("Apply Random Rotation"))
        {
            ApplyRandomRotation();
        }
    }

    private void ApplyRandomRotation()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj.transform, "Random Rotate");

            float randomYRotation = Random.Range(minYRotation, maxYRotation);
            float roundedYRotation = Mathf.Round(randomYRotation / 90f) * 90f;

            Vector3 currentRotation = obj.transform.eulerAngles;
            obj.transform.eulerAngles = new Vector3(currentRotation.x, roundedYRotation, currentRotation.z);
        }
    }
}
#endif