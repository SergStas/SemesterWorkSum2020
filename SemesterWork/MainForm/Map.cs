using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace MainForm
{
    class Map
    {
        public Size MapSize { get; set; }
        public Bitmap Bitmap { get; set; }
        public Size MatrixSize { get; }

        Size cellSize;

        public Map()
        {
            MatrixSize = new Size(Constants.CellsCountX, Constants.CellsCountY);
            ProcessSizeChanging(new Size(Constants.XWindow, Constants.YWindow));

        }

        public void ProcessSizeChanging(Size windowSize)
        {
            MapSize = new Size((int)(Constants.FieldRatioX * windowSize.Width), (int)(Constants.FieldRatioY * windowSize.Height));
            Bitmap = new Bitmap((int)(Constants.FieldRatioX * windowSize.Width), (int)(Constants.FieldRatioY * windowSize.Height));
            cellSize = new Size(MapSize.Width / MatrixSize.Width, MapSize.Height / MatrixSize.Height);

        }

        public void CrookedNail()
        {

        }
    }
}