using System.Collections.Generic;
using UnityEngine.Events;
using UnityEditor.Events;
using Doozy.Engine.UI;
using App.Vibrations;
using UnityEngine;
using UnityEditor;

public sealed class VibrationButtonAdderEditor : EditorWindow
{
    private const float MAX_WINDOW_HEIGHT = 225.5f;
    private const float MAX_SCROLL_HEIGHT = 125f;

    private const float WINDOW_HEIGHT = 160f;
    private const float WINDOW_WIDTH = 400f;

    private const float PREFAB_HEIGHT = 25f;

    private List<GameObject> _prefabs = new List<GameObject>();
    private Vector2 _scrollPos;

    [MenuItem("Tools/Vibration button adder")]
    public static void ShowWindow()
    {
        GetWindow<VibrationButtonAdderEditor>("Vibration button adder");
    }

    private void HandleClearPrefabs()
    {
        _prefabs.Clear();
    }

    private void HandleAddPrefabs()
    {
        var paths = EditorUtility.OpenFilePanelWithFilters("Select Prefabs", "Assets", new string[]
        {
            "Prefab files", "prefab", "All files", "*"
        });

        if (!string.IsNullOrEmpty(paths))
        {
            var assetPath = "Assets" + paths.Substring(Application.dataPath.Length);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (prefab != null && !_prefabs.Contains(prefab))
            {
                _prefabs.Add(prefab);
            }
        }
    }

    private void HandleProcessComponents()
    {
        foreach (var prefab in _prefabs)
        {
            if (prefab == null)
            {
                continue;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            var buttons = instance.GetComponentsInChildren<UIButton>();

            foreach (var button in buttons) // some operations
            {
                AddOnPointerDownEvent(button);
            }

            PrefabUtility.SaveAsPrefabAsset(instance, AssetDatabase.GetAssetPath(prefab));
            DestroyImmediate(instance);
        }

        Debug.Log("Processing complete");
    }

    private void AddOnPointerDownEvent(UIButton button)
    {
        if (button.gameObject.TryGetComponent<VibrationButton>(out _))
        {
            Debug.LogWarning($"UIButton({button.gameObject.name}) has \"VibrationButton\" component");
            return;
        }

        var vibrationButton = button.gameObject.AddComponent<VibrationButton>();

        if (!button.OnPointerDown.Enabled)
        {
            button.OnPointerDown.Enabled = true;
        }

        var action = new UnityAction(vibrationButton.SendPlayEvent);

        UnityEventTools.AddPersistentListener(button.OnPointerDown.OnTrigger.Event, action);

        Debug.Log($"\"VibrationButton\" added to UIButton({button.gameObject.name})");
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        Event evt = Event.current;

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                {
                    return;
                }

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        var prefab = draggedObject as GameObject;

                        if (prefab != null && !_prefabs.Contains(prefab))
                        {
                            _prefabs.Add(prefab);
                        }
                    }
                }

                Event.current.Use();
                break;
        }
    }

    private void AdjustWindowHeight()
    {
        if (_prefabs.Count * PREFAB_HEIGHT > MAX_SCROLL_HEIGHT)
        {
            this.minSize = new Vector2(WINDOW_WIDTH, MAX_WINDOW_HEIGHT);
            this.maxSize = new Vector2(WINDOW_WIDTH, MAX_WINDOW_HEIGHT);

            return;
        }

        float additionalHeight = _prefabs.Count * PREFAB_HEIGHT / 2;
        float minHeight = WINDOW_HEIGHT + additionalHeight;

        this.minSize = new Vector2(WINDOW_WIDTH, minHeight);
        this.maxSize = new Vector2(WINDOW_WIDTH, minHeight);
    }

    private void OnGUIDragAndDrop()
    {
        var dropAreaStyle = new GUIStyle(GUI.skin.box);

        dropAreaStyle.border = new RectOffset(3, 3, 3, 3);

        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        var dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUILayout.Space(5);
        GUILayout.EndHorizontal();

        if (dropArea.Contains(Event.current.mousePosition))
        {
            dropAreaStyle.normal.textColor = Color.green;
        }

        GUI.Box(dropArea, "Drag prefabs here", dropAreaStyle);

        HandleDragAndDrop(dropArea);
    }

    private void OnGUIScroll()
    {
        float totalPrefabsHeight = _prefabs.Count * PREFAB_HEIGHT;
        float height = totalPrefabsHeight > MAX_SCROLL_HEIGHT
            ? MAX_SCROLL_HEIGHT
            : totalPrefabsHeight;

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(height));

        foreach (var prefab in _prefabs)
        {
            EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
        }

        EditorGUILayout.EndScrollView();
    }

    private void OnGUIButtons()
    {
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear prefabs"))
        {
            HandleClearPrefabs();
        }
        if (GUILayout.Button("Add prefabs"))
        {
            HandleAddPrefabs();
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Start"))
        {
            HandleProcessComponents();
        }
    }

    private void OnGUI()
    {
        if (_prefabs.Count == 0)
        {
            EditorGUILayout.HelpBox("Add prefabs", MessageType.Info);
        }

        OnGUIScroll();
        OnGUIDragAndDrop();
        OnGUIButtons();

        AdjustWindowHeight();
    }
}
