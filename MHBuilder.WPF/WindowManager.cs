using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Text.Json.Serialization;
using System.Windows.Input;
using MHBuilder.Core;

namespace MHBuilder.WPF
{
    public interface IWindowConfiguration
    {
        string? Type { get; }
        int? Left { get; }
        int? Top { get; }
        int? Width { get; }
        int? Height { get; }
        bool IsMaximized { get; }
    }

    internal record ReadonlyWindowConfiguration(string? Type, int? Left, int? Top, int? Width, int? Height, bool IsMaximized) : IWindowConfiguration;

    public static class WindowManager
    {
        private class WindowContainer : IWindowConfiguration
        {
            public string? Type { get; set; }
            public int? Left { get; set; }
            public int? Top { get; set; }
            public int? Width { get; set; }
            public int? Height { get; set; }
            public bool IsMaximized { get; set; }
            public Window? Instance { get; set; }
        }

        private static readonly Dictionary<string, WindowContainer> windowContainers = new Dictionary<string, WindowContainer>();

        public static IWindowConfiguration[] GetWindowsConfiguration()
        {
            var result = new List<IWindowConfiguration>();

            foreach (KeyValuePair<string, WindowContainer> item in windowContainers)
            {
                result.Add(new ReadonlyWindowConfiguration(
                    item.Key,
                    item.Value.Left,
                    item.Value.Top,
                    item.Value.Width,
                    item.Value.Height,
                    item.Value.IsMaximized
                ));
            }

            return result.ToArray();
        }

        public static void SetWindowsConfiguration(IWindowConfiguration[] windowsConfiguration)
        {
            windowContainers.Clear();

            foreach (IWindowConfiguration item in windowsConfiguration)
            {
                windowContainers[item.Type!] = new WindowContainer
                {
                    IsMaximized = item.IsMaximized,
                    Left = item.Left,
                    Top = item.Top,
                    Width = item.Width,
                    Height = item.Height,
                };
            }
        }

        public static void Show<T>(object? argument = null) where T : Window
        {
            Window window = PrepareToShow<T>();

            bool isAlreadyOpened = window.IsVisible;

            IManagedWindow? managedWindow = window as IManagedWindow;

            if (managedWindow != null)
                managedWindow.OnOpening(isAlreadyOpened, argument);

            window.Show();
            window.Activate();

            if (managedWindow != null)
                managedWindow.OnOpened(isAlreadyOpened, argument);
        }

        public static bool? ShowDialog<T>(object? argument = null) where T : Window
        {
            Window window = PrepareToShow<T>();

            bool isAlreadyOpened = window.IsVisible;

            if (window is IManagedWindow managedWindow)
                managedWindow.OnOpening(isAlreadyOpened, argument);

            if (isAlreadyOpened)
                window.Hide();

            return window.ShowDialog();
        }

        private static void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string key = GetKeyOf(sender);

            WindowContainer container = windowContainers[key];
            Window? window = container.Instance;

            if (window != null)
            {
                StorePositionInternal(container, window);

                if (Application.Current.MainWindow == window)
                    Application.Current.Shutdown();
                else
                {
                    window.Hide();
                    e.Cancel = true;
                }
            }
        }

        private static Window PrepareToShow<T>() where T : Window
        {
            WindowContainer container = GetOrCreateWindow<T>();

            Window window = container.Instance!;

            if (window.IsVisible == false)
                RestorePositionInternal(container, window);
            else if (window.WindowState == WindowState.Minimized)
                window.WindowState = WindowState.Normal;

            FitInScreen(window);

            return window;
        }

        private static void RestorePositionInternal<T>(WindowContainer container, T window) where T : Window
        {
            if (container.Left.HasValue && container.Top.HasValue && container.Width.HasValue && container.Height.HasValue)
                window.WindowStartupLocation = WindowStartupLocation.Manual;

            if (container.Left.HasValue)
                window.Left = container.Left.Value;
            if (container.Top.HasValue)
                window.Top = container.Top.Value;
            if (container.Width.HasValue)
                window.Width = container.Width.Value;
            if (container.Height.HasValue)
                window.Height = container.Height.Value;

            if (container.IsMaximized)
                window.WindowState = WindowState.Maximized;
        }

        private static void StorePositionInternal(WindowContainer container, Window instance)
        {
            if (double.IsInfinity(instance.RestoreBounds.Left) == false)
                container.Left = (int)instance.RestoreBounds.Left;

            if (double.IsInfinity(instance.RestoreBounds.Top) == false)
                container.Top = (int)instance.RestoreBounds.Top;

            if (double.IsInfinity(instance.RestoreBounds.Width) == false)
                container.Width = (int)instance.RestoreBounds.Width;

            if (double.IsInfinity(instance.RestoreBounds.Height) == false)
                container.Height = (int)instance.RestoreBounds.Height;

            container.IsMaximized = instance.WindowState == WindowState.Maximized;
        }

        private static string GetKeyOf(Type type)
        {
            string? typeName = type.FullName;

            if (typeName == null)
                throw new Exception($"Invalid type argument {type}");

            return typeName;
        }

        private static string GetKeyOf(object instance)
        {
            return GetKeyOf(instance.GetType());
        }

        private static string GetKeyOf<T>()
        {
            return GetKeyOf(typeof(T));
        }

        private static WindowContainer? GetContainerOf<T>()
        {
            windowContainers.TryGetValue(GetKeyOf<T>(), out WindowContainer? container);

            return container;
        }

        private static WindowContainer GetOrCreateWindow<T>() where T : Window
        {
            string key = GetKeyOf<T>();

            windowContainers.TryGetValue(key, out WindowContainer? container);

            if (container == null)
            {
                container = new WindowContainer();
                windowContainers.Add(key, container);
            }

            Window? window = container.Instance;

            if (window == null)
            {
                window = Activator.CreateInstance<T>();
                window.Closing += OnWindowClosing;
                container.Instance = window;

                window.InputBindings.Add(new InputBinding(new AnonymousCommand(() =>
                {
                    bool shouldClose = true;

                    if (window is IManagedWindow managedWindow)
                    {
                        var cancellableOperationParameter = new CancellableOperationParameter();
                        managedWindow.OnCancel(cancellableOperationParameter);
                        if (cancellableOperationParameter.IsCancelled)
                            shouldClose = false;
                    }

                    if (shouldClose)
                        window.Close();
                }), new KeyGesture(Key.Escape, ModifierKeys.None)));
            }

            return container;
        }

        public static bool FitInScreen(Window window)
        {
            var windowInteropHelper = new WindowInteropHelper(window);

            IntPtr hMonitor = MonitorFromWindow(windowInteropHelper.Handle, MONITOR_DEFAULTTONEAREST);
            if (hMonitor == IntPtr.Zero)
                return false;

            var monitorInfo = MonitorInfoEx.Create();
            if (GetMonitorInfo(hMonitor, ref monitorInfo) == false)
                return false;

            DpiScale dpiInfo = VisualTreeHelper.GetDpi(Application.Current.MainWindow);
            monitorInfo.Monitor.bottom = (int)Math.Floor(monitorInfo.Monitor.right / dpiInfo.DpiScaleX);
            monitorInfo.Monitor.bottom = (int)Math.Floor(monitorInfo.Monitor.bottom / dpiInfo.DpiScaleY);
            monitorInfo.WorkArea.right = (int)Math.Floor(monitorInfo.WorkArea.right / dpiInfo.DpiScaleX);
            monitorInfo.WorkArea.bottom = (int)Math.Floor(monitorInfo.WorkArea.bottom / dpiInfo.DpiScaleY);

            int top = (int)window.Top;
            if (top < monitorInfo.WorkArea.top)
            {
                window.Top = monitorInfo.WorkArea.top;
            }

            int bottom = (int)(window.Top + window.Height);
            if (bottom > monitorInfo.WorkArea.bottom)
            {
                if (window.Height < monitorInfo.WorkArea.bottom)
                    window.Top = monitorInfo.WorkArea.bottom - window.Height;
                else
                {
                    window.Top = monitorInfo.WorkArea.top;
                    window.Height = monitorInfo.WorkArea.bottom - monitorInfo.WorkArea.top;
                }
            }

            int left = (int)window.Left;
            if (left < monitorInfo.WorkArea.left)
            {
                window.Left = monitorInfo.WorkArea.left;
            }

            int right = (int)(window.Left + window.Width);
            if (right > monitorInfo.WorkArea.right)
            {
                if (window.Width < monitorInfo.WorkArea.right)
                    window.Left = monitorInfo.WorkArea.right - window.Width;
                else
                {
                    window.Left = monitorInfo.WorkArea.left;
                    window.Width = monitorInfo.WorkArea.right - monitorInfo.WorkArea.left;
                }
            }

            return true;
        }

        #region Native

        private const int MONITOR_DEFAULTTONEAREST = 2;

        [DllImport("user32.dll")]
        private extern static IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        // size of a device name string
        private const int CCHDEVICENAME = 32;

        /// <summary>
        /// The MONITORINFOEX structure contains information about a display monitor.
        /// The GetMonitorInfo function stores information into a MONITORINFOEX structure or a MONITORINFO structure.
        /// The MONITORINFOEX structure is a superset of the MONITORINFO structure. The MONITORINFOEX structure adds a string member to contain a name
        /// for the display monitor.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MonitorInfoEx
        {
            /// <summary>
            /// The size, in bytes, of the structure. Set this member to sizeof(MONITORINFOEX) (72) before calling the GetMonitorInfo function.
            /// Doing so lets the function determine the type of structure you are passing to it.
            /// </summary>
            public int Size;

            /// <summary>
            /// A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates.
            /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
            /// </summary>
            public NativeRect Monitor;

            /// <summary>
            /// A RECT structure that specifies the work area rectangle of the display monitor that can be used by applications,
            /// expressed in virtual-screen coordinates. Windows uses this rectangle to maximize an application on the monitor.
            /// The rest of the area in rcMonitor contains system windows such as the task bar and side bars.
            /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
            /// </summary>
            public NativeRect WorkArea;

            /// <summary>
            /// The attributes of the display monitor.
            ///
            /// This member can be the following value:
            ///   1 : MONITORINFOF_PRIMARY
            /// </summary>
            public uint Flags;

            /// <summary>
            /// A string that specifies the device name of the monitor being used. Most applications have no use for a display monitor name,
            /// and so can save some bytes by using a MONITORINFO structure.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string DeviceName;

            public static MonitorInfoEx Create()
            {
                return new MonitorInfoEx
                {
                    Size = 40 + 2 * CCHDEVICENAME,
                    Monitor = new NativeRect(),
                    WorkArea = new NativeRect(),
                    DeviceName = string.Empty,
                    Flags = 0
                };
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeRect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        #endregion // Native
    }
}
