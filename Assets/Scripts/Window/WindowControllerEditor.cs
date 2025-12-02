#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class WindowControllerEditor : EditorWindow
{
    enum WindowPos
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    WindowPos pos = WindowPos.MiddleCenter;
    int width = 500;
    int height = 300;

    FullScreenMode fullscreenMode = FullScreenMode.Windowed;
    bool runInBackground = true;
    bool resizable = false;
    bool visibleInBackground = true;
    bool usePlayerLog = true;
    bool allowFullscreenSwitch = true;
    bool forceSingleInstance = false;
    bool useDXGIFlip = false;

    bool borderless = false;
    bool alwaysOnTop = false;
    float opacity = 1f;
    bool transparent = false;
    Color transparentColor = Color.black;

    WindowSettings ws;

    [MenuItem("Tools/Window Controller")]
    static void Open()
    {
        GetWindow<WindowControllerEditor>("Window Controller");
    }

    GUIStyle infoStyle;

    void OnEnable()
    {
        infoStyle = new GUIStyle(EditorStyles.label);
        infoStyle.fontSize = 10;
        infoStyle.normal.textColor = new Color(0.65f, 0.65f, 0.65f);
        infoStyle.padding = new RectOffset(20, 0, -4, 6);

        ws = Resources.Load<WindowSettings>("WindowSettings");
        if (ws == null)
        {
            ws = ScriptableObject.CreateInstance<WindowSettings>();
            AssetDatabase.CreateAsset(ws, "Assets/Resources/WindowSettings.asset");
        }
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Unity Player Settings", EditorStyles.boldLabel);

        runInBackground = EditorGUILayout.Toggle("Run In Background", runInBackground);
        EditorGUILayout.LabelField("Oyun arka planda çalışmaya devam eder.", infoStyle);

        fullscreenMode = (FullScreenMode)EditorGUILayout.EnumPopup("Fullscreen Mode", fullscreenMode);
        EditorGUILayout.LabelField("Pencere veya tam ekran modu.", infoStyle);

        width = EditorGUILayout.IntField("Default Width", width);
        EditorGUILayout.LabelField("Oyun penceresinin varsayılan genişliği.", infoStyle);

        height = EditorGUILayout.IntField("Default Height", height);
        EditorGUILayout.LabelField("Oyun penceresinin varsayılan yüksekliği.", infoStyle);

        usePlayerLog = EditorGUILayout.Toggle("Use Player Log", usePlayerLog);
        EditorGUILayout.LabelField("Player.log dosyasını aktif eder.", infoStyle);

        resizable = EditorGUILayout.Toggle("Resizable Window", resizable);
        EditorGUILayout.LabelField("Pencerenin kullanıcı tarafından yeniden boyutlandırılmasına izin verir.",
            infoStyle);

        visibleInBackground = EditorGUILayout.Toggle("Visible In Background", visibleInBackground);
        EditorGUILayout.LabelField("Oyun arka plandayken görüntü güncellenmeye devam eder.", infoStyle);

        allowFullscreenSwitch = EditorGUILayout.Toggle("Allow Fullscreen Switch", allowFullscreenSwitch);
        EditorGUILayout.LabelField("Alt+Enter ile tam ekran geçişine izin verir.", infoStyle);

        forceSingleInstance = EditorGUILayout.Toggle("Force Single Instance", forceSingleInstance);
        EditorGUILayout.LabelField("Oyunun aynı anda yalnızca 1 instance çalışmasına izin verir.", infoStyle);

        useDXGIFlip = EditorGUILayout.Toggle("Use DXGI Flip Model", useDXGIFlip);
        EditorGUILayout.LabelField("DXGI Flip modelini kullanarak performans iyileştirmesi sağlar.", infoStyle);

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Extra Window Options (WinAPI)", EditorStyles.boldLabel);

        pos = (WindowPos)EditorGUILayout.EnumPopup("Position", pos);
        EditorGUILayout.LabelField("Oyunun ekranda açılacağı pozisyon.", infoStyle);

        borderless = EditorGUILayout.Toggle("Borderless", borderless);
        EditorGUILayout.LabelField("Pencere çerçevesini tamamen kaldırır.", infoStyle);

        alwaysOnTop = EditorGUILayout.Toggle("Always On Top", alwaysOnTop);
        EditorGUILayout.LabelField("Oyun penceresini her zaman diğerlerinin üstünde tutar.", infoStyle);

        opacity = EditorGUILayout.Slider("Opacity", opacity, 0f, 1f);
        EditorGUILayout.LabelField("Pencerenin saydamlık seviyesini ayarlar.", infoStyle);

        transparent = EditorGUILayout.Toggle("Transparent (Color Key)", transparent);
        EditorGUILayout.LabelField("Belirli bir rengi tamamen şeffaf yapar.", infoStyle);

        if (transparent)
        {
            transparentColor = EditorGUILayout.ColorField("Color Key", transparentColor);
            EditorGUILayout.LabelField("Şeffaf yapılacak renk.", infoStyle);
        }

        GUILayout.Space(20);

        if (GUILayout.Button("APPLY TO PLAYER SETTINGS", GUILayout.Height(40)))
        {
            ApplyToScriptable();
            SaveSO();
            ApplyPlayerSettings();
        }
    }

    void SaveSO()
    {
        EditorUtility.SetDirty(ws);
        AssetDatabase.SaveAssets();
    }
    
    void ApplyToScriptable()
    {
        ws.runInBackground = runInBackground;
        ws.fullscreenMode = fullscreenMode;
        ws.width = width;
        ws.height = height;

        ws.usePlayerLog = usePlayerLog;
        ws.resizable = resizable;
        ws.visibleInBackground = visibleInBackground;
        ws.allowFullscreenSwitch = allowFullscreenSwitch;
        ws.forceSingleInstance = forceSingleInstance;
        ws.useDXGIFlip = useDXGIFlip;

        ws.borderless = borderless;
        ws.alwaysOnTop = alwaysOnTop;
        ws.opacity = opacity;
        ws.transparent = transparent;
        ws.transparentColor = transparentColor;
        ws.positionPreset = pos.ToString();
    }

    void ApplyPlayerSettings()
    {
        PlayerSettings.runInBackground = runInBackground;
        PlayerSettings.fullScreenMode = fullscreenMode;
        PlayerSettings.defaultScreenWidth = width;
        PlayerSettings.defaultScreenHeight = height;

        PlayerSettings.usePlayerLog = usePlayerLog;
        PlayerSettings.resizableWindow = resizable;
        PlayerSettings.visibleInBackground = visibleInBackground;
        PlayerSettings.allowFullscreenSwitch = allowFullscreenSwitch;
        PlayerSettings.forceSingleInstance = forceSingleInstance;
        PlayerSettings.useFlipModelSwapchain = useDXGIFlip;

        AssetDatabase.SaveAssets();

        SettingsService.OpenProjectSettings("Project/Player");

        EditorApplication.delayCall += () =>
        {
            if (EditorWindow.focusedWindow != null)
                EditorWindow.focusedWindow.Repaint();
        };

        Debug.Log("Player Settings Updated (Unity 6).");
    }
}
#endif