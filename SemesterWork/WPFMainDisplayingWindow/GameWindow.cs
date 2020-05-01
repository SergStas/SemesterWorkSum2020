using System.Windows;

namespace WPFMainDisplayingWindow
{
    public class GameWindow : Window
    {
        WidgetAssembler assembler;
        
        public GameWindow()
        {
            Width = 800;
            Height = 600;
            assembler = new WidgetAssembler(this);
            Content = assembler.OutputGrid;
            Show();
        }

        public void Start()
        {
            assembler.Start();
        }
        
    }
}