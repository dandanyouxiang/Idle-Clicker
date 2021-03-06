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
    /// Interaction logic for BuildingItem.xaml
    /// </summary>
    public partial class BuildingItem : UserControl
    {
        Building building;
        Brush defaultTextBrush = new SolidColorBrush(Color.FromRgb(228, 181, 123));
        Brush grayItemBrush = new SolidColorBrush(Color.FromRgb(139, 0, 0));
        Brush defaultItemBrush = new SolidColorBrush(Color.FromRgb(73, 36, 18));

        public BuildingItem(Building building)
        {
            InitializeComponent();
            this.building = building;
            this.building.OnChangeLevel += UpdateRequirements;
            GameEngine.GameTimer.OnTick += GameTimer_OnTick;
        
        }

        private void GameTimer_OnTick(TickEventArgs e)
        {
            UpdateRequirements(building.Level);
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            InfoPopup.IsOpen = true;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            InfoPopup.IsOpen = false;
        }

        public void UpdateRequirements(int Level)
        {
            this.BuildingLevelValue.Text = Level.ToString();
            bool ifCompleted = true;

            for (int i = 0; i < building.Requirements.Count; i++)
            {
                if (!building.Requirements[i].CheckIfCompleted())
                {
                    ifCompleted = false;
                    //BuildingItemMainGrid.Cursor = Cursors.Arrow; // AK: Nie wiem czy nie za chamsko
                    if ((building.Requirements[i].requiredObject.RequireType & RequireType.BuildingOrMaterial) != 0)
                    {
                        ((ResourceInfo)LogicalTreeHelper.FindLogicalNode(this, "w" + i)).ResourceCountTB.Text = building.Requirements[i].RequireValue.ToString();
                        ((ResourceInfo)LogicalTreeHelper.FindLogicalNode(this, "w" + i)).ResourceCountTB.Foreground = Brushes.Red;
                    }
                    else
                    {
                        ((OtherRequirementLine)LogicalTreeHelper.FindLogicalNode(this, "w" + i)).ResourceTextTB.Text = building.Requirements[i].RequireValue.ToString();
                        ((OtherRequirementLine)LogicalTreeHelper.FindLogicalNode(this, "w" + i)).ResourceTextTB.Foreground = Brushes.Red;
                    }
                }
                else
                {
                    
                    BuildingItemMainGrid.Cursor = Cursors.Hand;
                    if ((building.Requirements[i].requiredObject.RequireType & RequireType.BuildingOrMaterial) != 0)
                    {
                        ((ResourceInfo)LogicalTreeHelper.FindLogicalNode(this, "w" + i)).ResourceCountTB.Text = building.Requirements[i].RequireValue.ToString();
                        ((ResourceInfo)LogicalTreeHelper.FindLogicalNode(this, "w" + i)).ResourceCountTB.Foreground = defaultTextBrush;
                    }
                    else
                    {
                        ((OtherRequirementLine)LogicalTreeHelper.FindLogicalNode(this, "w" + i)).ResourceTextTB.Text = building.Requirements[i].RequireValue.ToString();
                        ((OtherRequirementLine)LogicalTreeHelper.FindLogicalNode(this, "w" + i)).ResourceTextTB.Foreground = defaultTextBrush;
                    }
                }
            }

            if (ifCompleted)
                BuildingItemMainGrid.Background = defaultItemBrush;
            else
                BuildingItemMainGrid.Background = grayItemBrush;

        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            building.Build();
        }
    }
}
