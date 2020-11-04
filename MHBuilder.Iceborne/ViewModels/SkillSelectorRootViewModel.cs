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
        public ReadOnlyObservableCollection<SkillViewModel> Skills
        {
            get { return RootViewModel.Skills; }
        }

        public SkillSelectorRootViewModel(RootViewModel rootViewModel)
            : base(rootViewModel)
        {
        }
    }
}
