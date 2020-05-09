using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFMainDisplayingWindow
{
    public static class Designer
    {
        public static void SetButtonDesign(Button b, string content)
        {
            b.Content = content;
            b.HorizontalContentAlignment = HorizontalAlignment.Center;
            b.VerticalContentAlignment = VerticalAlignment.Center;
            b.FontWeight = FontWeights.Bold;
            b.Margin = new Thickness(Constants.LayoutMargin);
            b.FontSize = 20;
        }

        public static void SetLabelDesign(Label l, string content, bool large, bool black)
        {
            l.Content = content;
            l.HorizontalContentAlignment = HorizontalAlignment.Center;
            l.VerticalContentAlignment = VerticalAlignment.Center;
            l.FontWeight = FontWeights.Bold;
            l.Margin = new Thickness(Constants.LayoutMargin);
            l.FontSize = large ? 24 : 13;
            l.FontFamily = new FontFamily("Courier New");
            l.Foreground = new SolidColorBrush(black ? Colors.Black : Colors.White);
        }

        public static Label GetDesignedLabel(string content, bool large, bool black)
        {
            Label l = new Label();
            SetLabelDesign(l, content, large, black);
            return l;
        }
    }
}