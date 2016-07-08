using System;
using System.Windows.Forms;

namespace HotKeyLib
{
    public abstract class HKSControl : UserControl
    {
        public event EventHandler HotkeyChanged;

        protected virtual void OnHotkeyChanged()
        {
            HotkeyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
