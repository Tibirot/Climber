using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualInfinityStudios.GamePlay
{
    [Serializable]
    public class VIS_Obstacol : MonoBehaviour
    {
        public enum TipObstacol { Static, InainteInapoi, SusJos, Rotativ, Miraj, SuperDificila };
        public enum LocatieObstacol { Stanga, Dreapta };



        public TipObstacol tipObstacol = TipObstacol.Static;
        public LocatieObstacol locatieObstacol = LocatieObstacol.Stanga;

        public bool obtsacolSmart;
        public float vitezaDeplasare = 1.0f;
        public float distantaDeplasare = 1; //CATE UNITATI IN SPATIU SA SE DEPLASEZE CAPCANA, IN CAZUL NOSTRU 1
        private float punctDeplasareCurenta;

        public Material materialPrestabilit;
        public bool razaActivaDebug = false;
        public float lungimeRaza = 0.5f;
        public LayerMask layerActiv = -1;
        public Transform pivotLansareRaze;

        public Color culoareRaza = Color.green;
        public Color culoareRazaHit = Color.red;

        [HideInInspector]
        public Vector3 pozitieObstacol;     //FOLOSIM ACEST V3 SI PENTRU MISCARE SI PNTR. GENERARE DIN SEGMENT
        public float vitezaDeplasareY = 5f;

        private float dist; //pentru raycast calcul dist intre elemente


        private void OnEnable()
        {
            pozitieObstacol = transform.localPosition;
        }

        private void Update()
        {
            if (obtsacolSmart)
            {
                if (locatieObstacol == LocatieObstacol.Stanga)
                {
                    ObtacolSmartStangaVegheaza();
                }

                if (locatieObstacol == LocatieObstacol.Dreapta)
                {
                    ObtacolSmartDreaptaVegheaza();
                }
            }
            else
            {
                return;
            }
        }


        public void ObtacolSmartStangaVegheaza()
        {
            switch (tipObstacol)
            {
                case TipObstacol.InainteInapoi:
                    punctDeplasareCurenta = Mathf.PingPong(Time.time * vitezaDeplasare, distantaDeplasare) - distantaDeplasare;
                    transform.position = new Vector3(transform.position.x, transform.position.y, punctDeplasareCurenta);
                    break;

                case TipObstacol.SusJos:
                    //punctDeplasareCurenta = Mathf.PingPong(Time.time * vitezaDeplasare, transform.localPosition.y) - distantaDeplasare;
                    //transform.localPosition = new Vector3(transform.localPosition.x, punctDeplasareCurenta, transform.localPosition.z);

                    //CALCUL PENTRU NOUA LOCATIE PE INALTIME (Y)
                    float noulY = (Mathf.Sin(Time.time * vitezaDeplasareY) + pozitieObstacol.y) * distantaDeplasare;
                    //SETAM NOUA INALTIME PE OBSTACOL (Y)
                    transform.localPosition = new Vector3(pozitieObstacol.x, noulY, pozitieObstacol.z);

                    break;
            }
        }


        public void ObtacolSmartDreaptaVegheaza()
        {
            switch (tipObstacol)
            {
                case TipObstacol.InainteInapoi:
                    punctDeplasareCurenta = Mathf.PingPong(Time.time * vitezaDeplasare, 1.0f) - distantaDeplasare;
                    transform.position = new Vector3(-punctDeplasareCurenta, transform.position.y, transform.position.z);
                    break;

                case TipObstacol.SusJos:
                    //CALCUL PENTRU NOUA LOCATIE PE INALTIME (Y)
                    float noulY = (Mathf.Sin(Time.time * vitezaDeplasareY) + pozitieObstacol.y) * distantaDeplasare;
                    //SETAM NOUA INALTIME PE OBSTACOL (Y)
                    transform.localPosition = new Vector3(pozitieObstacol.x, noulY, pozitieObstacol.z);
                    //    pozitieObsDificil = new Vector3(transform.localPosition.x, 4.0f * Mathf.Sin(yPozitie), transform.localPosition.z);
                    //    transform.localPosition = pozitieObsDificil;

                    //    yPozitie += marireYPosition * Time.deltaTime;
                    //    if (yPozitie > Mathf.PI)
                    //    {
                    //        yPozitie = yPozitie - Mathf.PI;
                    //    }
                    //    break;

                    //case TipObstacol.SuperDificila:

                    //    // Move sphere around the circle.
                    //    pozitieObsDificil = new Vector3(2.0f * Mathf.Sin(xzPozitie), 4.0f * Mathf.Sin(yPozitie), 2.0f * Mathf.Cos(xzPozitie));
                    //    transform.position = pozitieObsDificil;

                    //    // Update the rotating position.
                    //    xzPozitie += marireXZPozitie * Time.deltaTime;
                    //    if (xzPozitie > 2.0f * Mathf.PI)
                    //    {
                    //        xzPozitie = xzPozitie - 2.0f * Mathf.PI;
                    //    }

                    //    // Update the up/down position.
                    //    yPozitie += marireYPosition * Time.deltaTime;
                    //    if (yPozitie > Mathf.PI)
                    //    {
                    //        yPozitie = yPozitie - Mathf.PI;
                    //    }
                    break;
            }
        }

        public void SchimbaInaltimea()
        {
            CalculeRaycasts();
        }


        private void CalculeRaycasts()
        {
            Vector3 directieRaza = transform.TransformDirection(new Vector3(0, 1, 0));
            Vector3 pPos = new Vector3(transform.localPosition.x, pivotLansareRaze.position.y, transform.localPosition.z);


            Debug.DrawRay(pPos, Quaternion.AngleAxis(5, transform.up) * directieRaza * lungimeRaza, culoareRaza);
            Debug.DrawRay(pPos, Quaternion.AngleAxis(-5, transform.up) * directieRaza * lungimeRaza, culoareRaza);

            //Raycast-uri VERTICAL**************************************************************************************
            if (Physics.Raycast(pPos, Quaternion.AngleAxis(90, transform.up) * directieRaza, out RaycastHit hit, lungimeRaza, layerActiv) && !hit.collider.isTrigger && hit.transform.root != transform)
            {
                if (hit.transform.gameObject.GetComponent<VIS_Obstacol>())
                {
                    dist = hit.distance;

                    switch (locatieObstacol)
                    {
                        case LocatieObstacol.Stanga:
                            if (dist < 2)
                            {
                                hit.transform.position = new Vector3(0, Mathf.RoundToInt(UnityEngine.Random.Range(3f, 10f)), -1.0f);
                            }
                            break;

                        case LocatieObstacol.Dreapta:
                            if (dist < 2)
                            {
                                hit.transform.position = new Vector3(1, Mathf.RoundToInt(UnityEngine.Random.Range(3f, 10f)), 0.0f);
                            }
                            break;
                    }
                    Debug.DrawRay(pPos, Quaternion.AngleAxis(90, transform.up) * directieRaza * lungimeRaza, culoareRazaHit);
                }
            }
        }

        public void ObtineMaterialPrestabilit()
        {
            materialPrestabilit = GetComponent<MeshRenderer>().sharedMaterial;
        }

        public void ReseteazaMaterial()
        {
            GetComponent<MeshRenderer>().sharedMaterial = materialPrestabilit;
        }

        public void SchimbaMaterial(Material mat)
        {
            GetComponent<MeshRenderer>().sharedMaterial = mat;
        }


        private void OnDrawGizmos()
        {
            if (razaActivaDebug)
            {
                CalculeRaycasts();
            }
        }
    }
}

