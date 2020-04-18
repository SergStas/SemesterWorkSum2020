using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleGeneticCode;

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
            BotProgram.UploadCommands(BasicCommands.GetBasicCommands());
            World world = new World(Constants.CellsCountX, Constants.CellsCountY, 10);
            GridMap map = new GridMap(world);
            outputGrid.Children.Add(map.Map);
            Grid.SetColumn(map.Map, 0);
            Grid.SetRow(map.Map, 0);
        }
    }
}