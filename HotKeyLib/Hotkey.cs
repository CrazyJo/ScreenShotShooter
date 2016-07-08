using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Windows.Forms;
using HelpersLib;
using NativeMethods = HelpersLib.Native.NativeMethods;

namespace HotKeyLib
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public sealed class Hotkey : IMessageFilter 
    {
        public delegate void HotkeyEventHandler(ushort id, Keys key, Modifiers modifier);

        public event HotkeyEventHandler HotkeyPress;
        private readonly Stopwatch _repeatLimitTimer;
        private readonly IntPtr _handle;

        public int HotkeyRepeatLimit { get; set; }

        public Hotkey()
        {
            HotkeyRepeatLimit = 1000;
            _repeatLimitTimer = Stopwatch.StartNew();
            _handle = new IntPtr(0);
        }

        public void RegisterHotkey(HotkeyInfo hotkeyInfo)
        {
            if (hotkeyInfo != null && hotkeyInfo.Status != HotkeyStatus.Registered)
            {
                if (!hotkeyInfo.IsValidHotkey)
                {
                    hotkeyInfo.Status = HotkeyStatus.NotConfigured;
                    return;
                }

                if (hotkeyInfo.ID == 0)
                {
                    string uniqueID = Helpers.GetUniqueID();
                    hotkeyInfo.ID = NativeMethods.GlobalAddAtom(uniqueID);

                    if (hotkeyInfo.ID == 0)
                    {
                        hotkeyInfo.Status = HotkeyStatus.Failed;
                        return;
                    }
                }

                if (!NativeMethods.RegisterHotKey(_handle, hotkeyInfo.ID, (uint)hotkeyInfo.ModifiersEnum, (uint)hotkeyInfo.KeyCode))
                {
                    NativeMethods.GlobalDeleteAtom(hotkeyInfo.ID);
                    hotkeyInfo.ID = 0;
                    hotkeyInfo.Status = HotkeyStatus.Failed;
                    return;
                }

                hotkeyInfo.Status = HotkeyStatus.Registered;
            }
        }

        public bool UnregisterHotkey(HotkeyInfo hotkeyInfo)
        {
            if (hotkeyInfo != null)
            {
                if (hotkeyInfo.ID > 0)
                {
                    bool result = NativeMethods.UnregisterHotKey(_handle, hotkeyInfo.ID);

                    if (result)
                    {
                        NativeMethods.GlobalDeleteAtom(hotkeyInfo.ID);
                        hotkeyInfo.ID = 0;
                        hotkeyInfo.Status = HotkeyStatus.NotConfigured;
                        return true;
                    }
                }

                hotkeyInfo.Status = HotkeyStatus.Failed;
            }

            return false;
        }

        /*protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WindowsMessages.HOTKEY && CheckRepeatLimitTime())
            {
                ushort id = (ushort)m.WParam;
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                Modifiers modifier = (Modifiers)((int)m.LParam & 0xFFFF);
                OnKeyPressed(id, key, modifier);
                return;
            }

            base.WndProc(ref m);
        }*/

        private void OnKeyPressed(ushort id, Keys key, Modifiers modifier)
        {
            HotkeyPress?.Invoke(id, key, modifier);
        }

        private bool CheckRepeatLimitTime()
        {
            if (HotkeyRepeatLimit > 0)
            {
                if (_repeatLimitTimer.ElapsedMilliseconds >= HotkeyRepeatLimit)
                {
                    _repeatLimitTimer.Reset();
                    _repeatLimitTimer.Start();
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == (int)WindowsMessages.HOTKEY && CheckRepeatLimitTime())
            {
                ushort id = (ushort)m.WParam;
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                Modifiers modifier = (Modifiers)((int)m.LParam & 0xFFFF);
                OnKeyPressed(id, key, modifier);
                return true;
            }

            return false;
        }
    }
}
