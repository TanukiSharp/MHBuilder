using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Core
{
    public class LanguageEventArgs : EventArgs
    {
        public Language PreviousLanguage { get; private set; }
        public Language NewLanguage { get; private set; }

        public LanguageEventArgs(Language previousLanguage, Language newLanguage)
        {
            PreviousLanguage = previousLanguage;
            NewLanguage = newLanguage;
        }
    }

    public class LocalizationContextEventArgs : EventArgs
    {
        public LocalizationContext? PreviousLocalizationContext { get; private set; }
        public LocalizationContext? NewLocalizationContext { get; private set; }

        public LocalizationContextEventArgs(LocalizationContext? previousLocalizationContext, LocalizationContext? newLocalizationContext)
        {
            PreviousLocalizationContext = previousLocalizationContext;
            NewLocalizationContext = newLocalizationContext;
        }
    }

    public class LocalizationContext
    {
        private static LocalizationContext? defaultContext;

        public static LocalizationContext? DefaultContext
        {
            get { return defaultContext; }
            set
            {
                if (defaultContext != value)
                {
                    var previousContext = defaultContext;
                    defaultContext = value;
                    DefaultContextChanged?.Invoke(null, new LocalizationContextEventArgs(previousContext, defaultContext));
                }
            }
        }

        public static event EventHandler<LocalizationContextEventArgs>? DefaultContextChanged;

        private Language currentLanguage;
        
        public Language DefaultLanguage { get; private set; }

        public Language CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                if (currentLanguage.Code != value.Code)
                {
                    var previousLanguage = currentLanguage;
                    currentLanguage = value;
                    CurrentLanguageChanged?.Invoke(this, new LanguageEventArgs(previousLanguage, currentLanguage));
                }
            }
        }

        public event EventHandler<LanguageEventArgs>? CurrentLanguageChanged;

        public LocalizationContext(Language currentLanguage)
            : this(currentLanguage, currentLanguage)
        {
        }

        public LocalizationContext(Language currentLanguage, Language defaultLanguage)
        {
            this.currentLanguage = currentLanguage;
            DefaultLanguage = defaultLanguage;
        }
    }
}
