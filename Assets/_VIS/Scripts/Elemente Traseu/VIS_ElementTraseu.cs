using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
    public class VIS_ElementTraseu : MonoBehaviour
    {
        [Header("PARAMETRI GIZMOS & SETARI")]
        public float probabilitateGenerare;
        public float gradDificultateSegment;
        public int idSegment;
        public Transform punctInceput;
        public Transform punctSfarsit;
        public Color culoareImbinari;
        public float dimensiuneImbinare = 0.5f;
        public Material materialObiectSelectat;
        [Range(2.0f, 10)]
        public float offsetMargini = 5;
        public float lungimeSegment;

        [Header("OBSTACOLE")]
        public bool rayCastActiv;
        public LayerMask layerObstacole;
        public Transform obstacoleStanga;
        public Transform obstacoleDreapta;
        public List<VIS_Obstacol> prefabricateObstacol = new List<VIS_Obstacol>();
        public List<VIS_Obstacol> prefabricateObstacoleeee = new List<VIS_Obstacol>();

        public List<Capcana> listaCapcaneStanga = new List<Capcana>();
        public List<Capcana> listaCapcaneDreapta = new List<Capcana>();

        public float distantareObstacoleStanga = 0.0f;
        public float distantareObstacoleDreapta = 0.0f;
        [Range(2.0f, 20.0f)]
        public float offsetVerticalObstacole = 2.5f;


        [Space(10)]
        [Header("COLECTABILE")]
        public Transform holderColectabileStanga;
        public Transform holderColectabileDreapta;
        [Range(0.6f, 2)]
        public float offsetColectabilOrizontal = 1.0f;
        [Range(0.1f, 1)]
        public float offsetColectabilVertical = 0.1f;
        private float distantareColectabileStanga = 0.0f;
        private float distantareColectabileDreapta = 0.0f;
        public List<VIS_Colectabil> prefabsColectabile = new List<VIS_Colectabil>();
        public List<Colectabil> listaColectabileStanga = new List<Colectabil>();
        public List<Colectabil> listaColectabileDreapta = new List<Colectabil>();

        Vector3 pozIncGenerareColStanga = new Vector3(0.0f, 0.0f, -1);
        Vector3 pozSfGenerareColStanga = new Vector3(0.0f, 20.0f, -1);
        Vector3 pozIncGenerareColDreapta = new Vector3(1.0f, 0.0f, 0.0f);
        Vector3 pozSfGenerareColDreapta = new Vector3(1.0f, 20.0f, 0.0f);

        public Transform element;



        private void Awake()
        {
            if (element == null)
            {
                element = transform.Find("GRAFIC").GetComponent<Transform>();
            }
        }


        public void InitiereEditor()
        {
            //setam parametri prestabiliti
            listaCapcaneStanga.Clear();
            listaCapcaneDreapta.Clear();
            offsetMargini = 2.0f;
            distantareObstacoleStanga = 0.0f;
            distantareObstacoleDreapta = 0.0f;
        }

        public void ActiveazaElement()
        {
            gameObject.SetActive(true);
        }

        public void DezactiveazaElement()
        {
            Debug.Log("Vrea sa ma dezactivez!");
            gameObject.SetActive(false);
        }

        public void MaDistrug(int viata)
        {
            Debug.Log(transform.name + viata);
        }

        public void AdaugaObstacolNou(string locatie)
        {
            //CREEM OBSTACOL PE SEGMENT 
            //SETAM PARINTELE & HOLDER
            //ADAUGAM OBSTACOLUL IN LISTA DE OBSTACOLE
            VIS_Obstacol _clona = Instantiate(prefabricateObstacol[0], transform.position, Quaternion.identity);

            switch (locatie)
            {
                case "Stanga":
                    _clona.locatieObstacol = VIS_Obstacol.LocatieObstacol.Stanga;
                    distantareObstacoleStanga += offsetVerticalObstacole;

                    if (distantareObstacoleStanga >= (punctSfarsit.transform.position.y - offsetMargini))
                    {
                        //listaObstacole.Remove(_clona);
                        //listaObstacoleStanga.Remove(_clona);
                        //DestroyImmediate(_clona);
                        //Debug.Log("UPS" + (punctSfarsit.transform.position.y - offsetImbinari).ToString());
                        return;
                    }
                    else
                    {
                        _clona.transform.position = new Vector3(0.0f, distantareObstacoleStanga, -1.0f);
                        _clona.pozitieObstacol = _clona.transform.position;
                        _clona.transform.SetParent(obstacoleStanga);
                        ADAUGARE_OBSTACOL(locatie, _clona);
                        //_clona.gameObject.name = "OBST_STANGA_" + listaCapcaneStanga.IndexOf(_clona);
                    }
                    break;


                case "Dreapta":
                    //_clona.gameObject.name = "OBST_DREAPTA_" + listaObstacole.IndexOf(_clona);
                    _clona.locatieObstacol = VIS_Obstacol.LocatieObstacol.Dreapta;
                    //listaObstacoleDreapta.Add(_clona);

                    distantareObstacoleDreapta += offsetVerticalObstacole;

                    _clona.transform.position = new Vector3(1.0f, distantareObstacoleDreapta, 0.0f);
                    _clona.pozitieObstacol = _clona.transform.position;
                    _clona.ObtineMaterialPrestabilit();
                    _clona.transform.SetParent(obstacoleDreapta);
                    ADAUGARE_OBSTACOL(locatie, _clona);
                    break;
            }


            _clona.pivotLansareRaze = _clona.transform;
            _clona.layerActiv = layerObstacole;
            _clona.gameObject.layer = 12;
        }

        public void AranjareObstacole()
        {
            //foreach (VIS_Obstacol o in listaObstacole)
            //{
            //    o.SchimbaInaltimea();

            //}
        }

        public void ActiveazaRayCast()
        {
            //for (int i = 0; i < listaObstacole.Count; i++)
            //{
            //    listaObstacole[i].razaActivaDebug = rayCastActiv;
            //}
        }

        public void SelecteazaObiect(int indexObstacolSelectat)
        {
            for (int i = 0; i < listaCapcaneDreapta.Count; i++)
            {
                if (listaCapcaneDreapta.IndexOf(listaCapcaneDreapta[i]) != indexObstacolSelectat)
                {
                    listaCapcaneDreapta[i].refObstacol.GetComponent<VIS_Obstacol>().ReseteazaMaterial();
                }
                else
                {
                    listaCapcaneDreapta[i].refObstacol.GetComponent<VIS_Obstacol>().SchimbaMaterial(materialObiectSelectat);
                }
            }
        }

        public void RefacereMateriale()
        {
            for (int i = 0; i < listaCapcaneDreapta.Count; i++)
            {
                listaCapcaneDreapta[i].refObstacol.GetComponent<VIS_Obstacol>().ReseteazaMaterial();
            }
        }

        public void ADAUGARE_OBSTACOL(string directie, VIS_Obstacol obstacol)
        {
            if (directie == "Stanga")
            {
                listaCapcaneStanga.Add(new Capcana
                {
                    refObstacol = obstacol,
                    tipObstacol = Capcana.TipObstacol.Stanga,
                    pozitieObstacol = obstacol.pozitieObstacol,
                    indexLayer = obstacol.layerActiv.value,
                    layerObs = obstacol.layerActiv.value,
                });
                //SOLUTIE REDENUMIRE: IMPOSIBILITATE REDENUMIRE INITIERE PREFAB
                int indexUltimulObstacolAdaugat = listaCapcaneStanga.Count - 1;
                listaCapcaneStanga[indexUltimulObstacolAdaugat].refObstacol.name = "OBST_STANGA_" + indexUltimulObstacolAdaugat;

            }
            else if (directie == "Dreapta")
            {

                listaCapcaneDreapta.Add(new Capcana
                {
                    refObstacol = obstacol,
                    tipObstacol = Capcana.TipObstacol.Dreapta,
                    pozitieObstacol = obstacol.pozitieObstacol,
                    indexLayer = obstacol.layerActiv.value,
                    layerObs = obstacol.layerActiv.value
                });

                int indexUltimulObstacolAdaugat = listaCapcaneDreapta.Count - 1;
                listaCapcaneDreapta[indexUltimulObstacolAdaugat].refObstacol.name = "OBST_DREAPTA_" + indexUltimulObstacolAdaugat;
            }
        }

        public void ELIMINA_OBSTACOL(string directie, int indexObstacol)
        {
            if (directie == "Stanga")
            {
                if (listaCapcaneStanga.Count > 0)
                    listaCapcaneStanga.RemoveAt(indexObstacol);

            }
            else if (directie == "Dreapta")
            {
                if (listaCapcaneDreapta.Count > 0)
                    listaCapcaneDreapta.RemoveAt(indexObstacol);
            }
        }



        public void ADAUGARE_COLECTABILE(string _caz)
        {
            //ADAUG ACEASTA LOGICA DEOARECE:
            //NUMARUL DE COLECTABILE PENTRU GENERARE 
            //ESTE STRICT LEGAT DE OFFSET COLECTABIL VERTICAL
            //EXEMPLU: segment = 20 & offsetColectabilVertical = 0.5 => BANUTI PANA LA JUMATATEA SEGMENTULUI

            float _nrColectabileDeGenerat = 0;
            if (offsetColectabilVertical <= 0.5f)
            {
                _nrColectabileDeGenerat = lungimeSegment * 2;
            }
            else
            {
                _nrColectabileDeGenerat = lungimeSegment;
            }

            if (_caz == "Stanga")
            {
                for (int i = 0; i < _nrColectabileDeGenerat; i++)
                {
                    distantareColectabileStanga += offsetColectabilVertical;
                    Vector3 pozGenerareColectabileStg = new Vector3(pozIncGenerareColStanga.x, pozIncGenerareColStanga.y + distantareColectabileStanga, -offsetColectabilOrizontal);

                    VIS_Colectabil _clonaCol = Instantiate(prefabsColectabile[0], pozGenerareColectabileStg, Quaternion.identity);
                    _clonaCol.transform.SetParent(holderColectabileStanga);
                    string _numeCol = "COL_STNG_" + i;
                    _clonaCol.name = _numeCol;

                    listaColectabileStanga.Add(new Colectabil
                    {
                        idColectabil = _numeCol,
                        refColectabil = _clonaCol,
                        tipColectabil = "Banut",
                        pozitieColectabil = _clonaCol.transform.position
                    });
                }
            }
            else if (_caz == "Dreapta")
            {
                Quaternion _rotDreapta = new Quaternion(0.0f, -90.0f, 0.0f, 0.0f);

                for (int i = 0; i < _nrColectabileDeGenerat; i++)
                {
                    distantareColectabileDreapta += offsetColectabilVertical;
                    Vector3 pozGenerareColectabileDrt = new Vector3(offsetColectabilOrizontal, pozIncGenerareColDreapta.y + distantareColectabileDreapta, pozIncGenerareColDreapta.z);

                    VIS_Colectabil _clonaCol = Instantiate(prefabsColectabile[0], pozGenerareColectabileDrt, _rotDreapta);
                    _clonaCol.transform.SetParent(holderColectabileDreapta);
                    string _numeCol = "COL_DREAPTA_" + i;
                    _clonaCol.name = _numeCol;

                    listaColectabileDreapta.Add(new Colectabil
                    {
                        idColectabil = _numeCol,
                        refColectabil = _clonaCol,
                        tipColectabil = "Banut",
                        pozitieColectabil = _clonaCol.transform.position
                    });
                }
            }
        }


        public void ELIMINA_COLECTABILE(string _caz)
        {
            if (_caz == "Stanga")
            {
                for (int i = 0; i < listaColectabileStanga.Count; i++)
                {
                    DestroyImmediate(listaColectabileStanga[i].refColectabil.gameObject);
                }

                listaColectabileStanga.Clear();
                offsetColectabilVertical = 1.0f;
                distantareColectabileStanga = 0.8f;

            }
            else if (_caz == "Dreapta")
            {
                for (int i = 0; i < listaColectabileDreapta.Count; i++)
                {
                    DestroyImmediate(listaColectabileDreapta[i].refColectabil.gameObject);
                }

                listaColectabileDreapta.Clear();
                offsetColectabilVertical = 1.0f;
                distantareColectabileDreapta = 0.8f;
            }
            else
            {
                return;
            }

        }

        public void ACTUALIZEAZA_POZITII_COLECTABILE(string _axa)
        {
            switch (_axa)
            {
                case "Vertical":


                    for (int i = 0; i < listaColectabileStanga.Count; i++)
                    {
                        listaColectabileStanga[i].refColectabil.transform.SetPositionAndRotation(new Vector3(listaColectabileStanga[i].refColectabil.transform.position.x, /*listaColectabileStanga[i].refColectabil.transform.position.y - */offsetColectabilVertical, listaColectabileStanga[i].refColectabil.transform.position.z), Quaternion.identity);
                        listaColectabileStanga[i].pozitieColectabil = listaColectabileStanga[i].refColectabil.transform.position;
                    }

                    break;

                case "Orizontal":


                    for (int i = 0; i < listaColectabileStanga.Count; i++)
                    {
                        listaColectabileStanga[i].refColectabil.transform.SetPositionAndRotation(new Vector3(listaColectabileStanga[i].refColectabil.transform.position.x, listaColectabileStanga[i].refColectabil.transform.position.y, -offsetColectabilOrizontal), Quaternion.identity);
                        listaColectabileStanga[i].pozitieColectabil = listaColectabileStanga[i].refColectabil.transform.position;
                    }

                    for (int i = 0; i < listaColectabileDreapta.Count; i++)
                    {
                        listaColectabileDreapta[i].refColectabil.transform.SetPositionAndRotation(new Vector3(offsetColectabilOrizontal, listaColectabileDreapta[i].refColectabil.transform.position.y, listaColectabileDreapta[i].refColectabil.transform.position.z), Quaternion.identity);
                        listaColectabileDreapta[i].pozitieColectabil = listaColectabileDreapta[i].refColectabil.transform.position;
                    }

                    break;
            }
        }

        private void OnDrawGizmos()
        {
            //SOLUTIE PENTRU OPTIMIZARE ;)
            if (Selection.activeTransform == transform)
            {
                Color culoareInitiala = Gizmos.color;

                Gizmos.color = culoareImbinari;
                if (punctInceput != null && punctSfarsit != null)
                {
                    //AFISARE LINIE LUNGIME SEGEMENT + CAPETELE ACESTUIA
                    Gizmos.color = culoareImbinari;
                    Gizmos.DrawLine(punctInceput.position, punctSfarsit.position);
                    Gizmos.DrawCube(punctInceput.position, new Vector3(0.15f, 0.15f, 0.15f));
                    Gizmos.DrawCube(punctSfarsit.position, new Vector3(0.15f, 0.15f, 0.15f));

                    var pozJumatate = punctSfarsit.position.y / 2;
                    lungimeSegment = punctSfarsit.position.y;
                    Handles.Label(new Vector3(0, 10.0f, 0), pozJumatate.ToString());
                    Gizmos.color = culoareInitiala;

                    //AFISARE LINII & PUNCTE GENERARE COLECTABILE
                    Gizmos.color = Color.magenta;
                    pozIncGenerareColStanga = new Vector3(punctInceput.transform.position.x, punctInceput.transform.position.y, punctInceput.transform.position.z - offsetColectabilOrizontal);
                    pozSfGenerareColStanga = new Vector3(punctSfarsit.transform.position.x, punctSfarsit.transform.position.y, punctSfarsit.transform.position.z - offsetColectabilOrizontal);
                    pozIncGenerareColDreapta = new Vector3(punctInceput.transform.position.x + offsetColectabilOrizontal, punctInceput.transform.position.y, punctInceput.transform.position.z);
                    pozSfGenerareColDreapta = new Vector3(punctSfarsit.transform.position.x + offsetColectabilOrizontal, punctSfarsit.transform.position.y, punctSfarsit.transform.position.z);

                    Gizmos.DrawLine(pozIncGenerareColStanga, pozSfGenerareColStanga);
                    Gizmos.DrawLine(pozIncGenerareColDreapta, pozSfGenerareColDreapta);
                    Gizmos.color = culoareInitiala;

                    //LIMITE GENERARE OBSTACOLE
                    Gizmos.color = Color.yellow;

                    Vector3 pozLimitareJos = new Vector3(0.0f, punctInceput.transform.position.y + offsetMargini, 0.0f);
                    Vector3 pozLimitareSus = new Vector3(0.0f, punctSfarsit.transform.position.y - offsetMargini, 0.0f);
                    Gizmos.DrawWireCube(pozLimitareJos, new Vector3(3.0f, 0.1f, 3.0f));
                    Gizmos.DrawWireCube(pozLimitareSus, new Vector3(3.0f, 0.1f, 3.0f));
                    Gizmos.color = culoareInitiala;


                    Handles.color = Color.black;
                    Handles.Label(pozLimitareJos, pozLimitareJos.y.ToString());
                    Handles.Label(pozLimitareSus, pozLimitareSus.y.ToString());
                }


                Gizmos.color = culoareInitiala;
            }
        }


        [Serializable]
        public class Capcana
        {
            public enum TipObstacol { Stanga, Dreapta };

            public VIS_Obstacol refObstacol;
            public LayerMask layerObs;
            public TipObstacol tipObstacol;
            public Vector3 pozitieObstacol;
            public int indexLayer;


            public void SeteazaLayerCorect()
            {
                layerObs = indexLayer;
            }

            public void ActualizeazaDateObstacol()
            {
                pozitieObstacol = refObstacol.pozitieObstacol;
            }
        }

        [Serializable]
        public class Colectabil
        {
            public string idColectabil;
            public VIS_Colectabil refColectabil;
            public string tipColectabil;
            public Vector3 pozitieColectabil;
        }

    }


}

