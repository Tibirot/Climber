using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VirtualInfinityStudios;
using VirtualInfinityStudios.GamePlay;
/***********************************
 * CopyRight 2019
 * Programmer: Buraca Dorin
 * Programmer: Socea Tiberiu
 * Website: http://www.VirtualInfinityStudios.ro
 * Game: Climber
 *  ***********************************/
namespace VirtualInfinityStudios.UI
{
    public class VIS_ManagerUI : MonoBehaviour
    {
        public TextMeshProUGUI textDistanta;
        public TextMeshProUGUI textTimp;
        public VIS_Jucator jucator;
        public float timpTrecut = 0.0f;

        public GameObject panouGameOver;

        private string stringTimp = string.Empty;


        private void Awake()
        {
            panouGameOver.SetActive(false);
        }

        void Update()
        {
            textDistanta.text = jucator.distantaParcursa.ToString() + "m";

            timpTrecut += Time.deltaTime;
            int secunde = (int)(timpTrecut % 60);
            int minute = (int)(timpTrecut / 60) % 60;


            stringTimp = string.Format("{0:00}:{1:00}", minute, secunde);
            textTimp.text = stringTimp;

        }

        public void ActiveazaPanouGameOver()
        {
            panouGameOver.SetActive(true);
        }

        public void DaUnRestart()
        {
            panouGameOver.SetActive(false);
            VIS_ManagerGP.Instance.RestartTemp();
        }
    }
}
