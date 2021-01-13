using UnityEditor;

using UnityEngine;

public class GetObjectDependencies : EditorWindow
{
    public static GetObjectDependencies instance;

    private Object _obj = null;
    private string _objPath = "";

    private string[] _referenceAssets = new string[0];

    private Vector2 _scroll = Vector2.zero;



    [MenuItem("Tools/Dependencies/Get Object Dependencies")]
    public static void GetWindow()
    {
        if (instance != null) return;
        instance = InitWindow();
    }

    private static GetObjectDependencies InitWindow()
    {
        GetObjectDependencies window = (GetObjectDependencies)EditorWindow.GetWindow(typeof(GetObjectDependencies));
        window.Show();

        return window;
    }

    public static void UpdateDependencies(Object obj)
    {
        GetWindow();
        instance.GetDependencies(obj);
    }


    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Object:");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _obj = EditorGUILayout.ObjectField(_obj, typeof(Object), true);

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Update", GUILayout.Width(50f)))
        {
            GetDependenciesPaths();
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndHorizontal();

        /*EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(_asset);
        EditorGUILayout.EndHorizontal();*/

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        string label = "Dependencies:";
        if (_referenceAssets.Length > 1)
        {
            label = string.Concat("Dependencies (" + _referenceAssets.Length + "):");
        }
        EditorGUILayout.LabelField(label);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (_referenceAssets.Length < 1)
        {
            ShowNotification(new GUIContent("No dependencies were found"));
        }

        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        foreach (string asset in _referenceAssets)
        {
            EditorGUILayout.BeginHorizontal();

            Object refAsset = LoadAssetByPath(asset);
            string refMeta = string.Concat(asset, ".meta");

#pragma warning disable CS0618
            EditorGUILayout.ObjectField(refAsset, typeof(Object));
#pragma warning restore CS0618

            //GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Dep", GUILayout.Width(40f)))
            {
                UpdateDependencies(refAsset);
            }
            //GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    private void GetDependencies(Object obj)
    {
        _obj = obj;
        GetDependenciesPaths();
    }

    private void GetDependenciesPaths()
    {
        _objPath = AssetDatabase.GetAssetPath(_obj);

        _referenceAssets = AssetDatabase.GetDependencies(_objPath, true);

        System.Array.Sort(_referenceAssets);

        _referenceAssets = System.Array.FindAll(_referenceAssets, RemoveItSelf);

        ShowNotification(new GUIContent("UPDATED"));
    }

    private bool RemoveItSelf(string path)
    {
        return _objPath != path;
    }

    private Object LoadAssetByPath(string assetPath)
    {
        if (string.IsNullOrEmpty(assetPath)) return null;

        return AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object)) as Object;
    }

}