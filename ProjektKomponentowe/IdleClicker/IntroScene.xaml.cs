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
    /// Interaction logic for IntroScene.xaml
    /// </summary>
    public partial class IntroScene : UserControl, IScene
    {
        SceneController sceneController;

        public IntroScene()
        {
            InitializeComponent();
        }

        public void Close()
        {
            
        }

        public void Load()
        {
            
        }

        public void SetSceneController(SceneController sc)
        {
            sceneController = sc;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            sceneController.LoadScene<MainMenuScene>(new MainMenuScene());
        }
    }
}