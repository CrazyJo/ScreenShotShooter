using HotKeyLib;

namespace MainLib
{
    public interface IUserControlFactory
    {
        HKSControl CreateHKControl(HotkeySettings arg);
    }
}
