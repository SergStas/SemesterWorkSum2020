using System;
using System.Security.Cryptography.Pkcs;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using SimpleGeneticCode;

namespace WPFMainDisplayingWindow
{
    public class GameWidgetAssembler
    {
        public GridMap Map { get; private set; }
        public Grid OutputGrid { get; private set; }

        public int FramesPerSeconds
        {
            get => fps;
            set
            {
                if (value > 0 && value <= Constants.MaxFPS)
                {
                    fps = value;
                    timer.Interval = TimeSpan.FromMilliseconds(1000 / FramesPerSeconds);
                    fpsLabel.Content = $"FPS lock = {FramesPerSeconds}";
                }
            }
        }
        int fps = 30;
        DispatcherTimer timer;
        bool started;
        GameWindow gameWindow;

        Grid infoPanel;
        ICell subject;

        Grid buttonPanel;
        Button freezeButton;
        Button stepButton;
        Button incFPSButton;
        Button decFPSButton;
        Button menuButton;

        Grid statsPanel;
        Label iteratorLabel;
        Label fpsLabel;
        Label atmosphereLabel;
        Label countLabel;

        Color atmosphereColor;
        int iterationsCount;
        bool freezed = true;

        Func<ICell, GameWidgetAssembler, Button> visualizer = (cell, assembler) =>
        {
            Button result = new Button { Margin = new Thickness(Constants.CellsMargin) };
            SolidColorBrush brush = new SolidColorBrush(cell.Color);
            result.Background = brush;
            result.Click += (sender, args) => UpdateInfoPanel(cell, assembler);
            return result;
        };

        public GameWidgetAssembler(GameWindow window)
        {
            gameWindow = window;
            SetUp();
        }

        void SetUp()
        {
            SetOutputGrid();
            SetMap();
            SetInterfacePanels();
            SetTimer();
            UpdateAtmosphereColor();
        }

        public void Start()
        {
            started = true;
        }

        void SetOutputGrid()
        {
            OutputGrid = new Grid();
            OutputGrid.Background = new SolidColorBrush(Colors.DimGray);
            OutputGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)});
            OutputGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
            OutputGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) });
            OutputGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        void SetInterfacePanels()
        {
            SetButtonPanel();
            SetInfoPanel();
            SetStatsPanel();
        }

        void SetInfoPanel()
        {
            infoPanel = new Grid();
            infoPanel.Margin = new Thickness(Constants.LayoutMargin);
            OutputGrid.Children.Add(infoPanel);
            Grid.SetColumn(infoPanel, 1);
        }

        void SetInfoPanel(Grid panel)
        {
            OutputGrid.Children.Remove(infoPanel);
            infoPanel = panel;
            infoPanel.Margin = new Thickness(Constants.LayoutMargin);
            OutputGrid.Children.Add(infoPanel);
            Grid.SetColumn(infoPanel, 1);
        }

        void SetButtonPanel()
        {
            buttonPanel = new Grid();
            buttonPanel.ColumnDefinitions.Add(new ColumnDefinition());
            buttonPanel.ColumnDefinitions.Add(new ColumnDefinition());
            buttonPanel.ColumnDefinitions.Add(new ColumnDefinition());
            buttonPanel.RowDefinitions.Add(new RowDefinition());
            buttonPanel.RowDefinitions.Add(new RowDefinition());
            OutputGrid.Children.Add(buttonPanel);
            Grid.SetRow(buttonPanel, 1);
            SetFpsControlButtons();
            SetStepButton();
            SetStopButton();
            SetMenuButton();
        }

        void SetFpsControlButtons()
        {
            incFPSButton = new Button();
            Designer.SetButtonDesign(incFPSButton, "Speed Up");
            decFPSButton = new Button();
            Designer.SetButtonDesign(decFPSButton, "Speed Down");
            incFPSButton.Click += (sender, args) => FramesPerSeconds += Constants.FramesInc;
            decFPSButton.Click += (sender, args) => FramesPerSeconds -= Constants.FramesInc;
            buttonPanel.Children.Add(incFPSButton);
            buttonPanel.Children.Add(decFPSButton);
            Grid.SetRow(incFPSButton, 1);
            Grid.SetRow(decFPSButton, 1);
            Grid.SetColumn(incFPSButton, 1);
        }

        void SetStopButton()
        {
            freezeButton = new Button();
            Designer.SetButtonDesign(freezeButton, freezed ? "Unfreeze" : "Freeze");
            freezeButton.FontWeight = freezed ? FontWeights.ExtraBold : FontWeights.Bold;
            buttonPanel.Children.Add(freezeButton);
            freezeButton.Click += (sender, args) => Freeze();
        }

        void SetStepButton()
        {
            stepButton = new Button();
            Designer.SetButtonDesign(stepButton, "Step");
            stepButton.IsEnabled = freezed;
            stepButton.Click += (sender, args) => Tick(sender);
            stepButton.Click += (sender, args) =>
            {
                if (!(subject is null))
                    UpdateInfoPanel(subject, this);
            };
            buttonPanel.Children.Add(stepButton);
            Grid.SetColumn(stepButton, 1);
        }

        void SetMenuButton()
        {
            menuButton = new Button();
            Designer.SetButtonDesign(menuButton, "Menu");
            buttonPanel.Children.Add(menuButton);
            Grid.SetColumn(menuButton, 2);
            menuButton.Click += (sender, args) => OpenMenu();
        }

        void SetStatsPanel()
        {
            statsPanel = new Grid
            {
                Background = new SolidColorBrush(Colors.LightGray),
                Margin = new Thickness(Constants.LayoutMargin)
            };
            statsPanel.ColumnDefinitions.Add(new ColumnDefinition());
            statsPanel.ColumnDefinitions.Add(new ColumnDefinition());
            statsPanel.RowDefinitions.Add(new RowDefinition());
            statsPanel.RowDefinitions.Add(new RowDefinition());
            OutputGrid.Children.Add(statsPanel);
            Grid.SetRow(statsPanel, 1);
            Grid.SetColumn(statsPanel, 1);
            SetIterator();
            SetAtmosphereDisplay();
            SetFPSControlLabel();
            SetCountDisplay();
        }

        void SetIterator()
        {
            iteratorLabel = new Label();
            Designer.SetLabelDesign(iteratorLabel, iterationsCount.ToString(), true, true);
            statsPanel.Children.Add(iteratorLabel);
        }

        void SetAtmosphereDisplay()
        {
            atmosphereLabel = new Label();
            Designer.SetLabelDesign(atmosphereLabel, $"Atmosphere level:\n            {Map.World.AtmosphereThickness}", true, true);
            statsPanel.Children.Add(atmosphereLabel);
            Grid.SetRow(atmosphereLabel, 1);
        }

        void SetCountDisplay()
        {
            countLabel = new Label();
            Designer.SetLabelDesign(countLabel,
                $"Count: {Map.World.Size.Height * Map.World.Size.Width - Map.World.FreeSpace}", true, true);
            statsPanel.Children.Add(countLabel);
            Grid.SetRow(countLabel, 1);
            Grid.SetColumn(countLabel, 1);
        }

        void SetFPSControlLabel()
        {
            fpsLabel = new Label();
            Designer.SetLabelDesign(fpsLabel, $"FPS lock = {FramesPerSeconds}", true, true);
            statsPanel.Children.Add(fpsLabel);
            Grid.SetColumn(fpsLabel, 2);
        }

        void SetTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 / FramesPerSeconds);
            timer.Tick += (sender, args) => Tick(sender);
            timer.Start();
        }

        void Freeze()
        {
            freezed = !freezed;
            freezeButton.Content = freezed ? "Unfreeze" : "Freeze";
            freezeButton.FontWeight = freezed ? FontWeights.ExtraBold : FontWeights.Bold;
            stepButton.IsEnabled = freezed;
        }

        void Tick(object sender)
        {
            if (!started || freezed && !(sender is Button))
                return;
            Map.NextTick();
            iterationsCount++;
            iteratorLabel.Content = $"Iteration #{iterationsCount}";
            atmosphereLabel.Content = $"ATM: {Map.World.AtmosphereThickness}";
            countLabel.Content = $"Count: {Map.World.Size.Height * Map.World.Size.Width - Map.World.FreeSpace}";
            UpdateAtmosphereColor();
        }

        void SetMap()
        {
            World world = new World(Constants.CellsCountX, Constants.CellsCountY,
                Configurations.BotsStartCount, Configurations.BeginWithRandomProgram != 0);
            Map = new GridMap(world, visualizer, this);
            Map.Map.Margin = new Thickness(Constants.LayoutMargin);
            OutputGrid.Children.Add(Map.Map);
        }

        void OpenMenu()
        {
            Menu menu = new Menu();
            menu.Show();
            gameWindow.Close();
        }

        void UpdateAtmosphereColor()
        {
            byte gray = (byte)(255 - (double)(Map.World.AtmosphereThickness * 127 / SimpleGeneticCode.Configurations.MaxThickness));
            atmosphereColor = Color.FromRgb(gray, gray, gray);
            Map.Map.Background = new SolidColorBrush(atmosphereColor);
        }

        static void UpdateInfoPanel(object sender, GameWidgetAssembler assembler)
        {
            Grid panel = new Grid();
            ICell cell = (ICell)sender;
            assembler.subject = cell;
            StackPanel sp = new StackPanel();
            sp.Background = new SolidColorBrush(Colors.LightGray);
            panel.Children.Add(sp);
            sp.Children.Add(Designer.GetDesignedLabel($"Position: ({cell.Position.X}; {cell.Position.Y})", true, true));
            sp.Children.Add(Designer.GetDesignedLabel($"Energy: {cell.EnergyReserve}", true, true));
            sp.Children.Add(Designer.GetDesignedLabel($"Object - {(cell is Food ? "food" : "bot")}", true, true));
            assembler.SetInfoPanel(panel);
            if (cell is Food)
                return;
            Bot bot = (Bot)cell;
            sp.Children.Add(Designer.GetDesignedLabel($"Bot ID: {bot.Id}", true, true));
            sp.Children.Add(Designer.GetDesignedLabel($"Program: \n{bot.Program.GetCommandsString()}", true, true));
            sp.Children.Add(Designer.GetDesignedLabel($"Current: {bot.Program.Current}[{bot.Program.CommandPointer}] - {bot.Program.CommandName}", true, true));
        }
    }
}