using System.Drawing;

namespace SimpleGeneticCode
{
    public interface ICell
    {
        public Point Position { get; }
        public World Environment { get; }
        public void Action();
        public void Remove();
    }
}