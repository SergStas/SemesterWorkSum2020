using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using SimpleGeneticCode;

namespace WPFMainDisplayingWindow
{
    public class OptionMenu : Window
    {
        Menu mainMenu;
        
        public OptionMenu(Menu menu)
        {
            mainMenu = menu;
            StackPanel panel = new StackPanel();
            Content = panel;
            foreach (FieldInfo field in typeof(Configurations).GetFields())
                panel.Children.Add(new ConfigurationBox(field.Name));
            Closed += (sender, args) => mainMenu.Show();
        }
    }
}