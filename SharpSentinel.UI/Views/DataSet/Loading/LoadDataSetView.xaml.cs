﻿using System;
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
using SharpSentinel.UI.Services.Window;

namespace SharpSentinel.UI.Views.DataSet
{
    /// <summary>
    /// Interaction logic for LoadDataSetView.xaml
    /// </summary>
    public partial class LoadDataSetView : UserControl, IWindow
    {
        public LoadDataSetView()
        {
            InitializeComponent();
        }

        public WindowSettings GetWindowSettings()
        {
            return WindowSettings.Standard().Resize(ResizeMode.NoResize).Width(500).Height(200).SizeToContent(SizeToContent.Manual);
        }
    }
}
