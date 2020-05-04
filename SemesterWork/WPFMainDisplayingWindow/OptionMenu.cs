using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SimpleGeneticCode;

namespace WPFMainDisplayingWindow
{
    public class OptionMenu : Window
    {
        Menu mainMenu;
        Grid mainGrid;
        Grid panel;
        Button closeButton;
        
        public OptionMenu(Menu menu)
        {
            mainMenu = menu;
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            Closed += (sender, args) => mainMenu.Show();
            SetGrid();
            Content = mainGrid;
        }

        void SetGrid()
        {
            mainGrid = new Grid
            {
                Background = new SolidColorBrush(Colors.DimGray)
            };
            mainGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(25, GridUnitType.Star)});
            mainGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
            SetPanel();
            SetButton();
        }

        void SetPanel()
        {
            panel = new Grid
            {
                Margin = new Thickness(Constants.MenuMargin)
            };
            mainGrid.Children.Add(panel);
            FieldInfo[] fields = typeof(Configurations).GetFields();
            int fieldsCount = fields.Length;
            for (int i = 0; i < fieldsCount; i++)
                if (fields[i].Name != "MaxValues")
                {
                    panel.RowDefinitions.Add(new RowDefinition());
                    ConfigurationBox box = new ConfigurationBox(fields[i].Name, fields[i].FieldType == typeof(double));
                    panel.Children.Add(box);
                    Grid.SetRow(box, i);
                }
        }

        void SetButton()
        {
            closeButton = new Button();
            Designer.SetButtonDesign(closeButton, "Set and return");
            mainGrid.Children.Add(closeButton);
            closeButton.Click += (sender, args) => Close();
            Grid.SetRow(closeButton, 1);
        }
    }
}