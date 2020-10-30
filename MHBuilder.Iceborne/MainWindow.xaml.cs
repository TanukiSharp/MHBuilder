using MHBuilder.WPF;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Globals.MasterData.GetSkills();

            WindowManager.Show<SkillSelectorWindow>();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            bool? result = WindowManager.ShowDialog<SkillSelectorWindow>();
            Console.WriteLine(result);
        }
    }
}
