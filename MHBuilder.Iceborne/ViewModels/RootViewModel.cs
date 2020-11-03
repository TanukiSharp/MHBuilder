using MHBuilder.Iceborne.Models;
using MHBuilder.WPF;
using MHBuilder.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Iceborne.ViewModels
{
    public class RootViewModel : ViewModelBase
    {
        private readonly ObservableCollection<SkillViewModel> skills = new ObservableCollection<SkillViewModel>();
        public ReadOnlyObservableCollection<SkillViewModel> Skills { get; }

        public RootViewModel()
        {
            Skills = new ReadOnlyObservableCollection<SkillViewModel>(skills);
        }

        public async Task Initialize()
        {
            await InitializeSkills();
        }

        private async Task InitializeSkills()
        {
            Skill[] skillModels = await Globals.MasterData.GetSkills();

            await EnumerableExtensions.IterateWithAirSpace(
                skillModels,
                10,
                (item) => skills.Add(new SkillViewModel(item))
            );
        }
    }

    public abstract class RootedViewModel : ViewModelBase
    {
        public RootViewModel RootViewModel { get; }

        protected RootedViewModel(RootViewModel rootViewModel)
        {
            RootViewModel = rootViewModel;
        }
    }
}
