using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SimpleGeneticCode;
using Timer = System.Timers;

namespace WPFMainDisplayingWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int iterationsCount;
        GridMap map;
        TextBox iteratorBox;
        Button freezeButton;
        bool freezed;
        
        public MainWindow()
        {
            InitializeComponent();
            map = CreateMap();
            DispatcherTimer timer = new DispatcherTimer();
            SetInterface();
            timer.Interval = TimeSpan.FromMilliseconds(1000 / Constants.FPS);
            timer.Tick += (sender, args) => Tick();
            timer.Start();
        }

        void SetInterface()
        {
            SetIterator();
            SetStopButton();
        }

        void SetIterator()
        {
            iteratorBox = new TextBox();
            outputGrid.Children.Add(iteratorBox);
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
            outputGrid.Children.Add(freezeButton);
            Grid.SetColumn(freezeButton, 1);
            Grid.SetRow(freezeButton, 1);
            freezeButton.Click += (sender, args) => Freeze();
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
            map.NextTick();
            iterationsCount++;
            iteratorBox.Text = iterationsCount.ToString();
        }

        GridMap CreateMap()
        {
            BotProgram.UploadCommands(BasicCommands.GetBasicCommands());
            World world = new World(Constants.CellsCountX, Constants.CellsCountY, SimpleGeneticCode.Constants.BotsStartCount);
            GridMap map = new GridMap(world);
            outputGrid.Children.Add(map.Map);
            return map;
        }

        Grid GetInfoPanel()
        {
            Grid panel = new Grid();
            throw new NotImplementedException();
        }
    }
}