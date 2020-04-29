using System;
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
        
        int iterationsCount;
        TextBox iteratorBox;
        Button freezeButton;
        bool freezed;
        Grid infoPanel;

        Func<ICell, WidgetAssembler, Button> visualizer = (cell, assembler) =>
        {
            Button result = new Button { Margin = new Thickness(Constants.GraphicsMargin) };
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
            SetTimer();
            SetIterator();
            SetStopButton();
            SetInfoPanel();
        }

        void SetOutputGrid()
        {
            OutputGrid = new Grid();
            OutputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            OutputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            OutputGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) });
            OutputGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        void SetInfoPanel()
        {
            infoPanel = new Grid();
            OutputGrid.Children.Add(infoPanel);
            Grid.SetColumn(infoPanel, 1);
        }

        void SetInfoPanel(Grid panel)
        {
            OutputGrid.Children.Remove(infoPanel);
            infoPanel = panel;
            OutputGrid.Children.Add(infoPanel);
            Grid.SetColumn(infoPanel, 1);
        }

        void SetIterator()
        {
            iteratorBox = new TextBox();
            OutputGrid.Children.Add(iteratorBox);
            Grid.SetColumn(iteratorBox, 0);
            Grid.SetRow(iteratorBox, 1);
        }

        void SetStopButton()
        {
            freezeButton = new Button
            {
                Content = "Freeze", 
                Background = new SolidColorBrush(Colors.Crimson),
                FontWeight = FontWeights.Bold
            };
            OutputGrid.Children.Add(freezeButton);
            Grid.SetColumn(freezeButton, 1);
            Grid.SetRow(freezeButton, 1);
            freezeButton.Click += (sender, args) => Freeze();
        }

        void SetTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 / Constants.FPS);
            timer.Tick += (sender, args) => Tick();
            timer.Start();
        }

        void Freeze()
        {
            freezed = !freezed;
            freezeButton.Content = freezed ? "Unfreeze" : "Freeze";
            freezeButton.FontWeight = freezed ? FontWeights.ExtraBold : FontWeights.Bold;
        }

        void Tick()
        {
            if (freezed)
                return;
            Map.NextTick();
            iterationsCount++;
            iteratorBox.Text = iterationsCount.ToString();
        }

        void SetMap()
        {
            BotProgram.UploadCommands(BasicCommands.GetBasicCommands());
            World world = new World(Constants.CellsCountX, Constants.CellsCountY,
                SimpleGeneticCode.Constants.BotsStartCount, SimpleGeneticCode.Constants.BeginWithRandomProgram);
            Map = new GridMap(world, visualizer, this);
            OutputGrid.Children.Add(Map.Map);
        }

        static void UpdateInfoPanel(object sender, WidgetAssembler assembler)
        {
            Grid panel = new Grid();
            ICell cell = (ICell)sender;
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
            sp.Children.Add(new Label { Content = $"Current: {bot.Program.Current} - {bot.Program.CommandName}" });
        }
    }
}