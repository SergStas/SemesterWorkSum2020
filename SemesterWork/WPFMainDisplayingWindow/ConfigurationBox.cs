using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using SimpleGeneticCode;

namespace WPFMainDisplayingWindow
{
    public class ConfigurationBox : Grid
    {
        public string OptionName { get; }

        object fieldValue;
        bool isDouble;
        private double maxValue = 1000;

        Slider slider;
        Label valueLabel;
        Label nameLabel;

        public ConfigurationBox(string name, bool d)
        {
            isDouble = d;
            OptionName = name;
            fieldValue = typeof(Configurations).GetField(OptionName).GetValue(null);
            SetUp();
        }

        public ConfigurationBox(string name, double max, bool d)
        {
            isDouble = d;
            OptionName = name;
            maxValue = max;
            fieldValue = typeof(Configurations).GetField(OptionName).GetValue(null);
            SetUp();
        }

        void SetUp()
        {
            Margin = new Thickness(Constants.MenuMargin);
            ShowGridLines = true;
            ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(4, GridUnitType.Star)});
            ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
            ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(5, GridUnitType.Star)});
            SetLabels();
            SetSlider();
        }

        void SetLabels()
        {
            nameLabel = new Label
            {
                Content = OptionName
            };
            valueLabel = new Label
            {
                Content = fieldValue
            };
            Children.Add(valueLabel);
            Children.Add(nameLabel);
            SetColumn(valueLabel, 1);
        }

        void SetSlider()
        {
            slider = new Slider();
            slider.Minimum = 0;
            slider.Maximum = maxValue;
            slider.Value = double.Parse(fieldValue.ToString()); // - иначе не работает InvalidCasException из Int32 в Double
            slider.ValueChanged+= (sender, args) =>
            {
                fieldValue = slider.Value;
                ChangeValue();
            };
            Children.Add(slider);
            SetColumn(slider,2);
        }

        void ChangeValue()
        {
            valueLabel.Content = isDouble ? double.Parse(fieldValue.ToString()) : (int)double.Parse(fieldValue.ToString());
            if (isDouble)
                typeof(Configurations).GetField(OptionName).SetValue(null, double.Parse(fieldValue.ToString()));
            else
                typeof(Configurations).GetField(OptionName).SetValue(null, (int)double.Parse(fieldValue.ToString()));
                
        }
    }
}