using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsApiWraperTemplate
{
    internal class Functions
    {
        //Dll Imports
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpClassName, string? lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint wMsg, int wParam, uint lParam);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint wMsg, uint wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowTextA(IntPtr hWnd, string text);
        [DllImport("user32.dll")]
        public static extern void keybd_event(uint vk, uint scan, int flags, int extrainfo);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern uint VkKeyScan(char ch);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int acton);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        [DllImport("User32.Dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, out GDI32.RECT rect);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();


        //Get list of windows containing given name
        public static List<IntPtr> getWindowsList(string windowName)
        {
            string lpClassName = windowName;
            List<IntPtr>  HWNDList = new List<IntPtr>();

            IntPtr parent = IntPtr.Zero;
            IntPtr child = IntPtr.Zero;

            IntPtr hwnd = FindWindowEx(parent, child, lpClassName, null);

            while (hwnd != IntPtr.Zero)
            {
                HWNDList.Add(hwnd);
                child = hwnd;
                hwnd = FindWindowEx(parent, child, lpClassName, null);
            }
            return HWNDList;
        }

        //Button Press Functions
        public static void SingleButtonPress(IntPtr HWND, Keys key)
        {
            PostMessage(HWND, KeyCodes.WM_KEYDOWN, (int)key, 0);
            PostMessage(HWND, KeyCodes.WM_KEYUP, (int)key, 0);
        }

        public static async void MultiButtonPress(List<IntPtr> HWNDList, Keys key, int delay)
        {
            for (int i = 0; i < HWNDList.Count; i++){
                SingleButtonPress(HWNDList.ElementAt(i), key);
                await Task.Delay(delay);
            }
        }

        public static void buttonWithShift(IntPtr HWND, char button)
        {
            uint VK = VkKeyScan(button);
            string stringButton = "{" + button.ToString() + "}";

            if (GetForegroundWindow() == HWND)
            {
                SendKeys.SendWait("+" + stringButton);
            }
            else
            {
                keybd_event(KeyCodes.VK_SHIFT, KeyCodes.KEYBDEVENTF_SHIFTSCANCODE, KeyCodes.KEYBDEVENTF_KEYDOWN, 0);
                SendMessage(HWND, KeyCodes.WM_KEYDOWN, VK, 0);
                SendMessage(HWND, KeyCodes.WM_KEYUP, VK, 0);
                keybd_event(KeyCodes.VK_SHIFT, KeyCodes.KEYBDEVENTF_SHIFTSCANCODE, KeyCodes.KEYBDEVENTF_KEYUP, 0);
            }
        }

        public async static void MultiButtonPressWithShift(List<IntPtr> HWNDList, char button, int delay)
        {
            for (int i = 0; i < HWNDList.Count; i++)
            {
                buttonWithShift(HWNDList.ElementAt(i), button);
                await Task.Delay(delay);
            }
        }

        public static void buttonWithControl(IntPtr HWND, char button)
        {
            uint VK = VkKeyScan(button);
            string stringButton = "{" + button.ToString() + "}";

            if (GetForegroundWindow() == HWND)
            {
                SendKeys.SendWait("^" + stringButton);
            }
            else
            {
                keybd_event(KeyCodes.VK_CONTROL, KeyCodes.KEYBDEVENTF_CONTROLSCANCODE, KeyCodes.KEYBDEVENTF_KEYDOWN, 0);
                SendMessage(HWND, KeyCodes.WM_KEYDOWN, VK, 0);
                SendMessage(HWND, KeyCodes.WM_KEYUP, VK, 0);
                keybd_event(KeyCodes.VK_CONTROL, KeyCodes.KEYBDEVENTF_CONTROLSCANCODE, KeyCodes.KEYBDEVENTF_KEYUP, 0);
            }
        }

        public async static void MultiButtonPressWithControl(List<IntPtr> HWNDList, char button, int delay)
        {
            for (int i = 0; i < HWNDList.Count; i++)
            {
                buttonWithControl(HWNDList.ElementAt(i), button);
                await Task.Delay(delay);
            }
        }

        public static void buttonWithAlt(IntPtr HWND, int key)
        {
            PostMessage(HWND, KeyCodes.WM_KEYDOWN, (int)key, 0x20020001);
            PostMessage(HWND, KeyCodes.WM_KEYUP, (int)key, 0xE0020001);
        }

        public async static void MultiButtonPressWithAlt(List<IntPtr> HWNDList, int key, int delay)
        {
            for (int i = 0; i < HWNDList.Count; i++)
            {
                buttonWithAlt(HWNDList.ElementAt(i), key);
                await Task.Delay(delay);
            }
        }


        //Mouse Clicks Functions
        public static void MouseClick()
        {
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(0x02 | 0x04, X, Y, 0, 0);
        }

        public static async Task ClickInBackground(short X, short Y, IntPtr HWND)
        {
            int Coordinates = MakeLong(X, Y);
            PostMessage(HWND, KeyCodes.WM_MOUSEMOVE, 0, Coordinates); //MOUSEMOVE
            PostMessage(HWND, KeyCodes.WM_LBUTTONDOWN, KeyCodes.MK_LBUTTON, Coordinates); // LBUTTONDOWN
            await Task.Delay(5);
            PostMessage(HWND, KeyCodes.WM_LBUTTONUP, 0, Coordinates); // LBUTTONUP
        }

      
        public static async Task MultiClickInBackground(short X, short Y, List<IntPtr> HWNDList, int delay)
        {
            int Coordinates = MakeLong(X, Y);
            for (int i = 0; i < HWNDList.Count; i++)
            {
                PostMessage(HWNDList.ElementAt(i), KeyCodes.WM_MOUSEMOVE, 0, Coordinates); //MOUSEMOVE
                PostMessage(HWNDList.ElementAt(i), KeyCodes.WM_LBUTTONDOWN, KeyCodes.MK_LBUTTON, Coordinates); // LBUTTONDOWN
                await Task.Delay(5);
                PostMessage(HWNDList.ElementAt(i), KeyCodes.WM_LBUTTONUP, 0, Coordinates); // LBUTTONUP
                await Task.Delay(delay);
            }
        }


        //Window operations
        public static void RenameClient(IntPtr HWND, string newName)
        {
            SetWindowTextA(HWND, newName);
        }

        public static void PopWindow(IntPtr HWND)
        {
            while (GetForegroundWindow() != HWND)
            {
                SetForegroundWindow(HWND);
            }
        }

        public static void moveWindow(IntPtr HWND, int x, int y)
        {
            SetWindowPos(HWND, 0, x, y, 0, 0, 0x0001);
        }

        public static void stackWindows(List<IntPtr> windowsList)
        {
            if (windowsList.Count == 0)
                return;

            Screen screen = null;

            for (int i = 0; i < windowsList.Count; i++)
            {
                screen = Screen.FromHandle(windowsList.ElementAt(0));
                break;
            }

            for (int i = 0; i < windowsList.Count; i++)
            {

                if (isMinimized(windowsList.ElementAt(i)))
                    ShowWindow(windowsList.ElementAt(i), KeyCodes.SW_NORMAL);

                GDI32.RECT curWindow = new GDI32.RECT();
                GDI32.GetClientRect(windowsList.ElementAt(i), ref curWindow);

                moveWindow(windowsList.ElementAt(i), screen.WorkingArea.X + screen.WorkingArea.Width / 2 - curWindow.right / 2, screen.WorkingArea.Y + screen.WorkingArea.Height / 2 - curWindow.bottom / 2);
            }
        }

        public static void windowsToWaterfall(List<IntPtr> windowsList)
        {
            Screen screen = null;
            GDI32.RECT firstAlt = new GDI32.RECT();
            for (int i = 0; i < windowsList.Count; i++)
            {
                screen = Screen.FromHandle(windowsList.ElementAt(i));
                GDI32.GetClientRect(windowsList.ElementAt(i), ref firstAlt);
                break;
            }

            if (screen == null)
                return;

            int xShift = (screen.WorkingArea.Width - firstAlt.right - 3) / (windowsList.Count - 1);
            int yShift = (screen.WorkingArea.Height - firstAlt.bottom - 26) / (windowsList.Count - 1);

            if (yShift > 110)
                yShift = 80;

            if (xShift > 250)
                xShift = 150;

            int j = 0;

            for (int i = 0; i < windowsList.Count; i++)
            {
                if (isMinimized(windowsList.ElementAt(i)))
                    ShowWindow(windowsList.ElementAt(i), KeyCodes.SW_NORMAL);

                moveWindow(windowsList.ElementAt(i), screen.WorkingArea.X + screen.WorkingArea.Width - firstAlt.right - j * xShift, screen.WorkingArea.Y + j * yShift);
                SetForegroundWindow(windowsList.ElementAt(i));
                j++;
            }
        }

        public static bool isMinimized(IntPtr HWND)
        {
            if (IsWindowVisible(HWND))
                return false;

            return true;
        }

        public static bool isFullScreen(IntPtr hWnd)
        {
            GDI32.RECT appBounds;
            Rectangle screenBounds;

            IntPtr desktopHandle = GetDesktopWindow();
            IntPtr shellHandle = GetShellWindow();

            //get the dimensions of the active window
            hWnd = GetForegroundWindow();
            if (hWnd != IntPtr.Zero && !hWnd.Equals(IntPtr.Zero))
            {
                //Check we haven't picked up the desktop or the shell
                if (!(hWnd.Equals(desktopHandle) || hWnd.Equals(shellHandle)))
                {
                    GetWindowRect(hWnd, out appBounds);
                    //determine if window is fullscreen
                    screenBounds = Screen.FromHandle(hWnd).Bounds;
                    if ((appBounds.bottom - appBounds.top) == screenBounds.Height && (appBounds.right - appBounds.left) == screenBounds.Width)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool isPointVisibleOnAScreen(Point p)
        {
            foreach (Screen s in Screen.AllScreens)
            {
                if (p.X < s.Bounds.Right && p.X > s.Bounds.Left && p.Y > s.Bounds.Top && p.Y < s.Bounds.Bottom)
                    return true;
            }
            return false;
        }

        public static bool isWidnowFullyVisible(IntPtr HWND)
        {
            GDI32.RECT window = new GDI32.RECT();
            GetWindowRect(HWND, out window);
            return isPointVisibleOnAScreen(new Point(window.left, window.top)) && isPointVisibleOnAScreen(new Point(window.right, window.top)) && isPointVisibleOnAScreen(new Point(window.left, window.bottom)) && isPointVisibleOnAScreen(new Point(window.right, window.bottom));
        }


        //Utilities
        public static int randomizeDelay(int maxDelay)
        {
            if (maxDelay == 0)
                return 0;

            Random r = new Random();
            int ret = r.Next(maxDelay - 100, maxDelay + 100);

            if (ret < 0)
                ret = 0;

            return ret;
        }

        public static int randomizeCoord(int coord, int range)
        {
            Random r = new Random();
            int ret = r.Next(coord - range, coord + range);

            return ret;
        }

        public static int MakeLong(short a, short b)
        {
            return (int)((ushort)a | ((uint)((ushort)b << 16)));
        }

    }
}

