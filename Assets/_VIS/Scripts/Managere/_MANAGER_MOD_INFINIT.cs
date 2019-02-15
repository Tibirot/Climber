using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Collections;
using UnityEngine.UI;
///***********************************
// * CopyRight 2016
// * www.devapp-solutions.com
// * Programmer: Buraca Dorin
// * Game: Escape From Transylvania 2D
// ***********************************/
public class _MANAGER_MOD_INFINIT : MonoBehaviour
{
    //{

    //    [Header("SETARI GENERARE")]
    //    public int distantaIntreSegmente;
    //    public string numeSpawner;
    //    public float intervalDespawn;

    //    [Header("SEGMENTE GENERATE")]
    //    public List<Transform> segmente = new List<Transform>();

    //    [Space(5)]
    //    [Header("SETARI AFISARE ECRAN")]
    //    //public StareJoc _STAREJOC;
    //    public GameObject _JUCATOR;
    //    //public GUI_JUCATOR _GUI_JUCATOR;
    //    public Transform semnCeaMaiBunaDistanta;

    //    [Header("TEXTE AFISARE ECRAN")]
    //    public Text textDistanta;
    //    public Text textPunctaj;
    //    public Text textVitezaJucator;
    //    public Text t_CeaMaiBunaDistanta;
    //    public Text t_CelMaiBunPunctaj;
    //    public Text t_valoareCeaMaiBunaDistanta;
    //    public Text t_valoareCelMaiBunPunctaj;
    //    //PARAMETRI
    //    public float metru;
    //    public float vitezaJoc = 10;
    //    public int nivelDif = 1;
    //    private int puncte = 2;
    //    private int curPunctaj = 0;
    //    //CEA MAI BUNA DISTANTA
    //    private bool setat = false;
    //    private float pozX;
    //    private float pozY;

    //    void Awake()
    //    {
    //        //SETEZ & POZITIONEZ SEMNUL PENTRU CEA MAI BUNA DISTANTA PARCURSA
    //        pozX = PlayerPrefs.GetFloat("InfPozitieX");
    //        //pozY = PlayerPrefs.GetFloat("InfPozitieY"); //-->> DE GASIT SOLUTIE BLOCARE/PASTRARE COORDONATA "Y"
    //        semnCeaMaiBunaDistanta.position = new Vector2(pozX, 3.5f);
    //        Debug.Log("Semnul <Cea Mai Buna Distanta> a fost pozitionat la: " + semnCeaMaiBunaDistanta.position);

    //        //AFISEZ PE GUI CEA MAI BUNA DISTANTA & CEL MAI BUN PUNCTAJ OBTINUT DE JUCATORUL CURENT
    //        t_valoareCeaMaiBunaDistanta.text = PlayerPrefs.GetFloat("InfCMBD").ToString() + " m";
    //        t_valoareCelMaiBunPunctaj.text = PlayerPrefs.GetInt("InfCMBP").ToString() + " p";
    //    }

    //    void Start()
    //    {
    //        //ACCESEZ MANAGERUL JOCULUI SI SCHIMB MODUL DE JOC (DIN STORY IN ENDLESS)
    //        //ASTFEL IMI PERMITE SA FOLOSESC ACELAS GUI, FARA SA CREEZ UNUL SEPARAT ( !ENORM DE MULTA MUNCA :P )
    //        //SI SA TRANSFORM TEXTUL DE USTUROI IN DISTANTA METRI
    //        //SufletJoc.copie.modEndless = true;
    //        //SufletJoc.copie._modInfinit = this;
    //        PopuleazaListaSegmente();
    //    }

    //    void Update()
    //    {
    //        //VERIFIC STAREA JOCULUI DE 60 FPS/S => !IMPORTANT: PENTRU ACTUALIZARERA DATELOR SI SCHIMBAREA AUTOMATA A STARII DE JOC
    //        //PREVIN CALCULAREA PREMATURA SAU DUPA MOARTEA JUCATORULUI A DISTANTEI IMPLICIT PUNCTAJULUI
    //        //if (_STAREJOC == StareJoc.Joaca)
    //        //{
    //        //    ActualizareDistanta();
    //        //}

    //        ////SOLUTIE TEMP
    //        //if (_JUCATOR.transform.position.y <= -4)
    //        //{
    //        //    _STAREJOC = StareJoc.Mort;
    //        //}

    //        //DE MODIFICAT, GASIT SOLUTIE EVENT/DELEGATE => SOLUTIE TEPM NEPERFORMANTA

    //        //if (_JUCATOR.GetComponent<SufletJucator>().inViata == false && setat == false && _JUCATOR.transform.position.x > pozX)
    //        //{
    //        //    SeteazaCeaMaiBunaDistanta();
    //        //    _STAREJOC = StareJoc.Mort;
    //        //    t_valoareCeaMaiBunaDistanta.text = string.Format("{0:N0} m", metru);
    //        //    t_valoareCelMaiBunPunctaj.text = curPunctaj.ToString() + "p";
    //        //}
    //    }


    //    #region AFISARE GUI 
    //    //IN CAZUL IN CARE JUCATORUL MOARE, SALVEZ POZITIA UNDE A MURIT SI DISTANTA, INCLUSIV PUNCTAJUL :)
    //    public void SeteazaCeaMaiBunaDistanta()
    //    {
    //        setat = true;
    //        PlayerPrefs.SetFloat("InfPozitieX", _JUCATOR.transform.position.x);
    //        //PlayerPrefs.SetFloat("InfPozitieY", _JUCATOR.transform.position.y);  !DE GASIT SOUTIE ELIMINARE POZITIE Y
    //        PlayerPrefs.SetFloat("InfCMBD", (int)metru);
    //        PlayerPrefs.SetInt("InfCMBP", curPunctaj);
    //        Debug.Log("Pozitia pentrul semnul CMBD a fost salvata!");
    //    }

    //    public void ActualizareDistanta()
    //    {
    //        //AFISARE TEMPORARA DOAR PENTRU NOI LA TESTE ==> APOI DE ELIMINAT
    //        textVitezaJucator.text = "VITEZA ALERGARE: " + _JUCATOR.GetComponent<Rigidbody2D>().velocity.x.ToString() + "m/s - stare joc: " + _STAREJOC.ToString() + "\n POZITIE PE COORDONATA X: " + _JUCATOR.transform.position.x.ToString();
    //        //CALCULEZ DISTANTA PARCURSA DE JUCATOR INMULTIND CU 10, APOI FORMATEZ SI AFISEZ TEXTUL PE GUI
    //        metru += Time.deltaTime * vitezaJoc;
    //        //_GUI_JUCATOR.textUsturoi.text = string.Format("{0:N0} m", metru);
    //        //CALCULEZ PUNCTAJUL INMULTIND 1M CU COTRAVALOAREA ACESTUIA , APOI FORMATEZ SI AFISEZ TEXTUL PE GUI
    //        curPunctaj = (int)metru * puncte;
    //       /// _GUI_JUCATOR.textPunctaj.text = curPunctaj.ToString() + " p";

    //        #region NiveleDificultate
    //        //AFISAM CHECKPOINT IN FUNCTIE DE DISTANTA PARCURSA
    //        if (metru >= 250 && nivelDif == 1)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("250 m");
    //        }
    //        if (metru >= 500 && nivelDif == 2)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("500 m");
    //        }
    //        if (metru >= 1000 && nivelDif == 3)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("1000 m");
    //        }
    //        if (metru >= 1500 && nivelDif == 4)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("1500 m");
    //        }
    //        if (metru >= 3000 && nivelDif == 5)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("3000 m");
    //        }
    //        if (metru >= 4000 && nivelDif == 6)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("4000 m");
    //        }
    //        if (metru >= 5000 && nivelDif == 7)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("5000 m");
    //        }
    //        if (metru >= 6000 && nivelDif == 8)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("6000 m");
    //        }
    //        if (metru >= 7000 && nivelDif == 9)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("7000 m");
    //        }
    //        if (metru >= 8000 && nivelDif == 10)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("8000 m");
    //        }
    //        if (metru >= 9000 && nivelDif == 11)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("9000 m");
    //        }
    //        if (metru >= 10000 && nivelDif == 12)
    //        {
    //            MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("10.000 m");
    //        }
    //        else if (metru >= 10500 && nivelDif == 13)
    //        {
    //            //MaresteViteza();
    //            _GUI_JUCATOR.AfiseazaMesaj("10.500 m");
    //        }

    //        #endregion
    //    }

    //    //SISTEM DIFICULTATE IN FUNCTIE DE DISTANTA & !DE GANDIT SI IN FUNCTIE DE PUNCTAJ
    //    public void MaresteViteza()
    //    {
    //        nivelDif += 1;
    //        vitezaJoc += 2;
    //        puncte += 1;
    //        //_JUCATOR.GetComponent<GunmanController>().runSpeed += 0.5f;
    //        Debug.Log("*********************DIFICULTATE MARITA*********************");
    //    }

    //    //SCHIMBAREA STARII DE JOC IN FUNCTIE DE SITUATIE:
    //    public void SchimbaStareaJocului(string stare)
    //    {
    //        switch (stare)
    //        {
    //            case "InJoc":
    //                _STAREJOC = StareJoc.Joaca;
    //                Debug.Log("STARE JOC: LIVE");
    //                break;

    //            case "InPauza":
    //                _STAREJOC = StareJoc.Pauza;
    //                Debug.Log("STARE JOC: PAUZA");
    //                break;

    //            case "InMoarte":
    //                _STAREJOC = StareJoc.Mort;
    //                Debug.Log("STARE JOC: MORT");
    //                break;
    //        }
    //    }


    //    //DEBUG TEST
    //    public void ReseteazaPunctajDistanta(int puncte)
    //    {
    //        PlayerPrefs.SetFloat("InfPozitieX", 0);
    //        PlayerPrefs.SetFloat("InfPozitieY", 0);
    //        PlayerPrefs.SetFloat("InfCMBD", puncte);
    //        PlayerPrefs.SetInt("InfCMBP", puncte);
    //    }
    //    #endregion

    //    #region SISTEM GENERARE

    //    public void PopuleazaListaSegmente()
    //    {
    //        //SpawnPool _Clone = PoolManager.Pools[this.numeSpawner];
    //        //segmente = new List<Transform>(_Clone);
    //        //Debug.Log(_Clone.Count);
    //    }

    //    public void VerificaSiGenereazaSegmente(Vector2 _punctGenerare)
    //    {
    //        int _idSegment = Random.Range(0, segmente.Count);
    //        Transform _clona = segmente[_idSegment];
    //        Transform inst;
    //        //SpawnPool Clone = PoolManager.Pools[this.numeSpawner];
    //        //inst = Clone.Spawn(_clona);
    //        //inst.transform.position = _punctGenerare;

    //        //StartCoroutine(Despawn());
    //        Debug.Log("!!!GENERAM O NOUA PLATFORMA!!!");
    //    }

    //    IEnumerator Despawn()
    //    {
    //        yield return new WaitForSeconds(intervalDespawn);
    //        //SpawnPool CloneGenerate = PoolManager.Pools[this.numeSpawner];
    //        //var copieListaObiecteSpanate = new List<Transform>(CloneGenerate);

    //        //foreach (Transform instance in copieListaObiecteSpanate)
    //        //{
    //        //    //CloneGenerate.Despawn(instance);
    //        //    yield return new WaitForSeconds(this.intervalDespawn);
    //        //}

    //        // Restart
    //        //this.StartCoroutine(Spawner());
    //        yield return null;
    //    }
    //    #endregion





    //    /*DE FACUT:
    //* 1.LOGICA FUNDAL DIFERIT
    //* 2.PLATFORMELE SA-SI SCHIMBE SPRITE-URILE IN FUNCTIE DE FUNDAL
    //* 3.LOGICA PENTRU DISTRUGEREA PREFABRICATELOR TRECUTE -->> SOLUTIE TEMP (NEPERFORMANTA) !REFACERE: FAC SISTEM DE RESPAWN PENTRU REFOLOSIRE
    //* 4.PREFABRICATELE SA LE FAC SECTIUNI MARI CE VOR CONTINE PLATFORME,INAMICI SI CAPCANE
    //* 5.BONUS PENTRU DEPASIREA PROPRIULUI PUNCTAJ/DISTANTA
    //* 6.ECRAN FINAL ENDLESS
    //* 7.LOGICA INVIERE PE ENDLESS ->> ATENTIE DIFICIL
    //*/
    //}
}