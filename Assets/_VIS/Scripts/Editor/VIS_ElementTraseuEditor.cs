using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
/***********************************
 * CopyRight 2019
 * Programmer: Buraca Dorin
 * Programmer: Socea Tiberiu
 * Website: http://www.VirtualInfinityStudios.ro
 * Game: Climber
 *  ***********************************/
namespace VirtualInfinityStudios.GamePlay
{
    [CustomEditor(typeof(VIS_ElementTraseu))]
    class VIS_ElementTraseuEditor : Editor
    {
        protected VIS_ElementTraseu segment;

        // GRAFICE:
        //-MENIU
        Color culoarePrestabilit;
        Texture2D logoVIS;
        Texture2D tab_SETARI;
        Texture2D tab_OBSTACOLE;
        Texture2D tab_COLECTABILE;

        //-OBSTACOLE
        Texture2D but_AdaugaStanga;
        Texture2D but_AdaugaDreapta;
        Texture2D but_EliminareObs;
        Texture2D but_AranjareAutoObs;
        Texture2D banda_UC;

        //-COLECTABILE
        Texture2D but_AdaugaColStanga;
        Texture2D but_AdaugaColDreapta;
        Texture2D but_AdaugaColAmbeleDir;
        Texture2D but_EliminaColStanga;
        Texture2D but_EliminaColDreapta;
        Texture2D but_EliminaColAmbeleDir;
        Texture2D but_AjustDistCol_Off;
        Texture2D but_AjustDistCol_On;
        Texture2D but_AplicaModificariMan;

        //-LISTE REORDER
        Texture2D ico_ochi;
        Texture2D ico_ochiOff;



        //BUTOANE & TABS
        public int indexTabCurent = 0;
        public string[] taburiCategorii = new string[] { "SETARI", "OBSTACOLE", "COLECTABILE" };

        bool cat_SETARI = false;
        bool cat_OBSTACOLE = false;
        bool cat_COLECTABILE = false;


        //REFERINTE SERIALIZED PROPERTY OBSTACOLE
        SerializedProperty sansaTest;
        SerializedProperty sp_layerObs;
        SerializedProperty sp_holderStanga;
        SerializedProperty sp_holderDreapta;
        SerializedProperty sp_listaPrefabsObstacole;
        SerializedProperty sp_limitaObs;
        SerializedProperty sp_offsetVerticalObs;

        //REFERINTE SERIALIZED PROPERTY COLECTABILE
        SerializedProperty sp_offsetColectabileO;
        SerializedProperty sp_offsetColectabileV;
        SerializedProperty sp_prefabsColectabile;

        //LISTE REORDERABLE:
        //-OBSTACOLE
        private ReorderableList listaObstacoleStanga;
        private ReorderableList listaObstacoleDreapta;
        private ReorderableList listaPrefababricateObstacole;
        //-COLECTABILE
        private ReorderableList listaColectabileStanga;
        private ReorderableList listaColectabileDreapta;
        private ReorderableList listaPrefababricateColectabile;

        //INDEX-URI SELECTII ELEMET LISTE REORDERABLE
        //-OBSTACOLE
        int indexObstacolStangaFocusat;
        int indexPrefabObstacoleFocusat;
        int indexObstacolDreaptaFocusat;
        //-COLECTABILE
        int indexColectabilStangaFocusat;
        int indexColectabilDreaptaFocusat;
        int indexPrefabColectabileFocusat;

        //BOOL-URI VIZUALIZARE ELEMENTE LISTE REORDERABLE
        //-OBSTACOLE
        bool vizualizareObstacoleStanga;
        bool vizualizareObstacoleDreapta;
        bool vizualizarePrefabricateObstacole;
        //-COLECTABILE
        bool vizualizareColectabileStanga;
        bool vizualizareColectabileDreapta;
        bool vizualizarePrefabricateColectabile;



        //ECRAN INFORMATII
        //-----------OBSTACOLE:
        int numarObtsacoleTotal;
        int numarObtsacoleStanga;
        int numarObtsacoleDreapta;
        //-----------COLECTABILE:
        int numarColectabileTotal;
        int numarColectabileStanga;
        int numarColectabileDreapta;

        Texture2D but_ImgEditareColectabile;
        bool editareColectabile = false;


        bool afiseazaColectabileStanga = true;
        string status = "AFISARE LISTA";
        string statusEdiColectabile = "";




        private void Awake()
        {
            culoarePrestabilit = GUI.color;
            logoVIS = Resources.Load("VIS_GUI/ED_Titlu_ElementTraseu", typeof(Texture2D)) as Texture2D;

            tab_SETARI = Resources.Load("VIS_GUI/ED_Buton_Setari", typeof(Texture2D)) as Texture2D;
            tab_OBSTACOLE = Resources.Load("VIS_GUI/ED_Buton_Obstacole", typeof(Texture2D)) as Texture2D;
            tab_COLECTABILE = Resources.Load("VIS_GUI/ED_Buton_Colectabile", typeof(Texture2D)) as Texture2D;

            banda_UC = Resources.Load("VIS_GUI/banda_UC", typeof(Texture2D)) as Texture2D;

            but_AdaugaStanga = Resources.Load("VIS_GUI/ED_Buton_Adauga_ObstacoleST", typeof(Texture2D)) as Texture2D;
            but_AdaugaDreapta = Resources.Load("VIS_GUI/ED_Buton_Adauga_ObstacoleDR", typeof(Texture2D)) as Texture2D;
            but_EliminareObs = Resources.Load("VIS_GUI/ED_Buton_Eliminare_Obstacole", typeof(Texture2D)) as Texture2D;
            but_AranjareAutoObs = Resources.Load("VIS_GUI/ED_Buton_Aranjare_Obstacole", typeof(Texture2D)) as Texture2D;

            ico_ochi = Resources.Load("VIS_GUI/ICO_ELIMINA", typeof(Texture2D)) as Texture2D;
            ico_ochiOff = Resources.Load("VIS_GUI/ICO_Ochi", typeof(Texture2D)) as Texture2D;

            //-COLECTABILE
            but_ImgEditareColectabile = Resources.Load("VIS_GUI/ED_Buton_Ajustare_ColectabileOFF", typeof(Texture2D)) as Texture2D;
            but_AdaugaColStanga = Resources.Load("VIS_GUI/ED_Buton_Adauga_ColectabileST", typeof(Texture2D)) as Texture2D;
            but_AdaugaColDreapta = Resources.Load("VIS_GUI/ED_Buton_Adauga_ColectabileDR", typeof(Texture2D)) as Texture2D;
            but_AdaugaColAmbeleDir = Resources.Load("VIS_GUI/ED_Buton_Adauga_ColectabileSTDR", typeof(Texture2D)) as Texture2D;
            but_EliminaColStanga = Resources.Load("VIS_GUI/ED_Buton_Eliminare_ColectabileST", typeof(Texture2D)) as Texture2D;
            but_EliminaColDreapta = Resources.Load("VIS_GUI/ED_Buton_Eliminare_ColectabileDR", typeof(Texture2D)) as Texture2D;
            but_EliminaColAmbeleDir = Resources.Load("VIS_GUI/ED_Buton_Eliminare_Colectabile", typeof(Texture2D)) as Texture2D;
            but_AjustDistCol_Off = Resources.Load("VIS_GUI/ED_Buton_Ajustare_ColectabileOFF", typeof(Texture2D)) as Texture2D;
            but_AjustDistCol_On = Resources.Load("VIS_GUI/ED_Buton_Ajustare_ColectabileON", typeof(Texture2D)) as Texture2D;
            but_AplicaModificariMan = Resources.Load("VIS_GUI/ED_Buton_Aplica_Modificari", typeof(Texture2D)) as Texture2D;
        }

        public void OnEnable()
        {
            segment = target as VIS_ElementTraseu;

            //LISTE REORDEABLE
            listaObstacoleStanga = new ReorderableList(serializedObject, serializedObject.FindProperty("listaCapcaneStanga"), true, false, false, false);
            listaObstacoleDreapta = new ReorderableList(serializedObject, serializedObject.FindProperty("listaCapcaneDreapta"), true, false, false, false);
            listaPrefababricateObstacole = new ReorderableList(serializedObject, serializedObject.FindProperty("listaCapcaneStanga"), true, false, true, true);

            listaColectabileStanga = new ReorderableList(serializedObject, serializedObject.FindProperty("listaColectabileStanga"), true, true, false, false);
            listaColectabileDreapta = new ReorderableList(serializedObject, serializedObject.FindProperty("listaColectabileDreapta"), true, false, false, false);
            listaPrefababricateColectabile = new ReorderableList(serializedObject, serializedObject.FindProperty("prefabsColectabile"), true, false, true, true);

            //CALLBACK-URI LISTE REORDERABLE
            //-OBSTACOLE STANGA:
            listaObstacoleStanga.drawElementCallback += ObstacolStanga_DrawCallback;
            listaObstacoleStanga.elementHeightCallback = element => 55;
            //-OBSACOLE DREAPTA:
            listaObstacoleDreapta.drawElementCallback += ObstacolDreapta_DrawCallback;
            listaObstacoleDreapta.elementHeightCallback = element => 55;
            //-PREFABRICATE OBSTACOLE
            listaPrefababricateObstacole.drawElementCallback += ObstacolDreapta_DrawCallback;
            listaPrefababricateObstacole.elementHeightCallback = element => 55;

            //-COLECTABILE STANGA:
            listaColectabileStanga.drawElementCallback += ColectabilStanga_DrawCallback;
            listaColectabileStanga.elementHeightCallback = element => 55;
            //-COLECTABILE DREAPTA:
            listaColectabileDreapta.drawElementCallback += ColectabilDreapta_DrawCallback;
            listaColectabileDreapta.elementHeightCallback = element => 55;
            //-PREFABRICATE OBSTACOLE
            listaPrefababricateColectabile.drawElementCallback += ObstacolDreapta_DrawCallback;
            listaPrefababricateColectabile.elementHeightCallback = element => 55;



            //SERIALIZED PROPERTY CAPCANE
            sansaTest = serializedObject.FindProperty("probabilitateGenerare");
            sp_layerObs = serializedObject.FindProperty("layerObstacole");
            sp_holderStanga = serializedObject.FindProperty("obstacoleStanga");
            sp_holderDreapta = serializedObject.FindProperty("obstacoleDreapta");
            sp_listaPrefabsObstacole = serializedObject.FindProperty("prefabricateObstacol");
            sp_limitaObs = serializedObject.FindProperty("offsetMargini");
            sp_offsetVerticalObs = serializedObject.FindProperty("offsetVerticalObstacole");

            //SERIALIZED PROPERTY COLECTABILE
            sp_offsetColectabileO = serializedObject.FindProperty("offsetColectabilOrizontal");
            sp_offsetColectabileV = serializedObject.FindProperty("offsetColectabilVertical");

            //sp_prefabsColectabile = serializedObject.FindProperty("prefabsColectabile");

            if (Selection.activeGameObject)
            {
                cat_SETARI = ActiveazaCategorie();
            }
        }


        //AICI INCEPEM RANDAREA TOOL-ULUI********************************************************************************/
        /****************************************************************************************************************/
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label(logoVIS, EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(45f));
            GUILayout.EndHorizontal();
            GUILayout.Space(5);


            #region SECTIUNE TABS TOP
            //***********************************************SECTIUNE TABS TOP**********************************************/
            /*************************************************************************************************************/

            GUILayout.BeginHorizontal();

            //BUTON SETARI------------------------------->
            if (cat_SETARI)
                GUI.backgroundColor = Color.gray;
            else GUI.backgroundColor = culoarePrestabilit;

            if (GUILayout.Button(tab_SETARI))
                cat_SETARI = ActiveazaCategorie();

            //BUTON OBSTACOLE------------------------------->
            if (cat_OBSTACOLE)
                GUI.backgroundColor = Color.gray;
            else GUI.backgroundColor = culoarePrestabilit;

            if (GUILayout.Button(tab_OBSTACOLE))
                cat_OBSTACOLE = ActiveazaCategorie();

            //BUTON COLECTABILE------------------------------->
            if (cat_COLECTABILE)
                GUI.backgroundColor = Color.gray;
            else GUI.backgroundColor = culoarePrestabilit;

            if (GUILayout.Button(tab_COLECTABILE))
                cat_COLECTABILE = ActiveazaCategorie();
            else GUI.backgroundColor = culoarePrestabilit;

            GUILayout.EndHorizontal();


            //***********************************************END SECTIUNE BUTOANE**********************************************/
            /*************************************************************************************************************/
            #endregion


            if (cat_SETARI)
            {
                numarObtsacoleStanga = segment.listaCapcaneStanga.Count;
                numarObtsacoleStanga = segment.listaCapcaneDreapta.Count;
                numarObtsacoleTotal = numarObtsacoleStanga + numarObtsacoleDreapta;

                GUILayout.BeginVertical(GUI.skin.box);
                GUI.color = Color.yellow;
                EditorGUILayout.HelpBox("INFORMATII SEGMENT", MessageType.None);
                GUI.color = culoarePrestabilit;

                GUI.color = Color.green;
                //OBSTACOLE
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("Obstacole total: " + numarObtsacoleTotal, MessageType.None);
                EditorGUILayout.HelpBox("Obstacole Stanga: " + numarObtsacoleStanga, MessageType.None);
                EditorGUILayout.HelpBox("Obstacole Dreapta: " + numarObtsacoleDreapta, MessageType.None);
                GUILayout.Space(10);
                GUILayout.EndHorizontal();

                GUILayout.Space(5);

                //COLECTABILE
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("Colectabile total: " + numarObtsacoleTotal, MessageType.None);
                EditorGUILayout.HelpBox("Colectabile Stanga: " + numarObtsacoleStanga, MessageType.None);
                EditorGUILayout.HelpBox("Colectabile Dreapta: " + numarObtsacoleDreapta, MessageType.None);
                GUILayout.Space(10);
                GUILayout.EndHorizontal();
                GUI.color = culoarePrestabilit;
                GUILayout.EndVertical();

                //EditorGUILayout.Slider(sansaTest, 2.0f, 100.0f, new GUIContent("SANSA: "));
                if (!sansaTest.hasMultipleDifferentValues)
                {
                    BaraProgres(sansaTest.floatValue, "Sansa obstacol");
                }

                //EditorGUILayout.PropertyField(sp_listaPrefabsObstacole, new GUIContent("PREFABRICATE OBSTACOL"));
            }

            if (cat_OBSTACOLE)
            {
                EditorGUILayout.BeginVertical(/*GUI.skin.box*/);
                GUI.color = Color.yellow;
                EditorGUILayout.HelpBox("-------------------SECTIUNE OBSTACOLE-------------------", MessageType.None);
                GUI.color = culoarePrestabilit;



                //SECTIUNE TOP OBSTACOLE****************************************
                EditorGUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.PropertyField(sp_offsetVerticalObs, new GUIContent("Offset Vertical"));
                EditorGUILayout.PropertyField(sp_limitaObs, new GUIContent("Limita Margini"));

                EditorGUILayout.PropertyField(sp_holderStanga, new GUIContent("Holder Stanga"));
                EditorGUILayout.PropertyField(sp_holderDreapta, new GUIContent("Holder Dreapta"));

                EditorGUILayout.PropertyField(sp_layerObs, new GUIContent("Layer Obstacol"));
                EditorGUILayout.PropertyField(sp_listaPrefabsObstacole, new GUIContent("PREFABRICATE OBSTACOL"), true);



                EditorGUILayout.BeginHorizontal(/*GUI.skin.box*/);
                if (GUILayout.Button(but_EliminareObs))
                {
                    for (int i = 0; i < segment.listaCapcaneStanga.Count; i++)
                    {
                        if (segment.listaCapcaneStanga[i].refObstacol.gameObject != null)
                            DestroyImmediate(segment.listaCapcaneStanga[i].refObstacol.gameObject);
                    }

                    for (int i = 0; i < segment.listaCapcaneDreapta.Count; i++)
                    {
                        if (segment.listaCapcaneDreapta[i].refObstacol.gameObject != null)
                            DestroyImmediate(segment.listaCapcaneDreapta[i].refObstacol.gameObject);
                    }

                    segment.listaCapcaneStanga.Clear();
                    segment.listaCapcaneDreapta.Clear();
                    segment.InitiereEditor();
                }

                if (GUILayout.Button(but_AranjareAutoObs))
                {
                    segment.AranjareObstacole();
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                /***************************************************************/

                GUILayout.Space(10);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(banda_UC, EditorStyles.inspectorDefaultMargins, GUILayout.MaxHeight(14f));
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.BeginVertical();
                GUI.color = Color.yellow;
                EditorGUILayout.HelpBox("OBSTACOLE STANGA", MessageType.None);
                GUI.color = culoarePrestabilit;

                GUI.color = Color.gray;

                listaObstacoleStanga.DoLayoutList();
                serializedObject.ApplyModifiedProperties();

                GUI.color = culoarePrestabilit;
                EditorGUILayout.EndVertical();


                GUILayout.Space(10);

                //SECTIUNE BUTOANE ADAUGA STANGA/DREAPTA*****************************************************

                EditorGUILayout.BeginHorizontal();
                GUI.color = Color.white;

                //ADAUGA ELEMENT STANGA*****************
                if (GUILayout.Button(but_AdaugaStanga))
                {
                    float limitaSus = segment.punctSfarsit.transform.position.y - segment.offsetMargini;

                    if (segment.distantareObstacoleStanga >= limitaSus - 1.0f)
                    {
                        int optiuniRaspuns = EditorUtility.DisplayDialogComplex("AI ATINS LIMITA DE OBSTACOLE!",
                       "Limita TOP: " + limitaSus + "Daca doresti sa mai adaugi Obtsacole, mareste limitele in Inspector: ",
                       "Mareste",
                       "Renunta",
                       "Nu stiu ce sa fac");

                        switch (optiuniRaspuns)
                        {
                            case 0:
                                segment.offsetMargini -= 2.0f;
                                //Debug.Log("MARESTE!");
                                break;

                            case 1:
                                //Debug.Log("RENUNTA!");
                                break;

                            case 2:
                                //Debug.Log("NU STIE :D");
                                break;
                        }
                    }
                    else
                    {
                        segment.AdaugaObstacolNou("Stanga");
                    }
                }

                //ADAUGA ELEMENT DREAPTA****************
                if (GUILayout.Button(but_AdaugaDreapta))
                {
                    float limitaSus = segment.punctSfarsit.transform.position.y - segment.offsetMargini;

                    if (segment.distantareObstacoleDreapta >= limitaSus - 1.0f)
                    {
                        int optiuniRaspuns = EditorUtility.DisplayDialogComplex("AI ATINS LIMITA DE OBSTACOLE!",
                       "Limita TOP: " + limitaSus + "Daca doresti sa mai adaugi Obtsacole, mareste limitele in Inspector: ",
                       "Mareste",
                       "Renunta",
                       "Nu stiu ce sa fac");

                        switch (optiuniRaspuns)
                        {
                            case 0:
                                segment.offsetMargini -= 2.0f;
                                //Debug.Log("MARESTE!");
                                break;

                            case 1:
                                //Debug.Log("RENUNTA!");
                                break;

                            case 2:
                                //Debug.Log("NU STIE :D");
                                break;
                        }
                    }
                    else
                    {
                        segment.AdaugaObstacolNou("Dreapta");
                    }

                    //segment.ELIMINA_OBSTACOL("Dreapta", indexObstacolDreaptaFocusat);
                }

                GUI.color = culoarePrestabilit;
                EditorGUILayout.EndHorizontal();

                /***************************************************************************/

                GUILayout.Space(10);
                EditorGUILayout.BeginVertical();
                GUI.color = Color.yellow;
                EditorGUILayout.HelpBox("OBSTACOLE DREAPTA", MessageType.None);
                GUI.color = culoarePrestabilit;

                GUI.color = Color.gray;
                listaObstacoleDreapta.DoLayoutList();
                serializedObject.ApplyModifiedProperties();

                GUI.color = culoarePrestabilit;
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();
            }

            if (cat_COLECTABILE)
            {
                EditorGUILayout.BeginVertical(/*GUI.skin.box*/);
                GUI.color = Color.yellow;
                EditorGUILayout.HelpBox("-------------------SECTIUNE COLECTABILE-------------------", MessageType.None);
                GUI.color = culoarePrestabilit;

                //SECTIUNE PROPRIETATI & LISTE COLECTABILE****************************************
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("prefabsColectabile"), new GUIContent("PREFABS"), true);
                EditorGUILayout.PropertyField(sp_offsetColectabileO, new GUIContent("Offset Colectabile Orizontal"));
                EditorGUILayout.PropertyField(sp_offsetColectabileV, new GUIContent("Offset Colectabile Vertical"));

                GUILayout.Space(15);

                EditorGUILayout.BeginHorizontal();
                GUI.color = Color.white;
                if (GUILayout.Button(but_AdaugaColStanga))
                {
                    segment.ADAUGARE_COLECTABILE("Stanga");
                }


                if (GUILayout.Button(but_AdaugaColDreapta))
                {
                    segment.ADAUGARE_COLECTABILE("Dreapta");
                }

                if (GUILayout.Button(but_AdaugaColAmbeleDir))
                {
                    segment.ADAUGARE_COLECTABILE("Stanga");
                    segment.ADAUGARE_COLECTABILE("Dreapta");
                }
                GUI.color = culoarePrestabilit;
                EditorGUILayout.EndHorizontal();


                GUILayout.Space(5);


                EditorGUILayout.BeginHorizontal();
                GUI.color = Color.gray;
                if (GUILayout.Button(but_EliminaColStanga))
                {
                    segment.ELIMINA_COLECTABILE("Stanga");
                }


                if (GUILayout.Button(but_EliminaColDreapta))
                {
                    segment.ELIMINA_COLECTABILE("Dreapta");
                }

                if (GUILayout.Button(but_EliminaColAmbeleDir))
                {
                    segment.ELIMINA_COLECTABILE("Stanga");
                    segment.ELIMINA_COLECTABILE("Dreapta");
                }
                GUI.color = culoarePrestabilit;
                EditorGUILayout.EndHorizontal();


                GUILayout.Space(10);
                EditorGUILayout.HelpBox("EDITARE AVANSATA", MessageType.None);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(but_ImgEditareColectabile))
                {
                    if (!editareColectabile)
                    {
                        but_ImgEditareColectabile = but_AjustDistCol_On;
                        editareColectabile = true;
                        statusEdiColectabile = "-ATENTIE- OPRESTE AJUSTAREA DISTANTEI DE MAI SUS!";
                    }
                    else
                    {
                        but_ImgEditareColectabile = but_AjustDistCol_Off;
                        editareColectabile = false;
                        statusEdiColectabile = "";
                    }
                }

                if (GUILayout.Button(but_AplicaModificariMan))
                {
                    //APLICA MODIFICArI MANUAL
                }
                EditorGUILayout.EndHorizontal();



                if (!sp_offsetColectabileO.hasMultipleDifferentValues && editareColectabile)
                {
                    ActualizeazaOffsetOrizontalColectabile();
                }

                //if (!sp_offsetColectabileV.hasMultipleDifferentValues && EditareColectabile)
                //{
                //    ActualizeazaOffsetVerticalColectabile();
                //}

                EditorGUILayout.EndVertical();

                if (editareColectabile)
                {
                    EditorGUILayout.HelpBox(statusEdiColectabile, MessageType.Warning);
                }


                GUILayout.Space(5);
                GUILayout.Label(banda_UC, EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(16f));

                EditorGUILayout.BeginVertical();
                GUI.color = Color.yellow;
                EditorGUILayout.HelpBox("COLECTABILE STANGA", MessageType.None);
                GUI.color = culoarePrestabilit;

                //FEEDBACK VISUAL LA AJUSTAREA DISTANTELOR COLECTABILOR
                if (editareColectabile)
                {
                    GUI.color = Color.green;
                }
                else { GUI.color = culoarePrestabilit; }

                afiseazaColectabileStanga = EditorGUILayout.Foldout(afiseazaColectabileStanga, status);
                if (afiseazaColectabileStanga)
                {
                    listaColectabileStanga.DoLayoutList();
                    serializedObject.ApplyModifiedProperties();
                }

                if (!afiseazaColectabileStanga)
                {
                    status = "AFISEAZA COLECTABILE STANGA";
                    afiseazaColectabileStanga = false;
                }

                GUI.color = Color.yellow;
                EditorGUILayout.HelpBox("COLECTABILE DREAPTA", MessageType.None);
                GUI.color = culoarePrestabilit;

                //FEEDBACK VISUAL LA AJUSTAREA DISTANTELOR COLECTABILOR
                if (editareColectabile)
                {
                    GUI.color = Color.green;
                }
                else { GUI.color = culoarePrestabilit; }


                listaColectabileDreapta.DoLayoutList();
                serializedObject.ApplyModifiedProperties();

                GUI.color = culoarePrestabilit;
                EditorGUILayout.EndVertical();



                EditorGUILayout.EndVertical();
            }

            /**************************************************************************************************************************************/
            /************************************************************PENTRU INSPECTOR DEBUG****************************************************/

            base.OnInspectorGUI();

            /**************************************************************************************************************************************/
            /**************************************************************************************************************************************/

            segment.ActiveazaRayCast();

            serializedObject.ApplyModifiedProperties();
            GUI.color = culoarePrestabilit;
        }



        //METODE*********************************************************************************************************/
        /****************************************************************************************************************/
        private void ActualizeazaOffsetOrizontalColectabile()
        {

            if (Selection.activeTransform)
            {
                segment.ACTUALIZEAZA_POZITII_COLECTABILE("Orizontal");
                //Debug.Log(sp_offsetColectabileV.floatValue);
            }
        }

        private void ActualizeazaOffsetVerticalColectabile()
        {

            if (Selection.activeTransform)
            {
                segment.ACTUALIZEAZA_POZITII_COLECTABILE("Vertical");
                //Debug.Log(sp_offsetColectabileV.floatValue);
            }
        }

        private bool ActiveazaCategorie()
        {
            cat_SETARI = false;
            cat_OBSTACOLE = false;
            cat_COLECTABILE = false;

            return true;
        }

        private void BaraProgres(float value, string label)
        {
            Rect rect = GUILayoutUtility.GetRect(20, 20, "TextField");

            if (value <= 0)
            {
                value = 0.0f;
            }
            if (value >= 100.0f)
            {
                value = 100;
            }

            float val_Nef = value / 100.0f;


            EditorGUI.ProgressBar(rect, val_Nef, label + " : " + value.ToString() + " %");
        }




        private void OnDisable()
        {
            listaObstacoleStanga.drawElementCallback -= ObstacolStanga_DrawCallback;
            listaObstacoleDreapta.drawElementCallback -= ObstacolDreapta_DrawCallback;
        }




        #region LISTA REORDABLE: OBSTACOLE & COLECTABILE

        //OBSTACOLE STANGA
        private void ObstacolStanga_DrawCallback(Rect rect, int index, bool esteActiv, bool esteFocusat)
        {
            var element = listaObstacoleStanga.serializedProperty.GetArrayElementAtIndex(index);
            var _id = element.FindPropertyRelative("layerObs");
            var _refObiect = element.FindPropertyRelative("refObstacol");
            var _ico = element.FindPropertyRelative("tipObstacol");
            var _blc = element.FindPropertyRelative("pozitieObstacol");

            var latButonOchi = 40;

            Rect rectId = rect;
            rectId.y += 5;
            //r1.width *= 0.5f;
            rectId.width = (rect.width / 1.5f) - 20 - latButonOchi - 15;
            rectId.height = EditorGUIUtility.singleLineHeight;

            Rect rectPiesa = rectId;
            rectPiesa.xMin = rectId.xMax + 10;
            //r2.xMax = rect.xMax;
            rectPiesa.xMax = rect.xMax - latButonOchi - 5;

            Rect rectIco = rect;
            rectIco.y += 28;
            //rectIco.width *= 0.5f;
            rectIco.width = (rect.width / 1.7f) - 15 - latButonOchi - 5;
            rectIco.height = EditorGUIUtility.singleLineHeight;

            Rect rectBlocat = rectIco;
            rectBlocat.xMin = rectIco.xMax + 10;
            rectBlocat.xMax = rect.xMax - latButonOchi - 5;


            //// BUTON AFISARE/NEAFISARE
            Rect rButon1 = rectId;
            rButon1.xMin = rectPiesa.xMax + 5;
            rButon1.xMax = rect.xMax;

            //// BUTON FUNCTIE SUPLIMENTARA
            Rect rButon2 = rectIco;
            //rButon1.xMin = rectPiesa.xMax + 5;
            rButon2.xMin = rectBlocat.xMax + 5;
            rButon2.xMax = rect.xMax;

            // REFERINTA  ACCESORIU*****************************
            GUIContent label_P = new GUIContent("Obstacol");
            Vector2 sizeP = EditorStyles.label.CalcSize(label_P);
            EditorGUIUtility.labelWidth = sizeP.x + 15.0f;

            EditorGUI.PropertyField(rectId, _refObiect, label_P);
            EditorGUIUtility.labelWidth = 0;

            //// DENUMIRE ACCESORIU & ID**************************
            GUIContent label_D = new GUIContent("Layer");
            Vector2 sizeD = EditorStyles.label.CalcSize(label_D);
            EditorGUIUtility.labelWidth = sizeD.x + 15.0f;

            EditorGUI.PropertyField(rectPiesa, _id, label_D);
            EditorGUIUtility.labelWidth = 0;



            //// REFERINTA ICONITA ACCESORIU************************
            GUIContent label_Pr = new GUIContent("Tip");
            Vector2 sizePr = EditorStyles.label.CalcSize(label_Pr);
            EditorGUIUtility.labelWidth = sizePr.x + 15.0f;

            EditorGUI.PropertyField(rectIco, _ico, label_Pr);
            EditorGUIUtility.labelWidth = 0;


            //// ESTE BLOCAT ACEST ACCESORIU*************************
            GUIContent label_B = new GUIContent("Pozitie");
            Vector2 sizeB = EditorStyles.label.CalcSize(label_B);
            EditorGUIUtility.labelWidth = sizeB.x + 15.0f;

            EditorGUI.PropertyField(rectBlocat, _blc, label_B);
            EditorGUIUtility.labelWidth = 0;

            //// BUTON VIZUALIZARE ACCESORIU & ICONITA**************************
            GUI.color = Color.white;

            //// DACA AFISAREA ESTE ACTIVA SCHIMB BUTONUL CU "OPRIRE AFISARE"
            //if (segment.listaCapcaneStanga[index].refObstacol != null && segment.listaCapcaneStanga[index].refObstacol.gameObject.activeInHierarchy && vizualizareObstacoleStanga)
            //{
            //    if (GUI.Button(rButon1, new GUIContent(ico_ochiOff, "Oprire afisare")))
            //    {
            //        segment.listaCapcaneStanga[index].refObstacol.gameObject.SetActive(false);
            //        vizualizareObstacoleStanga = false;
            //    }
            //}
            ////// DACA AFISAREA NU ESTE ACTIVA SCHIMB BUTONUL CU "AFISARE ELERON"
            //else if (segment.listaCapcaneStanga[index].refObstacol != null && segment.listaCapcaneStanga[index].refObstacol.gameObject.activeInHierarchy && !vizualizareObstacoleDreapta)
            //{
            //    if (GUI.Button(rButon1, new GUIContent(ico_ochi, "Afisare Eleron")))
            //    {
            //        segment.listaCapcaneStanga[index].refObstacol.gameObject.SetActive(true);
            //        vizualizareObstacoleStanga = true;
            //    }
            //    // CAZ: AFISARE BUTON "AFISARE ELERON" PENTRU REZTUL DE ELEMENTE DIN LISTA 
            //}
            //else
            //{
            //    if (GUI.Button(rButon1, new GUIContent(ico_ochi, "Afisare Eleron")))
            //    {

            //        for (int i = 0; i < segment.listaCapcaneStanga.Count; i++)
            //        {
            //            if (segment.listaCapcaneStanga[i].refObstacol != segment.listaCapcaneStanga[index].refObstacol)
            //            {
            //                if (segment.listaCapcaneStanga[i].refObstacol != null && segment.listaCapcaneStanga[i].refObstacol.gameObject.activeInHierarchy)
            //                    segment.listaCapcaneStanga[i].refObstacol.gameObject.SetActive(false);
            //            }
            //            else
            //            {
            //                if (segment.listaCapcaneStanga[i].refObstacol != null) // sa adaug eroare
            //                    segment.listaCapcaneStanga[i].refObstacol.gameObject.SetActive(true);
            //            }
            //        }
            //        vizualizareObstacoleStanga = true;
            //    }
            //}
            GUI.color = culoarePrestabilit;

            if (esteFocusat)
            {
                indexObstacolStangaFocusat = index;
            }
        }

        private void ObstacolStanga_AddCallback(ReorderableList lista)
        {
            float limitaSus = segment.punctSfarsit.transform.position.y - segment.offsetMargini;

            if (segment.distantareObstacoleStanga >= limitaSus)
            {
                int optiuniRaspuns = EditorUtility.DisplayDialogComplex("AI ATINS LIMITA DE OBSTACOLE!",
               "Limita TOP: <b>" + limitaSus + "</b> Daca doresti sa mai adaugi Obtsacole, mareste limitele in Inspector: ",
               "Mareste",
               "Renunta",
               "Nu stiu ce sa fac");

                switch (optiuniRaspuns)
                {
                    case 0:
                        segment.offsetMargini -= 2.0f;
                        Debug.Log("MARESTE!");
                        break;

                    case 1:
                        Debug.Log("RENUNTA!");
                        break;

                    case 2:
                        Debug.Log("NU STIE :D");
                        break;
                }
            }
            else
            {
                segment.AdaugaObstacolNou("Stanga");
            }
        }

        private void ObstacolStanga_RemoveCallback(ReorderableList lista)
        {
            segment.ELIMINA_OBSTACOL("Stanga", indexObstacolStangaFocusat);
        }

        //OBSTACOLE DREAPTA
        private void ObstacolDreapta_DrawCallback(Rect rect, int index, bool esteActiv, bool esteFocusat)
        {
            var element = listaObstacoleDreapta.serializedProperty.GetArrayElementAtIndex(index);

            var _id = element.FindPropertyRelative("layerObs");
            var _refObiect = element.FindPropertyRelative("refObstacol");
            var _ico = element.FindPropertyRelative("tipObstacol");
            var _blc = element.FindPropertyRelative("pozitieObstacol");

            var latButonOchi = 40;

            Rect rectId = rect;
            rectId.y += 5;
            //r1.width *= 0.5f;
            rectId.width = (rect.width / 1.5f) - 20 - latButonOchi - 15;
            rectId.height = EditorGUIUtility.singleLineHeight;

            Rect rectPiesa = rectId;
            rectPiesa.xMin = rectId.xMax + 10;
            //r2.xMax = rect.xMax;
            rectPiesa.xMax = rect.xMax - latButonOchi - 5;

            Rect rectIco = rect;
            rectIco.y += 28;
            //rectIco.width *= 0.5f;
            rectIco.width = (rect.width / 1.7f) - 15 - latButonOchi - 5;
            rectIco.height = EditorGUIUtility.singleLineHeight;

            Rect rectBlocat = rectIco;
            rectBlocat.xMin = rectIco.xMax + 10;
            rectBlocat.xMax = rect.xMax - latButonOchi - 5;


            //// BUTON AFISARE/NEAFISARE
            Rect rButon1 = rectId;
            rButon1.xMin = rectPiesa.xMax + 5;
            rButon1.xMax = rect.xMax;

            //// BUTON FUNCTIE SUPLIMENTARA
            Rect rButon2 = rectIco;
            //rButon1.xMin = rectPiesa.xMax + 5;
            rButon2.xMin = rectBlocat.xMax + 5;
            rButon2.xMax = rect.xMax;

            // REFERINTA  ACCESORIU*****************************
            GUIContent label_P = new GUIContent("Obstacol");
            Vector2 sizeP = EditorStyles.label.CalcSize(label_P);
            EditorGUIUtility.labelWidth = sizeP.x + 15.0f;

            EditorGUI.PropertyField(rectId, _refObiect, label_P);
            EditorGUIUtility.labelWidth = 0;

            //// DENUMIRE ACCESORIU & ID**************************
            GUIContent label_D = new GUIContent("Layer");
            Vector2 sizeD = EditorStyles.label.CalcSize(label_D);
            EditorGUIUtility.labelWidth = sizeD.x + 15.0f;

            EditorGUI.PropertyField(rectPiesa, _id, label_D);
            EditorGUIUtility.labelWidth = 0;



            //// REFERINTA ICONITA ACCESORIU************************
            GUIContent label_Pr = new GUIContent("Tip");
            Vector2 sizePr = EditorStyles.label.CalcSize(label_Pr);
            EditorGUIUtility.labelWidth = sizePr.x + 15.0f;

            EditorGUI.PropertyField(rectIco, _ico, label_Pr);
            EditorGUIUtility.labelWidth = 0;


            //// ESTE BLOCAT ACEST ACCESORIU*************************
            GUIContent label_B = new GUIContent("Pozitie");
            Vector2 sizeB = EditorStyles.label.CalcSize(label_B);
            EditorGUIUtility.labelWidth = sizeB.x + 15.0f;

            EditorGUI.PropertyField(rectBlocat, _blc, label_B);
            EditorGUIUtility.labelWidth = 0;

            //// BUTON VIZUALIZARE ACCESORIU & ICONITA**************************
            GUI.color = Color.white;

            //// DACA AFISAREA ESTE ACTIVA SCHIMB BUTONUL CU "OPRIRE AFISARE"
            if (GUI.Button(rButon1, new GUIContent(ico_ochi, "ELIMINA")))
            {
                int optiuniRaspuns = EditorUtility.DisplayDialogComplex("ELIMINARE OBSTACOL DREAPTA!",
                "Esti sigur ca doresti sa elimini acest Obstacol? ", "DA", "NU", "");

                switch (optiuniRaspuns)
                {
                    case 0:
                        DestroyImmediate(segment.listaCapcaneDreapta[index].refObstacol.gameObject);
                        segment.listaCapcaneDreapta.RemoveAt(index);
                        break;

                    case 1:
                        Debug.Log("Ai renuntat sa elimini un Obstacol din Dreapta!");
                        break;
                }
            }

            if (GUI.Button(rButon2, new GUIContent(ico_ochiOff, "SELECT")))
            {
                segment.SelecteazaObiect(index);
                Selection.SetActiveObjectWithContext(segment.listaCapcaneDreapta[index].refObstacol, null);
            }

            //vizualizareObstacoleDreapta = true;
            GUI.color = culoarePrestabilit;

            if (esteFocusat)
            {
                indexObstacolDreaptaFocusat = index;
            }

        }

        private void ObstacolDreapta_AddCallback(ReorderableList lista)
        {
            float limitaSus = segment.punctSfarsit.transform.position.y - segment.offsetMargini;

            if (segment.distantareObstacoleDreapta >= limitaSus - 1.0f)
            {
                int optiuniRaspuns = EditorUtility.DisplayDialogComplex("AI ATINS LIMITA DE OBSTACOLE!",
               "Limita TOP: " + limitaSus + "Daca doresti sa mai adaugi Obtsacole, mareste limitele in Inspector: ",
               "Mareste",
               "Renunta",
               "Nu stiu ce sa fac");

                switch (optiuniRaspuns)
                {
                    case 0:
                        segment.offsetMargini -= 2.0f;
                        Debug.Log("MARESTE!");
                        break;

                    case 1:
                        Debug.Log("RENUNTA!");
                        break;

                    case 2:
                        Debug.Log("NU STIE :D");
                        break;
                }
            }
            else
            {
                segment.AdaugaObstacolNou("Dreapta");
            }
        }

        private void ObstacolDreapta_RemoveCallback(ReorderableList lista)
        {
            segment.ELIMINA_OBSTACOL("Dreapta", indexObstacolDreaptaFocusat);
        }

        private void ObstacolDreapta_SelectCallback(ReorderableList lista)
        {
            segment.SelecteazaObiect(lista.index);
            Selection.SetActiveObjectWithContext(segment.listaCapcaneDreapta[lista.index].refObstacol, null);
            //Debug.Log(lista.index);
        }


        //CALLBACK-URI LISTE REORDABLE COLECTABILE  "STANGA"
        private void ColectabilStanga_DrawCallback(Rect rect, int index, bool esteActiv, bool esteFocusat)
        {
            var element = listaColectabileStanga.serializedProperty.GetArrayElementAtIndex(index);

            var _id = element.FindPropertyRelative("idColectabil");
            var _refObiect = element.FindPropertyRelative("refColectabil");
            var _ico = element.FindPropertyRelative("tipColectabil");
            var _blc = element.FindPropertyRelative("pozitieColectabil");

            var latButonOchi = 40;

            Rect rectId = rect;
            rectId.y += 5;
            //r1.width *= 0.5f;
            rectId.width = (rect.width / 1.5f) - 20 - latButonOchi - 15;
            rectId.height = EditorGUIUtility.singleLineHeight;

            Rect rectPiesa = rectId;
            rectPiesa.xMin = rectId.xMax + 10;
            //r2.xMax = rect.xMax;
            rectPiesa.xMax = rect.xMax - latButonOchi - 5;

            Rect rectIco = rect;
            rectIco.y += 28;
            //rectIco.width *= 0.5f;
            rectIco.width = (rect.width / 1.7f) - 15 - latButonOchi - 5;
            rectIco.height = EditorGUIUtility.singleLineHeight;

            Rect rectBlocat = rectIco;
            rectBlocat.xMin = rectIco.xMax + 10;
            rectBlocat.xMax = rect.xMax - latButonOchi - 5;


            //// BUTON AFISARE/NEAFISARE
            Rect rButon1 = rectId;
            rButon1.xMin = rectPiesa.xMax + 5;
            rButon1.xMax = rect.xMax;

            //// BUTON FUNCTIE SUPLIMENTARA
            Rect rButon2 = rectIco;
            //rButon1.xMin = rectPiesa.xMax + 5;
            rButon2.xMin = rectBlocat.xMax + 5;
            rButon2.xMax = rect.xMax;

            // REFERINTA  ACCESORIU*****************************
            GUIContent label_P = new GUIContent("Obstacol");
            Vector2 sizeP = EditorStyles.label.CalcSize(label_P);
            EditorGUIUtility.labelWidth = sizeP.x + 15.0f;

            EditorGUI.PropertyField(rectId, _refObiect, label_P);
            EditorGUIUtility.labelWidth = 0;

            //// DENUMIRE ACCESORIU & ID**************************
            GUIContent label_D = new GUIContent("Layer");
            Vector2 sizeD = EditorStyles.label.CalcSize(label_D);
            EditorGUIUtility.labelWidth = sizeD.x + 15.0f;

            EditorGUI.PropertyField(rectPiesa, _id, label_D);
            EditorGUIUtility.labelWidth = 0;



            //// REFERINTA ICONITA ACCESORIU************************
            GUIContent label_Pr = new GUIContent("Tip");
            Vector2 sizePr = EditorStyles.label.CalcSize(label_Pr);
            EditorGUIUtility.labelWidth = sizePr.x + 15.0f;

            EditorGUI.PropertyField(rectIco, _ico, label_Pr);
            EditorGUIUtility.labelWidth = 0;


            //// ESTE BLOCAT ACEST ACCESORIU*************************
            GUIContent label_B = new GUIContent("Pozitie");
            Vector2 sizeB = EditorStyles.label.CalcSize(label_B);
            EditorGUIUtility.labelWidth = sizeB.x + 15.0f;

            EditorGUI.PropertyField(rectBlocat, _blc, label_B);
            EditorGUIUtility.labelWidth = 0;

            //// BUTON VIZUALIZARE ACCESORIU & ICONITA**************************
            GUI.color = Color.white;

            //// DACA AFISAREA ESTE ACTIVA SCHIMB BUTONUL CU "OPRIRE AFISARE"
            if (GUI.Button(rButon1, new GUIContent(ico_ochi, "ELIMINA")))
            {
                int optiuniRaspuns = EditorUtility.DisplayDialogComplex("ELIMINARE OBSTACOL DREAPTA!",
                "Esti sigur ca doresti sa elimini acest Obstacol? ", "DA", "NU", "");

                switch (optiuniRaspuns)
                {
                    case 0:
                        DestroyImmediate(segment.listaCapcaneDreapta[index].refObstacol.gameObject);
                        segment.listaCapcaneDreapta.RemoveAt(index);
                        break;

                    case 1:
                        Debug.Log("Ai renuntat sa elimini un Obstacol din Dreapta!");
                        break;
                }
            }

            if (GUI.Button(rButon2, new GUIContent(ico_ochiOff, "SELECT")))
            {
                segment.SelecteazaObiect(index);
                Selection.SetActiveObjectWithContext(segment.listaCapcaneDreapta[index].refObstacol, null);
            }

            //vizualizareObstacoleDreapta = true;
            GUI.color = culoarePrestabilit;

            if (esteFocusat)
            {
                indexColectabilStangaFocusat = index;
            }

        }

        private void ColectabilStangaAddCallback(ReorderableList lista)
        {
            float limitaSus = segment.punctSfarsit.transform.position.y - segment.offsetMargini;

            if (segment.distantareObstacoleDreapta >= limitaSus - 1.0f)
            {
                int optiuniRaspuns = EditorUtility.DisplayDialogComplex("AI ATINS LIMITA DE OBSTACOLE!",
               "Limita TOP: " + limitaSus + "Daca doresti sa mai adaugi Obtsacole, mareste limitele in Inspector: ",
               "Mareste",
               "Renunta",
               "Nu stiu ce sa fac");

                switch (optiuniRaspuns)
                {
                    case 0:
                        segment.offsetMargini -= 2.0f;
                        Debug.Log("MARESTE!");
                        break;

                    case 1:
                        Debug.Log("RENUNTA!");
                        break;

                    case 2:
                        Debug.Log("NU STIE :D");
                        break;
                }
            }
            else
            {
                segment.AdaugaObstacolNou("Dreapta");
            }
        }

        private void ColectabilStanga_RemoveCallback(ReorderableList lista)
        {
            //segment.ELIMINA_OBSTACOL("Dreapta", indexObstacolDreaptaFocusat);
        }

        //CALLBACK-URI LISTE REORDABLE COLECTABILE  "STANGA"
        private void ColectabilDreapta_DrawCallback(Rect rect, int index, bool esteActiv, bool esteFocusat)
        {
            var element = listaColectabileDreapta.serializedProperty.GetArrayElementAtIndex(index);

            var _id = element.FindPropertyRelative("idColectabil");
            var _refObiect = element.FindPropertyRelative("refColectabil");
            var _ico = element.FindPropertyRelative("tipColectabil");
            var _blc = element.FindPropertyRelative("pozitieColectabil");

            var latButonOchi = 40;

            Rect rectId = rect;
            rectId.y += 5;
            //r1.width *= 0.5f;
            rectId.width = (rect.width / 1.5f) - 20 - latButonOchi - 15;
            rectId.height = EditorGUIUtility.singleLineHeight;

            Rect rectPiesa = rectId;
            rectPiesa.xMin = rectId.xMax + 10;
            //r2.xMax = rect.xMax;
            rectPiesa.xMax = rect.xMax - latButonOchi - 5;

            Rect rectIco = rect;
            rectIco.y += 28;
            //rectIco.width *= 0.5f;
            rectIco.width = (rect.width / 1.7f) - 15 - latButonOchi - 5;
            rectIco.height = EditorGUIUtility.singleLineHeight;

            Rect rectBlocat = rectIco;
            rectBlocat.xMin = rectIco.xMax + 10;
            rectBlocat.xMax = rect.xMax - latButonOchi - 5;


            //// BUTON AFISARE/NEAFISARE
            Rect rButon1 = rectId;
            rButon1.xMin = rectPiesa.xMax + 5;
            rButon1.xMax = rect.xMax;

            //// BUTON FUNCTIE SUPLIMENTARA
            Rect rButon2 = rectIco;
            //rButon1.xMin = rectPiesa.xMax + 5;
            rButon2.xMin = rectBlocat.xMax + 5;
            rButon2.xMax = rect.xMax;

            // REFERINTA  ACCESORIU*****************************
            GUIContent label_P = new GUIContent("Obstacol");
            Vector2 sizeP = EditorStyles.label.CalcSize(label_P);
            EditorGUIUtility.labelWidth = sizeP.x + 15.0f;

            EditorGUI.PropertyField(rectId, _refObiect, label_P);
            EditorGUIUtility.labelWidth = 0;

            //// DENUMIRE ACCESORIU & ID**************************
            GUIContent label_D = new GUIContent("Layer");
            Vector2 sizeD = EditorStyles.label.CalcSize(label_D);
            EditorGUIUtility.labelWidth = sizeD.x + 15.0f;

            EditorGUI.PropertyField(rectPiesa, _id, label_D);
            EditorGUIUtility.labelWidth = 0;



            //// REFERINTA ICONITA ACCESORIU************************
            GUIContent label_Pr = new GUIContent("Tip");
            Vector2 sizePr = EditorStyles.label.CalcSize(label_Pr);
            EditorGUIUtility.labelWidth = sizePr.x + 15.0f;

            EditorGUI.PropertyField(rectIco, _ico, label_Pr);
            EditorGUIUtility.labelWidth = 0;


            //// ESTE BLOCAT ACEST ACCESORIU*************************
            GUIContent label_B = new GUIContent("Pozitie");
            Vector2 sizeB = EditorStyles.label.CalcSize(label_B);
            EditorGUIUtility.labelWidth = sizeB.x + 15.0f;

            EditorGUI.PropertyField(rectBlocat, _blc, label_B);
            EditorGUIUtility.labelWidth = 0;

            //// BUTON VIZUALIZARE ACCESORIU & ICONITA**************************
            GUI.color = Color.white;

            //// DACA AFISAREA ESTE ACTIVA SCHIMB BUTONUL CU "OPRIRE AFISARE"
            if (GUI.Button(rButon1, new GUIContent(ico_ochi, "ELIMINA")))
            {
                int optiuniRaspuns = EditorUtility.DisplayDialogComplex("ELIMINARE OBSTACOL DREAPTA!",
                "Esti sigur ca doresti sa elimini acest Obstacol? ", "DA", "NU", "");

                switch (optiuniRaspuns)
                {
                    case 0:
                        DestroyImmediate(segment.listaCapcaneDreapta[index].refObstacol.gameObject);
                        segment.listaCapcaneDreapta.RemoveAt(index);
                        break;

                    case 1:
                        Debug.Log("Ai renuntat sa elimini un Obstacol din Dreapta!");
                        break;
                }
            }

            if (GUI.Button(rButon2, new GUIContent(ico_ochiOff, "SELECT")))
            {
                segment.SelecteazaObiect(index);
                Selection.SetActiveObjectWithContext(segment.listaCapcaneDreapta[index].refObstacol, null);
            }

            //vizualizareObstacoleDreapta = true;
            GUI.color = culoarePrestabilit;

            if (esteFocusat)
            {
                indexColectabilDreaptaFocusat = index;
            }

        }

        private void ColectabilDreaptaAddCallback(ReorderableList lista)
        {
            float limitaSus = segment.punctSfarsit.transform.position.y - segment.offsetMargini;

            if (segment.distantareObstacoleDreapta >= limitaSus - 1.0f)
            {
                int optiuniRaspuns = EditorUtility.DisplayDialogComplex("AI ATINS LIMITA DE OBSTACOLE!",
               "Limita TOP: " + limitaSus + "Daca doresti sa mai adaugi Obtsacole, mareste limitele in Inspector: ",
               "Mareste",
               "Renunta",
               "Nu stiu ce sa fac");

                switch (optiuniRaspuns)
                {
                    case 0:
                        segment.offsetMargini -= 2.0f;
                        Debug.Log("MARESTE!");
                        break;

                    case 1:
                        Debug.Log("RENUNTA!");
                        break;

                    case 2:
                        Debug.Log("NU STIE :D");
                        break;
                }
            }
            else
            {
                segment.AdaugaObstacolNou("Dreapta");
            }
        }

        private void ColectabilDreapta_RemoveCallback(ReorderableList lista)
        {
            //segment.ELIMINA_OBSTACOL("Dreapta", indexObstacolDreaptaFocusat);
        }

        #endregion
    }
}
