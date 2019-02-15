using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/***********************************
* CopyRight 2019
* Programmer: Buraca Dorin
* Programmer: Socea Tiberiu
* Website: http://www.VirtualInfinityStudios.ro
* Game: Climber
*  ***********************************/
namespace VirtualInfinityStudios.GamePlay
{
    public class VIS_ManagerSectiuni : MonoBehaviour
    {

        [Header("SECTIUNE INCEPUT")]
        public bool sectiuneStartActiva = true;
        public int numarSectiuniStart = 3;

        [Header("PARAMETRI & SETARI")]
        public float lungimeSectiune = 100;
        public int sectiuniPeEcran = 5;
        public int indexUltimuluiPrefab = 0;
        public float zonaSigura = 200f;

        [Header("REFERINTE OBIECTE")]
        [Tooltip("Prefabricatul cu index <0> il folosim ca si sectiune de START.")]
        public GameObject[] prefabSectiuni;
        public Transform pozitieJucator;
        public float spawnY = 0.0f;

        public List<GameObject> listaSectiuniActive;

        [Header("REFERINTE SECTIUNI PREFABRICATE")]
        public List<PrefabricatSegment> listaSegmentePrefabricat = new List<PrefabricatSegment>();
        string _idPrefab = String.Empty;

        void Start()
        {
            //pozitieJucator = GameObject.FindGameObjectWithTag("Player").transform;
            listaSectiuniActive = new List<GameObject>();

            for (int i = 0; i < sectiuniPeEcran; i++)
            {
                if (i < numarSectiuniStart && sectiuneStartActiva)
                    GenereazaSectiune(0);
                else
                    GenereazaSectiune();
            }
        }


        void Update()
        {
            if (pozitieJucator.position.y - zonaSigura > (spawnY - sectiuniPeEcran * lungimeSectiune))
            {
                GenereazaSectiune();
                DezactiveazaSectiune();
            }
        }

        private void GenereazaSectiune(int indexPrefab = -1)
        {
            GameObject _sectiune;

            if (indexPrefab == -1)
                _sectiune = Instantiate(prefabSectiuni[IndexAleatoriuPrefab()]) as GameObject;
            else
                _sectiune = Instantiate(prefabSectiuni[indexPrefab]) as GameObject;
            _sectiune.transform.SetParent(transform);
            _sectiune.transform.position = Vector3.up * spawnY;
            spawnY += lungimeSectiune;
            listaSectiuniActive.Add(_sectiune);
        }

        private void DezactiveazaSectiune()
        {
            Destroy(listaSectiuniActive[0]);
            listaSectiuniActive.RemoveAt(0);
        }

        private int IndexAleatoriuPrefab()
        {
            if (prefabSectiuni.Length <= 1)
                return 0;

            int indexAleatoriu = indexUltimuluiPrefab;
            while (indexAleatoriu == indexUltimuluiPrefab)
            {
                indexAleatoriu = UnityEngine.Random.Range(0, prefabSectiuni.Length);
            }

            indexUltimuluiPrefab = indexAleatoriu;
            return indexAleatoriu;
        }



        #region Logica Alegere Segmente Aleatoriu
        [ContextMenu("TEST PROBABILITATE")]
        public void CalculProbabilitateAlegere()
        {
            var _greutati = SumaGreutati();
            var _candidat = AlegeSegment(UnityEngine.Random.Range(0, _greutati));

            Debug.Log(_candidat.idSegment + " : " + _candidat.probabilitate);
        }

        private float SumaGreutati()
        {
            float suma = 0;
            for (int i = 0; i < listaSegmentePrefabricat.Count; i++)
            {
                var candidat = listaSegmentePrefabricat[i];
                suma += candidat.probabilitate;
            }
            return suma;
        }

        private PrefabricatSegment AlegeSegment(float num)
        {
            for (int i = 0; i < listaSegmentePrefabricat.Count; i++)
            {

                var candidat = listaSegmentePrefabricat[i];
                var weight = candidat.probabilitate;
                _idPrefab = candidat.idSegment;

                if (num <= weight)
                {
                    return candidat;
                }
                else
                {
                    num -= weight;
                }
            }
            return listaSegmentePrefabricat[listaSegmentePrefabricat.Count - 1];
        }


        [Serializable]
        public class PrefabricatSegment
        {
            public string idSegment;
            public GameObject refSegment;
            public float probabilitate = 1;
        }

        #endregion


    }

}