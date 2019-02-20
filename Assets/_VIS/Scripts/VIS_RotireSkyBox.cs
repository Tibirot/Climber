using UnityEngine;
using System.Collections;

namespace VirtualInfinityStudios.GamePlay
{
    public class VIS_RotireSkyBox : MonoBehaviour
    {

        public Material materialCer;
        public float viteza = 1f;

        void Update()
        {
            materialCer.SetFloat("_Rotation", Time.time * viteza);
            //if (Modules.reducedEffect < 2)
            //{
            //    myMatSky.SetFloat("_Rotation", Time.time * speed);
            //}
        }
    }
}