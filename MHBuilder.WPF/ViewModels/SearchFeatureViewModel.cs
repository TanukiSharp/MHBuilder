using MHBuilder.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.WPF.ViewModels
{
    public class SearchFeatureViewModel : ViewModelBase
    {
        private readonly Action<SearchStatement> onSearchTextChanged;

        public SearchFeatureViewModel(Action<SearchStatement> onSearchTextChanged)
        {
            this.onSearchTextChanged = onSearchTextChanged;
        }

        private string? value;
        public string? Value
        {
            get { return value; }
            set
            {
                if (SetValue(ref this.value, value))
                {
                    var searchStatement = SearchStatement.Create(value, null);
                    onSearchTextChanged(searchStatement);
                }
            }
        }
    }
}
