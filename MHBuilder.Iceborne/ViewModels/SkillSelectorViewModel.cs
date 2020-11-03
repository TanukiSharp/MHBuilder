using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Iceborne.ViewModels
{
    public class SkillSelectorViewModel : RootedViewModel
    {
        public string Test { get; } = " This is a binding test";

        public SkillSelectorViewModel(RootViewModel rootViewModel)
            : base(rootViewModel)
        {
        }
    }
}
