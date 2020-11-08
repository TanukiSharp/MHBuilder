using MHBuilder.Core;
using MHBuilder.Iceborne.ViewModels;
using MHBuilder.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MHBuilder.Iceborne
{
    /// <summary>
    /// Interaction logic for SkillSelectorWindow.xaml
    /// </summary>
    public partial class SkillSelectorWindow : Window, IManagedWindow
    {
        private SkillSelectorRootViewModel? skillSelectorRootViewModel;

        public SkillSelectorWindow()
        {
            InitializeComponent();
        }

        public void OnOpening(bool isAlreadyOpened, object? argument)
        {
            skillSelectorRootViewModel = (SkillSelectorRootViewModel?)argument;
            DataContext = skillSelectorRootViewModel;
        }

        public void OnCancel(CancellableOperationParameter cancellableOperationParameter)
        {
            if (skillSelectorRootViewModel is not null && string.IsNullOrWhiteSpace(skillSelectorRootViewModel.SearchText.Value) == false)
            {
                skillSelectorRootViewModel.SearchText.Value = string.Empty;
                cancellableOperationParameter.IsCancelled = true;
            }
        }
    }
}
