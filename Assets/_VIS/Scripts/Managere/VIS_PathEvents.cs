using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualInfinityStudios.GamePlay;
using VirtualInfinityStudios.UI;
using SWS;

namespace VirtualInfinityStudios.EvenimenteMeniu
{
    public class VIS_PathEvents : MonoBehaviour
    {
        public Animator anim;
        public List<Animation> listaAnimatii = new List<Animation>();
        public splineMove splineScript;


        public void StartJoc()
        {
            anim.SetTrigger("Fuge");
            splineScript.ChangeSpeed(10f);
        }

        public void Stai()
        {
            anim.SetTrigger("Sta");
            splineScript.ChangeSpeed(0f);
        }

        public void Urca()
        {
            anim.SetTrigger("Urca");
            splineScript.ChangeSpeed(2f);
        }

        public void Sare()
        {
            anim.SetTrigger("Sare");
            splineScript.ChangeSpeed(2f);
        }
    }
}
