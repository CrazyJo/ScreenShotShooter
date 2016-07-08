using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using HelpersLib;

namespace MainLib
{
    public static class LanguageHelper
    {
        public static CultureInfo AppCulture
        {
            set
            {
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = value;
            }
        }

        public static bool ChangeLanguage(SupportedLanguage language)
        {
            CultureInfo currentCulture;

            if (language == SupportedLanguage.Automatic)
            {
                currentCulture = CultureInfo.InstalledUICulture;
            }
            else
            {
                string cultureName;

                switch (language)
                {
                    case SupportedLanguage.English:
                        cultureName = "en-US";
                        break;
                    case SupportedLanguage.Russian:
                        cultureName = "ru-RU";
                        break;
                    default:
                        throw new ArgumentException("Is not supported language");
                }

                currentCulture = CultureInfo.GetCultureInfo(cultureName);
            }

            if (!currentCulture.Equals(Thread.CurrentThread.CurrentUICulture))
            {
                //AppCulture = currentCulture;
                Helpers.SetDefaultUICulture(currentCulture);

                return true;
            }

            return false;
        }

    }
}
