using UnityEngine;

[System.Serializable]
public class WindowSettings : ScriptableObject
{
    // Player Settings
    public bool runInBackground;
    public FullScreenMode fullscreenMode;
    public int width;
    public int height;

    public bool usePlayerLog;
    public bool resizable;
    public bool visibleInBackground;
    public bool allowFullscreenSwitch;
    public bool forceSingleInstance;
    public bool useDXGIFlip;

    // WinAPI
    public bool borderless;
    public bool alwaysOnTop;
    public float opacity;

    public bool transparent;
    public Color transparentColor;

    public string positionPreset = "MiddleCenter";
}