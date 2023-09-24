using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PixelSnapper.Hotkey
{
    public class KeyboardHook : IDisposable
    {
        bool Global = false;

        public delegate void LocalKeyEventHandler(KeyCombination combo);
        public event LocalKeyEventHandler KeyDown;
        public event LocalKeyEventHandler KeyUp;

        public delegate int CallbackDelegate(int Code, int W, int L);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct KBDLLHookStruct
        {
            public Int32 vkCode;
            public Int32 scanCode;
            public Int32 flags;
            public Int32 time;
            public Int32 dwExtraInfo;
        }

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(HookType idHook, CallbackDelegate lpfn, int hInstance, int threadId);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, int lParam);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        private int HookID = 0;
        CallbackDelegate TheHookCB = null;

        //Start hook
        public KeyboardHook(bool Global)
        {
            this.Global = Global;
            TheHookCB = new CallbackDelegate(KeybHookProc);

            //https://stackoverflow.com/questions/3567758/setwindowshookex-returns-0-when-compiling-for-the-net-4-0-framework-in-32bit-ma

            if (Global)
                HookID = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, TheHookCB, 0, 0);
            else
                HookID = SetWindowsHookEx(HookType.WH_KEYBOARD, TheHookCB, 0, GetCurrentThreadId());
        }

        bool IsFinalized = false;
        ~KeyboardHook()
        {
            if (!IsFinalized)
            {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }
        public void Dispose()
        {
            if (!IsFinalized)
            {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }

        //The listener that will trigger events
        private int KeybHookProc(int Code, int W, int L)
        {
            KBDLLHookStruct LS = new KBDLLHookStruct();
            if (Code < 0)
            {
                return CallNextHookEx(HookID, Code, W, L);
            }
            try
            {
                KeyEvents kEvent = (KeyEvents)W;
                
                Int32 vkCode = Marshal.ReadInt32((IntPtr)L); //Leser vkCode som er de første 32 bits hvor L peker.

                if (kEvent == KeyEvents.KeyDown || kEvent == KeyEvents.SKeyDown)
                {
                    if (KeyDown != null) KeyDown(new KeyCombination((Keys)vkCode, GetWinPressed(), GetCtrlPressed(), GetAltPressed(), GetShiftPressed()));
                }

                if (kEvent == KeyEvents.KeyUp || kEvent == KeyEvents.SKeyUp)
                {
                    if (KeyUp != null) KeyUp(new KeyCombination((Keys)vkCode, GetWinPressed(), GetCtrlPressed(), GetAltPressed(), GetShiftPressed()));
                }
            }
            catch (Exception)
            {
                //Ignore all errors...
            }

            return CallNextHookEx(HookID, Code, W, L);

        }

        [DllImport("user32.dll")]
        static public extern short GetKeyState(System.Windows.Forms.Keys nVirtKey);

        public static bool GetCapslock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.CapsLock)) & true;
        }
        public static bool GetNumlock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.NumLock)) & true;
        }
        public static bool GetScrollLock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.Scroll)) & true;
        }

        public static bool GetShiftPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ShiftKey);
            return state > 1 || state < -1;
        }
        public static bool GetCtrlPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ControlKey);
            return state > 1 || state < -1;
        }
        public static bool GetAltPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.Menu);
            return state > 1 || state < -1;
        }
        public static bool GetWinPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.LWin);
            return state > 1 || state < -1;
        }
    }
}
