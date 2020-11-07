using MHBuilder.Iceborne.Models;
using MHBuilder.WPF;
using MHBuilder.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MHBuilder.Iceborne.ViewModels
{
    public class SkillViewModel : ViewModelBase
    {
        public Skill Skill { get; }

        private bool isVisible = true;
        public bool IsVisible
        {
            get { return isVisible; }
            set { SetValue(ref isVisible, value); }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetValue(ref isSelected, value); }
        }

        public LocalizableStringViewModel Name { get; }

        public ICommand ToggleSkillSelectionCommand { get; }

        public SkillViewModel(Skill skill)
        {
            Skill = skill;

            Name = new LocalizableStringViewModel(skill.Name);

            ToggleSkillSelectionCommand = new AnonymousCommand(OnToggleSkillSelection);
        }

        private void OnToggleSkillSelection()
        {
            IsSelected = !IsSelected;
        }
    }
}
