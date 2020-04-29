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
    public class WidgetAssembler
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

        Grid infoPanel;
        ICell subject;

        Grid buttonPanel;
        Button freezeButton;
        Button stepButton;
        Button incFPSButton;
        Button decFPSButton;

        Grid statsPanel;
        Label iteratorLabel;
        Label fpsLabel;
        Label atmosphereLabel;
        
        int iterationsCount;
        bool freezed;

        Func<ICell, WidgetAssembler, Button> visualizer = (cell, assembler) =>
        {
            Button result = new Button { Margin = new Thickness(Constants.CellsMargin) };
            SolidColorBrush brush = new SolidColorBrush(cell.Color);
            result.Background = brush;
            result.Click += (sender, args) => UpdateInfoPanel(cell, assembler);
            return result;
        };

        public WidgetAssembler() => SetUp();

        void SetUp()
        {
            SetOutputGrid();
            SetMap();
            SetInterfacePanels();
            SetTimer();
        }

        void SetOutputGrid()
        {
            OutputGrid = new Grid();
            OutputGrid.ShowGridLines = true;
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
            buttonPanel.RowDefinitions.Add(new RowDefinition());
            buttonPanel.RowDefinitions.Add(new RowDefinition());
            buttonPanel.ShowGridLines = true;
            OutputGrid.Children.Add(buttonPanel);
            Grid.SetColumn(buttonPanel, 1);
            Grid.SetRow(buttonPanel, 1);
            SetFPSControlButtons();
            SetStepButton();
            SetStopButton();
        }

        void SetFPSControlButtons()
        {
            incFPSButton = new Button
            {
                Content = "Speed Up", 
                Background = new SolidColorBrush(Colors.ForestGreen),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(Constants.LayoutMargin)
            };
            decFPSButton = new Button
            {
                Content = "Speed Down", 
                Background = new SolidColorBrush(Colors.ForestGreen),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(Constants.LayoutMargin)
            };
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
            freezeButton = new Button
            {
                Content = "Freeze", 
                Background = new SolidColorBrush(Colors.Turquoise),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(Constants.LayoutMargin)
            };
            buttonPanel.Children.Add(freezeButton);
            freezeButton.Click += (sender, args) => Freeze();
        }

        void SetStepButton()
        {
            stepButton = new Button
            {
                Content = "Step",
                Background = new SolidColorBrush(Colors.Turquoise),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(Constants.LayoutMargin),
                IsEnabled = false
            };
            stepButton.Click += (sender, args) => Tick(sender);
            stepButton.Click += (sender, args) =>
            {
                if (!(subject is null))
                    UpdateInfoPanel(subject, this);
            };
            buttonPanel.Children.Add(stepButton);
            Grid.SetColumn(stepButton, 1);
        }

        void SetStatsPanel()
        {
            statsPanel = new Grid();
            statsPanel.ColumnDefinitions.Add(new ColumnDefinition());
            statsPanel.ColumnDefinitions.Add(new ColumnDefinition());
            statsPanel.ColumnDefinitions.Add(new ColumnDefinition());
            statsPanel.ShowGridLines = true;
            OutputGrid.Children.Add(statsPanel);
            Grid.SetRow(statsPanel, 1);
            SetIterator();
            SetAtmosphereDisplay();
            SetFPSControlLabel();
        }

        void SetIterator()
        {
            iteratorLabel = new Label
            {
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(Constants.LayoutMargin)
            };
            statsPanel.Children.Add(iteratorLabel);
        }

        void SetAtmosphereDisplay()
        {
            atmosphereLabel = new Label
            {
                Content = $"Atmosphere level:\n            {Map.World.AtmosphereThickness}",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(Constants.LayoutMargin)
            };
            statsPanel.Children.Add(atmosphereLabel);
            Grid.SetColumn(atmosphereLabel, 1);
        }

        void SetFPSControlLabel()
        {
            fpsLabel = new Label
            {
                Content = $"FPS lock = {FramesPerSeconds}",
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(Constants.LayoutMargin)
            };
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
            if (freezed && !(sender is Button))
                return;
            Map.NextTick();
            iterationsCount++;
            iteratorLabel.Content = $"Iteration #{iterationsCount}";
            atmosphereLabel.Content = $"Atmosphere level:\n            {Map.World.AtmosphereThickness}";
        }

        void SetMap()
        {
            BotProgram.UploadCommands(BasicCommands.GetBasicCommands());
            World world = new World(Constants.CellsCountX, Constants.CellsCountY,
                SimpleGeneticCode.Constants.BotsStartCount, SimpleGeneticCode.Constants.BeginWithRandomProgram);
            Map = new GridMap(world, visualizer, this);
            Map.Map.Margin = new Thickness(Constants.CellsMargin);
            OutputGrid.Children.Add(Map.Map);
        }

        static void UpdateInfoPanel(object sender, WidgetAssembler assembler)
        {
            Grid panel = new Grid();
            ICell cell = (ICell)sender;
            assembler.subject = cell;
            StackPanel sp = new StackPanel();
            panel.Children.Add(sp);
            sp.Children.Add(new Label { Content = $"Position: ({cell.Position.X}; {cell.Position.Y})" });
            sp.Children.Add(new Label { Content = $"Energy: {cell.EnergyReserve}" });
            sp.Children.Add(new Label { Content = $"Object - {(cell is Food ? "food" : "bot")}" });
            assembler.SetInfoPanel(panel);
            if (cell is Food)
                return;
            Bot bot = (Bot)cell;
            sp.Children.Add(new Label { Content = $"Bot ID: {bot.Id}" });
            sp.Children.Add(new Label { Content = $"Program: \n{bot.Program.GetCommandsString()}" });
            sp.Children.Add(new Label
            {
                Content = $"Current: {bot.Program.Current}[{bot.Program.CommandPointer}] - {bot.Program.CommandName}"
            });
        }
    }
}