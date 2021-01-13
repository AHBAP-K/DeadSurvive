using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;

public class SearchReferences : EditorWindow
{
    public static SearchReferences instance;

    private string _asset = null;
    private Object _obj = null;

    private List<string> _referenceAssets = new List<string>();

    private Vector2 _scroll = Vector2.zero;

    private int _totalAssets = 0;
    private int _currentAssets = 0;
    private float _lastTime = 0;

    [MenuItem("Tools/Dependencies/Search References")]
    public static void GetWindow()
    {
        if (instance != null) return;
        instance = InitWindow();
    }

    private static SearchReferences InitWindow()
    {
        EditorUtility.ClearProgressBar();

        SearchReferences window = (SearchReferences)EditorWindow.GetWindow(typeof(SearchReferences));
        window.Show();

        return window;
    }

    public static void UpdateReferences(Object obj, string asset)
    {
        GetWindow();
        instance.GetReferences(obj, asset);
    }

    private void GetReferences(Object obj, string asset)
    {
        _obj = obj;
        _asset = asset;

        GetWindow();

        instance._referenceAssets = new List<string>();

        if (_obj == null)
        {
            ShowNotification(new GUIContent("SELECT OBJECT FIRST"));
            return;
        }

        string guid = instance.GetAssetGUID(instance.LoadAsset(_asset));

        _scroll = Vector2.zero;

        _totalAssets = AssetDatabase.GetAllAssetPaths().Length;
        _currentAssets = 0;

        UpdateProgressBar();

        instance.GetAllAssetsWithGUID(guid, ref instance._referenceAssets);
        instance._referenceAssets.Sort();

        ShowNotification(new GUIContent("UPDATED"));

        EditorUtility.ClearProgressBar();
    }

    private void UpdateProgressBar()
    {
        // TIMER NEED TO INCREASE SPEED UP
        if (_lastTime + .1f > Time.realtimeSinceStartup) return;
        _lastTime = Time.realtimeSinceStartup;

        string label = string.Concat("Search ", " [", _currentAssets, " /  > ", _totalAssets, "]");

        EditorUtility.DisplayProgressBar("Searching", label, (float)_currentAssets / _totalAssets);
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
            string meta = string.Concat(AssetDatabase.GetAssetPath(_obj), ".meta");
            UpdateReferences(_obj, meta);
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("References:");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        foreach (string asset in _referenceAssets)
        {
            EditorGUILayout.BeginHorizontal();

            Object refAsset = LoadAssetByPath(asset);
            string refMeta = string.Concat(asset, ".meta");

#pragma warning disable CS0618
            EditorGUILayout.ObjectField(refAsset, typeof(Object));
#pragma warning restore CS0618

            if (GUILayout.Button("Refs", GUILayout.Width(40f)))
            {
                UpdateReferences(refAsset, refMeta);
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }
    private string GetAssetGUID(string metaData)
    {
        if (string.IsNullOrEmpty(_asset)) return null;

        string[] lines = metaData.Split('\n');

        string guid = "";
        foreach (string line in lines)
        {
            if (line.Contains("guid:"))
            {
                guid = line.Split(' ')[1]; // GET VALUE FORM ROW
                break;
            }
        }
        return guid.Trim();
    }

    private Object LoadAssetByPath(string assetPath)
    {
        if (string.IsNullOrEmpty(assetPath)) return null;

        return AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object)) as Object;
    }

    private string LoadAsset(string assetPath)
    {

        if (string.IsNullOrEmpty(assetPath)) return null;

        string metaData = "";

        using (StreamReader metaReader = new StreamReader(assetPath))
        {
            metaData = metaReader.ReadToEnd();
        }

        return metaData;
    }
    private void GetAllAssetsWithGUID(string guid, ref List<string> AssetsUsedGUID, string path = "")
    {
        // SEARCH IN FILES
        path = (path == "") ? "Assets/" : path;

        string[] fileEntries = Directory.GetFiles(path);

        foreach (string fileName in fileEntries)
        {
            _currentAssets++;

            if (!IsContainerAsset(fileName)) continue;

            if (!IsContainGUID(guid, fileName)) continue;

            if (AssetsUsedGUID.Contains(fileName)) continue;

            AssetsUsedGUID.Add(fileName);
        }

        UpdateProgressBar();

        // SEARCH IN DIRS RECURSIVETLY
        string[] dirsEntries = Directory.GetDirectories(path);

        foreach (string dir in dirsEntries)
        {
            GetAllAssetsWithGUID(guid, ref AssetsUsedGUID, dir);
        }
    }

    private bool IsContainGUID(string guid, string assetPath)
    {
        bool isContainsGUID = false;
        string data = LoadAsset(assetPath);
        if (data.Contains(guid))
        {
            isContainsGUID = true;
        }
        return isContainsGUID;
    }

    private bool IsContainerAsset(string fileName)
    {
        if (fileName.Contains(".meta")) return false;

        if (!fileName.Contains(".unity") &&
            !fileName.Contains(".prefab") &&
            !fileName.Contains(".controller") &&
            !fileName.Contains(".asset") &&
            !fileName.Contains(".mat")) return false;

        return true;
    }
}