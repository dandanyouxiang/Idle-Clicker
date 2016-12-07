﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleClicker
{
    public delegate void NewMaterialHandler(Material m);
    /// <summary>
    /// Klasa przechowująca listę surowców dostępnych w grze
    /// </summary>
    /// 
    class ListOfMaterials
    {
        private List<Material> Materials;

        public event NewMaterialHandler NewMaterial;

        /// <summary>
        /// Konstruktor klasy ListOfMaterials.
        /// </summary>
        public ListOfMaterials()
        {
            Materials = new List<Material>();
        }

        /// <summary>
        /// Metoda dodająca nowy surowiec do listy.
        /// </summary>
        /// <typeparam name="T">Typ ogólny surowca</typeparam>
        /// <param name="ge">GameEngine - instancja silnika gry podawana dla surowca</param>
        /// <returns>Zwraca instancję obiektu.</returns>
        public T AddNewMaterial<T>(GameEngine ge) where T : Material
        {
            foreach (Material item in Materials)
            {
                if (item is T) return (T)item;
            }
            
            Materials.Add((Material)Activator.CreateInstance(typeof(T), ge));
            NewMaterial((T)Materials[Materials.Count - 1]);
            return (T)Materials[Materials.Count - 1];
        }

    }
}
