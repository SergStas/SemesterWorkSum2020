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
        public MainWindow()
        {
            InitializeComponent();
            GridMap map = CreateMap();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 / Constants.FPS);
            int iterator = 0;
            TextBox iteratorBox = new TextBox();
            timer.Tick += (sender, args) =>
            {
                map.NextTick();
                iterator++;
                iteratorBox.Text = iterator.ToString();
            };
            outputGrid.Children.Add(iteratorBox);
            Grid.SetColumn(iteratorBox, 0);
            Grid.SetRow(iteratorBox, 1);
            timer.Start();
        }

        GridMap CreateMap()
        {
            BotProgram.UploadCommands(BasicCommands.GetBasicCommands());
            World world = new World(Constants.CellsCountX, Constants.CellsCountY, 50);
            GridMap map = new GridMap(world);
            outputGrid.Children.Add(map.Map);
            Grid.SetColumn(map.Map, 0);
            Grid.SetRow(map.Map, 0);
            return map;
        }
    }
}