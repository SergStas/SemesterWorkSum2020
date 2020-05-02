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
        //bool isDouble = false;
        private double maxValue = 100;

        Slider slider;
        Label valueLabel;
        Label nameLabel;

        public ConfigurationBox(string name)
        {
            OptionName = name;
            fieldValue = typeof(Configurations).GetField(OptionName).GetValue(null);
            SetUp();
        }

        public ConfigurationBox(string name, double max) : this(name) => maxValue = max;

        //public ConfigurationBox(string name, bool d) : this(name) => isDouble = d;

        //public ConfigurationBox(string name, double max, bool d) : this(name, max) => isDouble = d;

        void SetUp()
        {
            Margin = new Thickness(Constants.MenuMargin);
            ShowGridLines = true;
            ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(2, GridUnitType.Star)});
            ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(2, GridUnitType.Star)});
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
            slider.Value = (double)fieldValue;
            slider.Minimum = 0;
            slider.Maximum = maxValue;
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
            typeof(Configurations).GetField(OptionName).SetValue(null, fieldValue);
        }
    }
}