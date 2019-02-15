using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.SceneManagement;
using VirtualInfinityStudios.UI;
/***********************************
 * CopyRight 2019
 * Programmer: Buraca Dorin
 * Programmer: Socea Tiberiu
 * Website: http://www.VirtualInfinityStudios.ro
 * Game: Climber
 *  ***********************************/
namespace VirtualInfinityStudios
{
    public class VIS_ManagerGP : MonoBehaviour
    {

        public static VIS_ManagerGP Instance;

        public ProCamera2DShake proCam;
        public VIS_Jucator jucator;
        public VIS_ManagerUI managerUi;


        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                //DontDestroyOnLoad(gameObject);
            }

            //DontDestroyOnLoad(gameObject);
            Instance = this;
        }


        public void Zguduie(string tipShake)
        {
            proCam.Shake(tipShake);

        }

        public void JucatorMort()
        {
            managerUi.ActiveazaPanouGameOver();
            proCam.GetComponent<ProCamera2D>().enabled = false;
        }


        public void RestartTemp()
        {
            SceneManager.LoadScene(0);
        }

    }
}
