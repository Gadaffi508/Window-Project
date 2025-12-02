using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowControl
{
    public static class WindowAPI
    {
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const uint WS_CAPTION = 0x00C00000;
        public const uint WS_BORDER = 0x00800000;
        public const uint WS_THICKFRAME = 0x00040000;
        public const uint WS_MINIMIZEBOX = 0x00020000;
        public const uint WS_MAXIMIZEBOX = 0x00010000;
        public const uint WS_SYSMENU = 0x00080000;

        public const uint WS_OVERLAPPEDWINDOW =
            WS_CAPTION | WS_BORDER | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU;

        public const uint WS_EX_LAYERED = 0x00080000;
        public const uint WS_EX_TRANSPARENT = 0x00000020;
        public const uint WS_EX_TOOLWINDOW = 0x00000080;
        public const uint LWA_ALPHA = 0x2;
        public const uint LWA_COLORKEY = 0x1;

        public const int HWND_TOPMOST = -1;
        public const int HWND_NOTOPMOST = -2;

        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")] public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll")] public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")] public static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        [DllImport("user32.dll")] public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll")] public static extern bool SetWindowPos(IntPtr hWnd, IntPtr insertAfter, int X, int Y, int cx, int cy, uint flags);
        [DllImport("user32.dll")] public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")] public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        [DllImport("user32.dll")] public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public struct Color32
        {
            public byte r, g, b;
            public Color32(byte r, byte g, byte b) { this.r = r; this.g = g; this.b = b; }
        }

        // Unity pencere HWND bulur
        public static IntPtr FindRealUnityWindow()
        {
            IntPtr found = IntPtr.Zero;
            EnumWindows((hwnd, l) =>
            {
                var sb = new StringBuilder(256);
                GetClassName(hwnd, sb, 256);
                string cls = sb.ToString();
                if (cls.Contains("Unity") || cls == "UnityWndClass")
                {
                    found = hwnd;
                    return false;
                }
                return true;
            }, IntPtr.Zero);
            return found;
        }

        // Borderless mod açar
        public static void SetFullBorderless(IntPtr hwnd)
        {
            uint style = GetWindowLong(hwnd, GWL_STYLE);
            style &= ~WS_OVERLAPPEDWINDOW;
            SetWindowLong(hwnd, GWL_STYLE, style);
        }

        // Pencereyi her zaman üstte tutar
        public static void SetAlwaysOnTop(IntPtr hwnd, bool enable)
        {
            SetWindowPos(hwnd, enable ? (IntPtr)HWND_TOPMOST : (IntPtr)HWND_NOTOPMOST, 0, 0, 0, 0, 0x0003 | 0x0040);
        }

        // Opacity ayarla
        public static void SetOpacity(IntPtr hwnd, float value)
        {
            uint ex = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, ex | WS_EX_LAYERED);
            byte a = (byte)(Math.Clamp(value, 0f, 1f) * 255f);
            SetLayeredWindowAttributes(hwnd, 0, a, LWA_ALPHA);
        }

        // Color key transparency
        public static void SetTransparentColorKey(IntPtr hwnd, Color32 color)
        {
            uint ex = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, ex | WS_EX_LAYERED);
            uint col = (uint)(color.r | (color.g << 8) | (color.b << 16));
            SetLayeredWindowAttributes(hwnd, col, 255, LWA_COLORKEY);
        }

        // Pencereyi mouse ile tıklanamaz yapar
        public static void SetClickThrough(IntPtr hwnd, bool enable)
        {
            uint ex = GetWindowLong(hwnd, GWL_EXSTYLE);
            if (enable)
                SetWindowLong(hwnd, GWL_EXSTYLE, ex | WS_EX_TRANSPARENT | WS_EX_LAYERED);
            else
                SetWindowLong(hwnd, GWL_EXSTYLE, ex & ~WS_EX_TRANSPARENT);
        }

        // Pencereyi sabit bir pozisyona kilitler
        public static void SetFixedPosition(IntPtr hwnd, int x, int y)
        {
            SetWindowPos(hwnd, IntPtr.Zero, x, y, 0, 0, 0x0001 | 0x0002);
        }

        // Pencereyi ekranın tam ortasına taşır
        public static void CenterOnScreen(IntPtr hwnd, int width, int height)
        {
            int screenW = 1920;
            int screenH = 1080;
            int posX = (screenW - width) / 2;
            int posY = (screenH - height) / 2;
            MoveWindow(hwnd, posX, posY, width, height, true);
        }

        // Pencereyi sürüklemeyi tamamen kapatır
        public static void DisableDragging(IntPtr hwnd)
        {
            SetWindowLongPtr(hwnd, -12, IntPtr.Zero);
        }

        // Pencerenin hareket etmesini engeller (tam kilit)
        public static void FreezeWindow(IntPtr hwnd)
        {
            uint style = GetWindowLong(hwnd, GWL_STYLE);
            style &= ~WS_THICKFRAME;
            style &= ~WS_CAPTION;
            SetWindowLong(hwnd, GWL_STYLE, style);
        }

        // Pencere kilidini açar
        public static void UnfreezeWindow(IntPtr hwnd)
        {
            uint style = GetWindowLong(hwnd, GWL_STYLE);
            style |= WS_CAPTION | WS_THICKFRAME | WS_BORDER;
            SetWindowLong(hwnd, GWL_STYLE, style);
        }

        // Pencereyi minimize eder
        public static void Minimize(IntPtr hwnd)
        {
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, 0x0020);
        }

        // Pencereyi maximize eder
        public static void Maximize(IntPtr hwnd)
        {
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, 0x0100);
        }

        // Pencereyi eski haline döndürür
        public static void Restore(IntPtr hwnd)
        {
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, 0x0000);
        }

        // 9 konum preset'i uygular
        public static void ApplyPositionPreset(IntPtr hwnd, int width, int height, string preset)
        {
            int screenW = 1920;
            int screenH = 1080;
            int x = 0, y = 0;

            switch (preset)
            {
                case "TopLeft": x = 0; y = 0; break;
                case "TopCenter": x = (screenW - width) / 2; y = 0; break;
                case "TopRight": x = screenW - width; y = 0; break;
                case "MiddleLeft": x = 0; y = (screenH - height) / 2; break;
                case "MiddleCenter": x = (screenW - width) / 2; y = (screenH - height) / 2; break;
                case "MiddleRight": x = screenW - width; y = (screenH - height) / 2; break;
                case "BottomLeft": x = 0; y = screenH - height; break;
                case "BottomCenter": x = (screenW - width) / 2; y = screenH - height; break;
                case "BottomRight": x = screenW - width; y = screenH - height; break;
            }

            MoveWindow(hwnd, x, y, width, height, true);
        }
    }
}