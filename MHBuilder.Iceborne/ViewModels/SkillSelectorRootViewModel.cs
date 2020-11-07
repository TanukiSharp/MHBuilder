using MHBuilder.Core;
using MHBuilder.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Iceborne.ViewModels
{
    public class SkillSelectorRootViewModel : RootedViewModel
    {
        public SearchFeatureViewModel SearchText { get; }

        public ReadOnlyObservableCollection<SkillViewModel> Skills
        {
            get { return RootViewModel.Skills; }
        }

        public SkillSelectorRootViewModel(RootViewModel rootViewModel)
            : base(rootViewModel)
        {
            SearchText = new SearchFeatureViewModel(OnSearchTextChanged);
        }

        private void OnSearchTextChanged(SearchStatement searchStatement)
        {
            if (searchStatement.IsEmpty)
            {
                foreach (SkillViewModel skillViewModel in Skills)
                    skillViewModel.IsVisible = true;

                return;
            }

            foreach (SkillViewModel skillViewModel in Skills)
                skillViewModel.IsVisible = skillViewModel.Name.IsMatching(searchStatement);
        }
    }
}
