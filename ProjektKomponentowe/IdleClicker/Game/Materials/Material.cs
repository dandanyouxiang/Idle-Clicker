﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace IdleClicker
{
    /// <summary>
    /// Klasa opisująca surowiec.
    /// </summary>
    class Material
    {
        /// <summary>
        /// Ikona jako graficzna reprezentacja surowca.
        /// </summary>
        Image icon;
        /// <summary>
        /// Silnik gry.
        /// </summary>
        GameEngine gameEngine;
        /// <summary>
        /// Obecna ilość surowca.
        /// </summary>
        double currentAmount = 0;
        /// <summary>
        /// Obecny przyrost zasobu na tick zegara.
        /// </summary>
        double currentIncreaseQuantity = 0;
        /// <summary>
        /// Początkowy przyrost zasobu na tick zegara.
        /// </summary>
        double beginningIncreaseQuantity = 0;
         
        /// <summary>
        /// Konstruktor surowca.
        /// </summary>
        /// <param name="ge">Instancja klasy GameEngine, która zarządza czasem gry.</param>
        public Material(GameEngine ge)
        {
            gameEngine = ge;
        }

        /// <summary>
        /// Właściwość, która umożliwia pobranie icony danego surowca; 
        /// </summary>
        public Image Icon
        {
            get
            {
                return icon;
            }
        }

        /// <summary>
        /// Właściwość, która umożliwia ustawienie, bądź pobranie obecnej ilości danego surowca.
        /// </summary>
        public double CurrentAmount
        {
            get
            {
                return currentAmount;
            }
            set
            {
                currentAmount = value;
            }
        }

        /// <summary>
        /// Właściwość, która umożliwia ustawienie, bądź pobranie początkowego przyrostu danego surowca na tick zegara.
        /// </summary>
        public double BeginningIncreaseQuantity
        {
            get
            {
                return beginningIncreaseQuantity;
            }
            set
            {
                beginningIncreaseQuantity = value;
            }
        }

        /// <summary>
        /// Właściwość, która umożliwia ustawienie, bądź pobranie obecnego przyrostu danego surowca na tick zegara.
        /// </summary>
        public double CurrentIncreaseQuantity
        {
            get
            {
                return currentIncreaseQuantity;
            }
            set
            {
                currentIncreaseQuantity = value;
            }
        }

        /// <summary>
        /// Metoda, która umożliwia zwiększenie przyrostu zasobu na tick zegara, wyrażonego w procentach.
        /// </summary>
        /// <param name="percentage">Parametr, określający bonus wyrażony w procentach.</param>
        /// <param name="time">Paramter, określający czas trwania aktywności bonusa.</param>
        public void AddBonusPercentage(double percentage, int time)
        {
            currentIncreaseQuantity += percentage/100 * beginningIncreaseQuantity;

            Action action = new Action(time * 60);
            action.Actions += () => { currentIncreaseQuantity -= percentage * beginningIncreaseQuantity; };

            gameEngine.ActionList.AddAction(action);

        }

        /// <summary>
        /// Metoda, która umożliwia zwiększanie przyrostu zasobu na tick zegara, wyrażonego liczbą.
        /// </summary>
        /// <param name="quantity">Paramter, określający bonus wyrażone w liczbie jednostek danego zasobu.</param>
        /// <param name="time">Paramter, określający czas trwania aktywności bonusa.</param>
        public void AddBonusQuantity(double quantity, int time)
        {
            currentIncreaseQuantity += quantity;

            Action action = new Action(time * 60);
            action.Actions += () => { currentIncreaseQuantity -= quantity;  };

            gameEngine.ActionList.AddAction(action);

        }

        /// <summary>
        /// Metoda, odpowiadająca za zwiększenie ilości surowca o ustawiony przyrost zasobu na tick zegara.
        /// </summary>
        public void BoostMaterial()
        {
            currentAmount += currentIncreaseQuantity;
        }

        /// <summary>
        /// Metoda, odpowiadająca za zredukowanie ilości surowca o podaną liczbę jednostek.
        /// </summary>
        /// <param name="value">Paramter, określający liczbę jednostek surowca, o którą ogólna ilość ma zostać zredukowana.</param>
        public void ReduceMaterial(double value)
        {
            currentAmount -= value;
        }

        
    }
}