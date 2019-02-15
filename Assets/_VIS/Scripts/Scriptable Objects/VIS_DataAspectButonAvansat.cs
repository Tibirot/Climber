using System;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;
/***********************************
 * CopyRight 2019
 * Programmer: Buraca Dorin
 * Programmer: Socea Tiberiu
 * Website: http://www.VirtualInfinityStudios.ro
 * Application: Climber
 *  ***********************************/
namespace VirtualInfinityStudios.ElementeUI
{
    [CreateAssetMenu(menuName = "Virtual Infinity Studios/UI/Aspect Buton", fileName = "AB_NOU")]

    [Serializable]
    public class VIS_DataAspectButonAvansat : ScriptableObject
    {
        [Header("TEXT & TITLU")]
        public TMP_FontAsset fontTitlu;
        public Color culoareTitlu;
        public float marimeText;
        public bool bold = false;
        public bool contur = false;
        public string numeFont = "Fonts & Materials/Anton SDF";
        public string numeFontMaterial = "Fonts & Materials/Anton SDF - Outline";

        [Space(5)]
        [Header("FUNDAL BUTON")]
        public Sprite fundalButon;
        public Color culoareButon;

        [Space(5)]
        [Header("ICONITA")]
        public Sprite iconitaButon;
        public Color culoareIconita;
        public float marimeICO = 1.0f;
        public float pozitieOrizontala;
        public float poziteVerticala;

    }
}
