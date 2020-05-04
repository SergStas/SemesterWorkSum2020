using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFMainDisplayingWindow
{
    public class Menu : Window
    {
        Grid menuGrid;
        Grid buttonPanel;

        Button startButton;
        Button parameterButton;

        OptionMenu options;
        
        public Menu()
        {
            CreateGrid();
            Content = menuGrid;
            Width = 250;
            Height = 150;
            Show();
        }

        void CreateGrid()
        {
            menuGrid = new Grid
            {
                Background = new SolidColorBrush(Colors.DimGray)
            };
            menuGrid.ColumnDefinitions.Add(new ColumnDefinition());
            menuGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width = new GridLength(4, GridUnitType.Star)});
            menuGrid.ColumnDefinitions.Add(new ColumnDefinition());
            menuGrid.RowDefinitions.Add(new RowDefinition());
            menuGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(8, GridUnitType.Star)});
            menuGrid.RowDefinitions.Add(new RowDefinition());
            SetButtonPanel();
        }

        void SetButtonPanel()
        {
            buttonPanel = new Grid();
            buttonPanel.RowDefinitions.Add(new RowDefinition());
            buttonPanel.RowDefinitions.Add(new RowDefinition());
            menuGrid.Children.Add(buttonPanel);
            Grid.SetRow(buttonPanel, 1);
            Grid.SetColumn(buttonPanel, 1);
            SetStartButton();
            SetParameterButton();
        }
        
        void SetStartButton()
        {
            startButton = new Button();
            Designer.SetButtonDesign(startButton, "Start");
            buttonPanel.Children.Add(startButton);
            startButton.Click += (sender, args) =>
            {
                Hide();
                GameWindow window = new GameWindow();
                window.Show();
                window.Start();
                Close();
            };
        }

        void SetParameterButton()
        {
            parameterButton = new Button();
            Designer.SetButtonDesign(parameterButton, "Configure");
            buttonPanel.Children.Add(parameterButton);
            Grid.SetRow(parameterButton, 1);
            parameterButton.Click += (sender, args) =>
            {
                Hide();
                options = new OptionMenu(this);
                options.Show();
            };
        }
    }
}