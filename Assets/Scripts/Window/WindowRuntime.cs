using UnityEngine;
using WindowControl;
using System;

public class WindowRuntime : MonoBehaviour
{
    void Start()
    {
        IntPtr hwnd = WindowAPI.FindRealUnityWindow();
        if (hwnd == IntPtr.Zero) return;

        var settings = Resources.Load<WindowSettings>("WindowSettings");
        if (settings == null) return;

        if (settings.borderless)
            WindowAPI.SetFullBorderless(hwnd);

        WindowAPI.SetAlwaysOnTop(hwnd, settings.alwaysOnTop);

        WindowAPI.SetOpacity(hwnd, settings.opacity);

        if (settings.transparent)
        {
            var c = new WindowAPI.Color32(
                (byte)(settings.transparentColor.r * 255),
                (byte)(settings.transparentColor.g * 255),
                (byte)(settings.transparentColor.b * 255)
            );

            WindowAPI.SetTransparentColorKey(hwnd, c);
        }

        WindowAPI.ApplyPositionPreset(
            hwnd,
            settings.width,
            settings.height,
            settings.positionPreset
        );
    }
}