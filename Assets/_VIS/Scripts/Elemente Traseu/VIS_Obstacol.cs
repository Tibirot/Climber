using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualInfinityStudios.GamePlay
{
    [Serializable]
    public class VIS_Obstacol : MonoBehaviour
    {
        public enum LocatieObstacol { Stanga, Dreapta };
        public LocatieObstacol locatieObstacol;

        public Material materialPrestabilit;
        public bool razaActivaDebug = false;
        public float lungimeRaza = 0.5f;
        public LayerMask layerActiv = -1;
        public Transform pivotLansareRaze;

        public Color culoareRaza = Color.green;
        public Color culoareRazaHit = Color.red;

        public Vector3 pozitieObstacol;



        public float dist;


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

