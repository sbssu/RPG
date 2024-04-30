using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneEditor : Editor
{
    // MenuItem�� ��ο� ditorSceneManager.OpenScene�� ��δ� �ڽ��� Scene ���� �̸����� �����Ѵ�.
    [MenuItem("SceneMove/To Login &1", priority = 1)]
    private static void Scene1()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/1.Login.unity");
    }

    [MenuItem("SceneMove/To Game &2", priority = 2)]
    private static void Scene2()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/2.Game.unity");
    }

    [MenuItem("Selection/To Resources &#1", priority = 1)]
    private static void SelectResources()
    {
        string path = "Assets/Resources";

        // Load object
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));

        // Select the object in the project folder
        Selection.activeObject = obj;

        // Also flash the folder yellow to highlight it
        EditorGUIUtility.PingObject(obj);
    }
}
