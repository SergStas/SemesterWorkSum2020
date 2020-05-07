using System.Windows;

namespace WPFMainDisplayingWindow
{
    public class GameWindow : Window
    {
        GameWidgetAssembler assembler;
        
        public GameWindow()
        {
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            assembler = new GameWidgetAssembler(this);
            Content = assembler.OutputGrid;
            Show();
            Start();
        }

        public void Start()
        {
            assembler.Start();
        }
        
    }
}