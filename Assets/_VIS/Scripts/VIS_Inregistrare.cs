using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Combu;
/***********************************
 * CopyRight 2019
 * Programmer: Buraca Dorin
 * Programmer: Socea Tiberiu
 * Website: http://www.VirtualInfinityStudios.ro
 * Application: Services Manager
 *  ***********************************/
namespace VirtualInfinityStudios
{
    [Serializable]
    public class VIS_Inregistrare : MonoBehaviour
    {
        [Header("REFERINTE UI")]
        public GameObject panouLogin;
        public GameObject panouInregistrare;

        [Header("INCARCARE")]
        public GameObject indicatorIncarcareSingin;
        public GameObject indicatorIncarcareLogin;
        public Button butonAcces;
        public Button butonInregistrare;
        public Button butonLoginFb;

        public TextMeshProUGUI textEroare;


        public TMP_InputField inputLogare;
        public TMP_InputField inputParola;

        public List<Canvas> panouri = new List<Canvas>();

        [Header("INREGISTRARE UTILIZATOR")]
        public bool godMode = false;
        public TMP_InputField inregEmail;
        public TMP_InputField inregNume;
        public TMP_InputField inregPrenume;
        public TMP_InputField inregParola;
        public TMP_InputField inregTelefon;
        public TMP_InputField inregCodPostal;
        public bool inregAcordTermCond;
        public TextMeshProUGUI inregEroare;


        public void AtasareAutomata()
        {
            panouLogin = GameObject.Find("PANOU_LOGIN");

            //inputLogare = GameObject.Find("USERNAME").GetComponent<TMP_InputField>();
            //inputParola = GameObject.Find("PASSWORD").GetComponent<TMP_InputField>();

            //butonAcces = GameObject.Find("BUTON_ACCES").GetComponent<Button>();
            butonInregistrare = GameObject.Find("BUTON_INREGISTRARE").GetComponent<Button>();
            butonLoginFb = GameObject.Find("BUTON_FB").GetComponent<Button>();
        }
    }
}


