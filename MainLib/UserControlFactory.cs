using System.Threading;
using HotKeyLib;
using MainLib.AppSettings;

namespace MainLib
{
    public class UserControlFactory : IUserControlFactory
    {
        private static UserControlFactory _factory;

        protected UserControlFactory(ApplicationConfig config)
        {
            HotkeySelectionControl.HotKeysConfig = config.HotKeysConfig;
        }

        public static UserControlFactory GetFactory(ApplicationConfig config)
        {
            LazyInitializer.EnsureInitialized(ref _factory,
                () => new UserControlFactory(config));

            return _factory;
        }

        public HKSControl CreateHKControl(HotkeySettings arg)
        {
            return new HotkeySelectionControl(arg);
        }
    }
}
