using System;

namespace TemporaryContainer
{
    class Map
    {/*
        #region
        public Size MapSize { get; set; }
        public Bitmap Bitmap { get; set; }
        public Size MatrixSize { get; }
        public World Image { get; }
        
        Size windowSize;
        Size cellSize;

        public Map(World world)
        {
            Image = world;
            windowSize = new Size(Constants.XWindow, Constants.YWindow);
            MatrixSize = new Size(Constants.CellsCountX, Constants.CellsCountY);
            ProcessSizeChanging(windowSize);
            DrawMap();
        }

        public void NextTick()
        {
            Image.Tick();
            DrawMap();
        }

        public void ProcessSizeChanging(Size newSize)
        {
            windowSize = newSize;
            MapSize = new Size((int)(Constants.FieldRatioX * newSize.Width), (int)(Constants.FieldRatioY * newSize.Height));
            Bitmap = new Bitmap((int)(Constants.FieldRatioX * newSize.Width), (int)(Constants.FieldRatioY * newSize.Height));
            cellSize = new Size(MapSize.Width / MatrixSize.Width, MapSize.Height / MatrixSize.Height);
        }

        void DrawMap()
        {
            Bitmap = new Bitmap((int)(Constants.FieldRatioX * windowSize.Width), (int)(Constants.FieldRatioY * windowSize.Height));
            IEnumerable<ICell> filledCells = Image.GetOccupiedCells();
            foreach (ICell currentCell in filledCells)
                DrawCell(currentCell);
        }

        void DrawCell(ICell cell)
        {
            Point upperLeft = new Point(cellSize.Width * cell.Position.X, cellSize.Height * cell.Position.Y);
            for (int i = upperLeft.Y; i < upperLeft.Y + cellSize.Height; i++)
                for (int j = upperLeft.X; j <upperLeft.X + cellSize.Width; j++)
                    Bitmap.SetPixel(j, i, cell.Color);
            if (Math.Min(cellSize.Width,cellSize.Height) < 5)
                return;
            for (int i = upperLeft.Y; i < upperLeft.Y + cellSize.Height; i++)
            {
                Bitmap.SetPixel(upperLeft.X,i, Color.Black);
                Bitmap.SetPixel(upperLeft.X + cellSize.Width - 1,i, Color.Black);
            }
            for (int i = upperLeft.X; i < upperLeft.X + cellSize.Width; i++)
            {
                Bitmap.SetPixel(i, upperLeft.Y, Color.Black);
                Bitmap.SetPixel(i, upperLeft.Y + cellSize.Height - 1, Color.Black);
            }
        }
        #endregion
    */}
    static class Constants
    {
        public const double FieldRatioX = 1;
        public const double FieldRatioY = 0.75;
        public const int XWindow = 800;
        public const int YWindow = 800;
        public const int CellsCountX = 100;
        public const int CellsCountY = 100;
    }
    /*
     <Window x:Class="MainWindow.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:MainWindow"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height="2*"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        <TextBlock Background="Aqua" Grid.Row="0" Grid.Column="0" FontSize="20" TextAlignment="Center" VerticalAlignment="Center"/>
        <TextBlock Background="Gold" Grid.Row="1" Grid.Column="0" FontSize="20" TextAlignment="Center" VerticalAlignment="Center"/>
    </Grid>
</Window>
*/
}