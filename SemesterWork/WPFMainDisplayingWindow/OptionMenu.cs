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
            Width = 600;
            foreach (FieldInfo field in typeof(Configurations).GetFields())
            {
                if (field.Name == "MutationChance" || field.Name == "BeginWithRandomProgram")
                    panel.Children.Add(new ConfigurationBox(field.Name, 1, field.FieldType == typeof(double)));
                else
                    panel.Children.Add(new ConfigurationBox(field.Name, field.FieldType == typeof(double)));
            }

            Closed += (sender, args) => mainMenu.Show();
        }
    }
}