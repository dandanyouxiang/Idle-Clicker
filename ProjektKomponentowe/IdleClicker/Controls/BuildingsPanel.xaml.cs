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
    /// Interaction logic for BuildingsPanel.xaml
    /// </summary>
    public partial class BuildingsPanel : UserControl
    {
        public BuildingsPanel()
        {
            InitializeComponent();
        }

        public void ImportBuildings(List<Building> buildings)
        {
            ResourceInfo newResourceInfo;
            BuildingItem newItem;
            OtherRequirementLine line;

            foreach (Building item in buildings)
            {
                newItem = new BuildingItem(item);
                
                newItem.BuildingImage.Source = item.IconSource;
                newItem.BuildingName.Text = Dictionary.dictionary[item.Key];
                newItem.BuildingLevelValue.Text = item.Level.ToString();

                for (int i = 0; i < item.Requirements.Count; i++)
                {
                    if ((item.Requirements[i].requiredObject.RequireType & RequireType.BuildingOrMaterial) != 0)
                    {
                        newResourceInfo = new ResourceInfo();
                        newResourceInfo.Name = "w" + i;
                        newResourceInfo.ResourceIconME.Source = item.Requirements[i].requiredObject.GetIcon();
                        newResourceInfo.ResourceCountTB.Text = item.Requirements[i].RequireValue.ToString();

                        if (!item.Requirements[i].CheckIfCompleted())
                        {
                            newResourceInfo.ResourceCountTB.Foreground = Brushes.Red;
                        }

                        if (item.Requirements[i].requiredObject.RequireType == RequireType.Material)
                            newItem.BuildingRequireMaterials.Children.Add(newResourceInfo);
                        else
                            newItem.BuildingRequireBuildings.Children.Add(newResourceInfo);                   
                    }
                    else
                    {
                        line = new OtherRequirementLine();
                        line.ResourceTextTB.Text = item.Requirements[i].RequireValue.ToString();
                        if (item.Requirements[i].requiredObject.GetIcon() != null)
                        {
                            line.ResourceIconME.Source = item.Requirements[i].requiredObject.GetIcon();
                        }
                        else
                        {
                            line.ResourceIconME.Visibility = Visibility.Hidden;
                        }
                        newItem.BuildingOtherRequirements.Children.Add(line);
                    }
                }

                this.BuildingListStackPanel.Children.Add(newItem);
            }


        }
    }
}
