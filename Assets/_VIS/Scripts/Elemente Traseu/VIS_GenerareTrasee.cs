using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/***********************************
 * CopyRight 2019
 * Programmer: Buraca Dorin
 * Programmer: Socea Tiberiu
 * Website: http://www.VirtualInfinityStudios.ro
 * Game: Climber
 *  ***********************************/
namespace VirtualInfinityStudios.GamePlay
{
    public class VIS_GenerareTrasee : MonoBehaviour
    {
        public bool seJoaca = false;
        public bool traseuInitialGenerat = false;
        public float intervalGenerare = 0.05f;

        public Transform holderElemente;
        public float offsetVertical = 10.0f;
        public List<VIS_ElementTraseu> listaPrafabElemente = new List<VIS_ElementTraseu>();
        public int numarMaximElemente = 10;


        public List<VIS_ElementTraseu> listaElementeGenerate = new List<VIS_ElementTraseu>();
        public List<Vector3> pozitiiInitElemente = new List<Vector3>();



        [ContextMenu("START")]
        public void StartJoc()
        {
            seJoaca = true;
            StartCoroutine(PornesteJoc());
        }

        IEnumerator PornesteJoc()
        {
            while (seJoaca)
            {
                yield return new WaitForSeconds(intervalGenerare);
                if (!traseuInitialGenerat)
                {
                    GenereazaTraseu();
                }
                //else
                //{
                //    RegenereazaTraseu();
                //}
            }
        }


        public void GenereazaTraseu()
        {

            float _pozVert = 10.0f;
            int _indxAleatoriu = 0;

            // GENEREARE TRASEU INITIAL
            for (int i = 0; i < numarMaximElemente; i++)
            {

                _indxAleatoriu = Random.Range(0, listaPrafabElemente.Count - 1);
                VIS_ElementTraseu _clona = Instantiate(listaPrafabElemente[_indxAleatoriu], holderElemente.transform.position, Quaternion.identity);
                _clona.transform.SetParent(holderElemente);
                _clona.gameObject.name = "ElementTraseu_" + i;

                listaElementeGenerate.Add(_clona);
                _clona.idSegment = listaElementeGenerate.IndexOf(_clona);

                if (_clona.gameObject.name == "ElementTraseu_0")
                {
                    _clona.transform.position = new Vector3(0.0f, offsetVertical, 0.0f);

                }
                else
                {
                    _pozVert += 20.0f;
                    _clona.transform.position = new Vector3(0.0f, _pozVert, 0.0f);
                }

                pozitiiInitElemente.Add(_clona.transform.position);
                //_clona.startJoc = true;
            }


            if (listaElementeGenerate.Count == numarMaximElemente)
            {
                traseuInitialGenerat = true;
            }

            pozitiiInitElemente.Reverse();
        }

        public void RepozitionareElemente()
        {
            for (int i = 0; i < listaElementeGenerate.Count; i++)
            {
                listaElementeGenerate[i].transform.position = pozitiiInitElemente[i];
            }
        }

        public void RepozitionareElemente(int idElemTest)
        {
            //for (int i = 0; i < listaElementeGenerate.Count; i++)
            //{
            //    if (listaElementeGenerate[i].idElement == idElemTest)
            //    {
            Debug.Log(idElemTest);
            listaElementeGenerate[idElemTest].transform.position = new Vector3(0f, 180f, 0f);
            Debug.Log(pozitiiInitElemente[idElemTest]);
            //}
            //}
        }

        public void DezactiveazaElementeTraseu(int id)
        {
            for (int i = 0; i < listaElementeGenerate.Count; i++)
            {
                if (listaElementeGenerate[i].idSegment == id)
                {
                    listaElementeGenerate[i].DezactiveazaElement();
                }
            }
        }




    }
}