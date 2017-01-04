﻿using System;
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

namespace IdleClicker
{
    /// <summary>
    /// Interaction logic for GameScene.xaml
    /// </summary>
    public partial class GameScene : Scene
    {
        public GameScene()
        {
            InitializeComponent();
            
            MainPanel.MenuButton.Click += (o, i) => { menuPanel.Visibility = menuPanel.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed; };
            buildButton.Click += (o, i) => { buildPanel.Visibility = buildPanel.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed; };
            menuPanel.exitButton.Click += (o, i) => { sceneController.LoadScene(new MainMenuScene()); };
            //  villageBackground.Source = new BitmapImage(new Uri("/IdleClicker;component/Resources/Images/VillageBackground.png", UriKind.Relative));

            GameEngine.Enabled = true;

            ListOfMaterials lista = new ListOfMaterials();
            GameEngine.SetListOfMaterials(lista);

            lista.NewMaterial += this.MainPanel.UpdateKindOfMaterials;

            Material m = new Material();
            m.Key = "WOOD";
            m.IconSource = new BitmapImage(new Uri("/IdleClicker;component/Resources/Images/wood.png", UriKind.Relative));
            m.CurrentAmount = 200;
            m.BeginningIncreaseQuantity = 2;
            m.CurrentIncreaseQuantity = 2;
            m.onChangeMaterial += this.MainPanel.UpdateCountOfMaterials;
            lista.AddNewMaterial(m);

            TickAction bonus = new TickAction(10, 1, 0);
            bonus.Actions += delegate ()
            {
                m.AddBonusQuantity(1, 60);
            };
            GameEngine.ActionList.AddAction(bonus);

            TownHallPanel ratusz = new TownHallPanel(new BitmapImage(new Uri("/IdleClicker;component/Resources/Images/wood.png", UriKind.Relative)), "Dupa");
            ratusz.AddNewParagraph("Aktualny przyrost surowców:", "Złoto", "200", "Drewno", "500", "Żywność", "1000");
            ratusz.AddNewParagraph("Aktualne poziomy budynków:", "Farma", "999", "Leśniczówka", "600");

            canvas.Children.Add(ratusz);

            // AK: Dodawanie budynków w celach testowych
            List<Building> listOfBuildings = new List<Building>();
            listOfBuildings.Add(new Building("WOODCUTTER", "/IdleClicker;component/Resources/Buildings/Woodshed.png", 0, -400, -400, 20, BuildingType.Productive));
            listOfBuildings.Add(new Building("FARM", "/IdleClicker;component/Resources/Buildings/Farm.png", 0, -100, 500, 999, BuildingType.Productive));
            listOfBuildings.Add(new Building("TOWNHALL", "/IdleClicker;component/Resources/Buildings/TownHall.png", 0, 0, 0, 999, BuildingType.TownHall));
            listOfBuildings.Add(new Building("WARHOUSE", "/IdleClicker;component/Resources/Buildings/Warehouse.png", 0, 400, -400, 999, BuildingType.Warehouse));
            listOfBuildings.Add(new Building("MINE", "/IdleClicker;component/Resources/Buildings/Mine.png", 0,900, -400, 999, BuildingType.Productive));

            //BuildingAction bonusAction = new BuildingAction(1, 0, 1);
            //bonusAction.Actions += () => { m.AddBonusQuantity(10,0); };
            //listOfBuildings[0].AddBonus(bonusAction);

            listOfBuildings[0].AddBonusCount(10, 5, 5, m, 20, "Ilość produkowanego drewna");
            listOfBuildings[0].AddBuildingInfo("NIC", "Gówno warta właściwość", "0");

            Requirement req = new Requirement(100, m);
            req.requireAlgorithm = (level) => { return 100 * level; };

            listOfBuildings[1].Requirements.Add(req);
            listOfBuildings[1].AddRequirement(5, listOfBuildings[0]);


            Requirement r = new Requirement(1, listOfBuildings[2]);
            r.SetAlgorithm((int level) => { if (level % 5 == 0) return level / 5; else return 0; });
            listOfBuildings[3].AddRequirement(r);
            // ----------------------------------------

            buildPanel.ImportBuildings(listOfBuildings);
            buildingsLayer.UpdateBuildingsOnLayer(listOfBuildings);

        }
    }
}
