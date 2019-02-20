using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualInfinityStudios.GamePlay
{
    public class VIS_Colectabil : MonoBehaviour
    {
        public TipColectabil tipColectabil = TipColectabil.banut;
        public int idElementColectabil; //rename*

        public MeshRenderer modelGrafic;
        public ParticleSystem efxActivare;
        public ParticleSystem efxDezactivare;
        public float vitezaRotire = 50f;




        private void FixedUpdate()
        {
            modelGrafic.transform.Rotate(0, vitezaRotire * Time.fixedDeltaTime, 0);
        }




        private void OnTriggerEnter(Collider obiect)
        {
            if (obiect.gameObject.CompareTag("VIS/Jucator"))
            {
                DezactiveazaColectabil();
            }
        }


        public void ActiveazaColectabil()
        {
            efxActivare.Play();
            gameObject.SetActive(true);
        }


        public void DezactiveazaColectabil()
        {
            StartCoroutine(IncepeDezactivareaObiectului());
        }

        IEnumerator IncepeDezactivareaObiectului()
        {
            efxDezactivare.Play();
            vitezaRotire = vitezaRotire * 2;
            modelGrafic.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
            yield return new WaitForSeconds(0.15f);
            gameObject.SetActive(false);
        }

    }

    public enum TipColectabil
    {
        banut,
        cheie,
        litera,
        magnet,
        jetpack,
        scut,
        surpriza,
        boosterPunctaj,
        headStart,
        mysteryBox,
        misiune,
        provocare,
        bonus
    }



    //int maxValue = 15; // or whatever you want the max value to be
    //int minValue = -15; // or whatever you want the min value to be
    //int currentValue = 0; // or wherever you want to start
    //int direction = 1;

    //Update()
    //{
    //    currentValue += Time.deltaTime * direction; // or however you are incrementing the position
    //    if (currentValue >= maxValue)
    //    {
    //        direction *= -1;
    //        currentvalue = maxValue;
    //    }
    //    else if (currentValue <= minValue)
    //    {
    //        direction *= -1;
    //        currentvalue = minValue;
    //    }
    //    transform.position = new Vector3(currentValue, 0, 0);
    //}
}