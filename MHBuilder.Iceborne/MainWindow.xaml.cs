using MHBuilder.Core;
using MHBuilder.Iceborne.ViewModels;
using MHBuilder.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MHBuilder.Iceborne
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RootViewModel rootViewModel = new RootViewModel();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
        }

        private async void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            await rootViewModel.Initialize();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowManager.Show<SkillSelectorWindow>();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            bool? result = WindowManager.ShowDialog<SkillSelectorWindow>();
            Console.WriteLine(result);
        }
    }
}
