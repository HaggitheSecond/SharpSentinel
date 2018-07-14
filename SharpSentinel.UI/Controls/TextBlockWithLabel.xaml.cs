using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using SharpSentinel.Parser.Data.ManifestObjects;
using SharpSentinel.Parser.Helpers;

namespace SharpSentinel.UI.Controls
{
    /// <summary>
    /// Interaction logic for TextBlockWithLabel.xaml
    /// </summary>
    public partial class TextBlockWithLabel : UserControl
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            nameof(Label),
            typeof(string),
            typeof(TextBlockWithLabel),
            new PropertyMetadata(default(string)));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(string),
            typeof(TextBlockWithLabel),
            new PropertyMetadata(default(string), ValueChangedCallBack));

        private static void ValueChangedCallBack(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((TextBlockWithLabel)o)?.TrySetToolTip();
        }

        private void TrySetToolTip()
        {
            var path = this.GetBindingExpression(ValueProperty);

            if (path == null)
                return;

            var type = path.ResolvedSource.GetType();

            if (MethodHelper.TryGetPropertyDescription(type, path.ResolvedSourcePropertyName, out var description))
                this.ToolTip = description;

        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register(
            nameof(LabelWidth),
            typeof(double),
            typeof(TextBlockWithLabel),
            new PropertyMetadata(default(double)));

        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        public TextBlockWithLabel()
        {
            this.InitializeComponent();
        }
    }
}
