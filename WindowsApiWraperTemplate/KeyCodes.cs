using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsApiWraperTemplate
{
    public static class KeyCodes
    {
        public static uint WM_KEYDOWN = 0x0100;
        public static uint WM_KEYUP = 0x0101;
        public static uint WM_MOUSEMOVE = 0x0200;
        public static uint WM_LBUTTONDOWN = 0x0201;
        public static uint WM_LBUTTONUP = 0x0202;
        public static int MK_LBUTTON = 0x0001;
        public static int WM_MOUSEWHEEL = 0x020A;
        public static uint WM_RBUTTONDOWN = 0x0204;
        public static uint WM_RBUTTONUP = 0x0205;
        public static int WH_MOUSE_LL = 14;
        public static uint WM_CLOSE = 0x0010;

        public static int SW_NORMAL = 9;
        public static int SW_HIDE = 0;


        public static uint VK_CONTROL = 0x00A2;
        public static uint VK_SHIFT = 0x0010;
        public static uint VK_ALT = 0x0038;
        public static int VK_F4 = 0x003e;
        public static uint VK_E = 0x0069;

        //Do kombinacji kalwiszy z Shift i Ctlr
        public static uint KEYBDEVENTF_SHIFTSCANCODE = 0x2A;
        public static uint KEYBDEVENTF_CONTROLSCANCODE = 0x1D;

        public static int KEYBDEVENTF_KEYDOWN = 0;
        public static int KEYBDEVENTF_KEYUP = 2;
    }
}
