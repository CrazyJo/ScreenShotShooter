using System.Text;
using System.Windows.Forms;
using HelpersLib;

namespace HotKeyLib
{
    public class HotkeyInfo
    {
        public Keys Hotkey { get; set; }
        public ushort ID { get; set; }

        public HotkeyStatus Status { get; set; }

        public Keys KeyCode
        {
            get
            {
                return Hotkey & Keys.KeyCode;
            }
        }

        public Keys ModifiersKeys
        {
            get
            {
                return Hotkey & Keys.Modifiers;
            }
        }

        public bool Control
        {
            get
            {
                return Hotkey.HasFlag(Keys.Control);
            }
        }

        public bool Shift
        {
            get
            {
                return Hotkey.HasFlag(Keys.Shift);
            }
        }

        public bool Alt
        {
            get
            {
                return Hotkey.HasFlag(Keys.Alt);
            }
        }

        public bool Win { get; set; }

        public Modifiers ModifiersEnum
        {
            get
            {
                Modifiers modifiers = Modifiers.None;

                if (Alt) modifiers |= Modifiers.Alt;
                if (Control) modifiers |= Modifiers.Control;
                if (Shift) modifiers |= Modifiers.Shift;
                if (Win) modifiers |= Modifiers.Win;

                return modifiers;
            }
        }

        public bool IsOnlyModifiers
        {
            get
            {
                return KeyCode == Keys.ControlKey || KeyCode == Keys.ShiftKey || KeyCode == Keys.Menu || (KeyCode == Keys.None && Win);
            }
        }

        public bool IsValidHotkey
        {
            get
            {
                return KeyCode != Keys.None && !IsOnlyModifiers;
            }
        }

        public HotkeyInfo()
        {
            Status = HotkeyStatus.NotConfigured;
        }

        public HotkeyInfo(Keys hotkey) : this()
        {
            Hotkey = hotkey;
        }

        public HotkeyInfo(Keys hotkey, ushort id) : this(hotkey)
        {
            ID = id;
        }

        public override string ToString()
        {
            string text = "";

            if (Control)
            {
                text += "Ctrl + ";
            }

            if (Shift)
            {
                text += "Shift + ";
            }

            if (Alt)
            {
                text += "Alt + ";
            }

            if (Win)
            {
                text += "Win + ";
            }

            if (IsOnlyModifiers)
            {
                text += "...";
            }
            else if (KeyCode == Keys.Back)
            {
                text += "Backspace";
            }
            else if (KeyCode == Keys.Return)
            {
                text += "Enter";
            }
            else if (KeyCode == Keys.Capital)
            {
                text += "Caps Lock";
            }
            else if (KeyCode == Keys.Next)
            {
                text += "Page Down";
            }
            else if (KeyCode == Keys.Scroll)
            {
                text += "Scroll Lock";
            }
            else if (KeyCode >= Keys.D0 && KeyCode <= Keys.D9)
            {
                text += (KeyCode - Keys.D0).ToString();
            }
            else if (KeyCode >= Keys.NumPad0 && KeyCode <= Keys.NumPad9)
            {
                text += "Numpad " + (KeyCode - Keys.NumPad0).ToString();
            }
            else
            {
                text += ToStringWithSpaces(KeyCode);
            }

            return text;
        }

        private string ToStringWithSpaces(Keys key)
        {
            string name = key.ToString();

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < name.Length; i++)
            {
                if (i > 0 && char.IsUpper(name[i]))
                {
                    result.Append(" " + name[i]);
                }
                else
                {
                    result.Append(name[i]);
                }
            }

            return result.ToString();
        }
    }
}
