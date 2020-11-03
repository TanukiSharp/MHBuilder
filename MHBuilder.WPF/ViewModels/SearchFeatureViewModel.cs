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

        private string? searchText;
        public string? SearchText
        {
            get { return searchText; }
            set
            {
                if (SetValue(ref searchText, value))
                {
                    var searchStatement = SearchStatement.Create(searchText, null);
                    onSearchTextChanged(searchStatement);
                }
            }
        }
    }
}
