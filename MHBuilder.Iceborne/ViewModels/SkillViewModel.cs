using MHBuilder.Iceborne.Models;
using MHBuilder.WPF;
using MHBuilder.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public LocalizableStringViewModel Name { get; }

        public SkillViewModel(Skill skill)
        {
            Skill = skill;

            Name = new LocalizableStringViewModel(skill.Name);
        }
    }
}
