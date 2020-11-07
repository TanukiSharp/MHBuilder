using System;
using System.Collections.Generic;
using System.Text;
using MHBuilder.Core;
using MHBuilder.WPF.ViewModels;

namespace MHBuilder.WPF.ViewModels
{
    public class LocalizableStringViewModel : ViewModelBase, IDisposable
    {
        private readonly LocalizableString localizableString;

        public LocalizableStringViewModel(LocalizableString localizableString)
        {
            this.localizableString = localizableString;

            LocalizationContext.DefaultContextChanged += DefaultLocalizationContextChanged!;

            UpdateSubscription(null, LocalizationContext.DefaultContext);
        }

        public static implicit operator LocalizableStringViewModel(LocalizableString localizableString)
        {
            return new LocalizableStringViewModel(localizableString);
        }

        private void DefaultLocalizationContextChanged(object sender, LocalizationContextEventArgs e)
        {
            UpdateSubscription(e.PreviousLocalizationContext, e.NewLocalizationContext);
        }

        private void UpdateSubscription(LocalizationContext? previousLocalizationContext, LocalizationContext? newLocalizationContext)
        {
            if (previousLocalizationContext != null)
                previousLocalizationContext.CurrentLanguageChanged -= LanguageChanged!;

            if (newLocalizationContext != null)
                newLocalizationContext.CurrentLanguageChanged += LanguageChanged!;
        }

        private void LanguageChanged(object sender, LanguageEventArgs e)
        {
            NotifyPropertyChanged(nameof(Text));
        }

        public string Text
        {
            get
            {
                LocalizationContext? localizationContext = LocalizationContext.DefaultContext;

                if (localizationContext is null)
                    return "<no-localization-context>";

                if (localizableString.TryGetValue(localizationContext.CurrentLanguage, out string? result) == false)
                    return "<unknown-language>";

                return result;
            }
        }

        public bool IsMatching(SearchStatement searchStatement)
        {
            LocalizationContext? localizationContext = LocalizationContext.DefaultContext;

            if (localizationContext is null)
                return false;

            if (localizableString.TryGetValue(localizationContext.CurrentLanguage, out string? result) == false)
                return false;

            return searchStatement.IsMatching(result);
        }

        public virtual void Dispose()
        {
            if (LocalizationContext.DefaultContext != null)
                LocalizationContext.DefaultContext.CurrentLanguageChanged -= LanguageChanged!;

            LocalizationContext.DefaultContextChanged -= DefaultLocalizationContextChanged!;

            GC.SuppressFinalize(this);
        }
    }
}
