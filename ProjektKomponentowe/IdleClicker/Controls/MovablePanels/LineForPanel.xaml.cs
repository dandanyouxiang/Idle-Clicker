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

namespace IdleClicker
{
    /// <summary>
    /// Interaction logic for LineForPanel.xaml
    /// </summary>
    public partial class LineForPanel : UserControl
    {
        public LineForPanel()
        {
            InitializeComponent();
        }

        public LineForPanel(string property, string value)
        {
            InitializeComponent();
            TextLineTB.Text = property;
            TextNumberTB.Text = value;
        }
    }
}
