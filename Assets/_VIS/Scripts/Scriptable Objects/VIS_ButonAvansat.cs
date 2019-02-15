using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/***********************************
 * CopyRight 2019
 * Programmer: Buraca Dorin
 * Programmer: Socea Tiberiu
 * Website: http://www.VirtualInfinityStudios.ro
 * Application: Climber
 ***********************************/
namespace VirtualInfinityStudios.ElementeUI
{
    [RequireComponent(typeof(Button))]
    public class VIS_ButonAvansat : MonoBehaviour
    {
        public enum TipButon { Standard, CuIconita, FaraIconita };

        public bool personalizeaza = false;

        public TextMeshProUGUI textButon;
        public Image fundalButon;
        public Image iconitaButon;
        //public TipButon tipButon;

        public Color culoareButon;
        public Color culoareIconita;

        public VIS_DataAspectButonAvansat template;
        public List<VIS_TemplateButonAvansat> listaSabloane = new List<VIS_TemplateButonAvansat>();
        public Button compButon;

        //public override void OnSkinUI()
        //{
        //    base.OnSkinUI();

        //    textButon.color = template.culoareTitlu;
        //    textButon.font = template.fontTitlu;
        //    textButon.fontSize = template.marimeText;

        //    //if (!personalizeaza)
        //    //{

        //    //    if (textButon != null)
        //    //    {
        //    //        textButon.color = dataAspect.culoareTitlu;
        //    //        textButon.font = dataAspect.fontTitlu;
        //    //        textButon.fontSize = dataAspect.marimeText;

        //    //        if (dataAspect.contur)
        //    //        {
        //    //            textButon.font = Resources.Load<TMP_FontAsset>(dataAspect.numeFont);
        //    //            textButon.fontSharedMaterial = Resources.Load<Material>(dataAspect.numeFontMaterial);
        //    //        }

        //    //        if (dataAspect.bold)
        //    //        {
        //    //            textButon.fontStyle = FontStyles.Bold;
        //    //        }
        //    //        else
        //    //        {
        //    //            textButon.fontStyle = FontStyles.Normal;
        //    //        }

        //    //    }


        //    //    if (iconitaButon != null)
        //    //    {
        //    //        iconitaButon.sprite = dataAspect.iconitaButon;
        //    //        iconitaButon.color = dataAspect.culoareIconita;
        //    //        iconitaButon.rectTransform.localScale = new Vector2(dataAspect.marimeICO, dataAspect.marimeICO);
        //    //    }

        //    //    if (fundalButon != null)
        //    //    {
        //    //        fundalButon.sprite = dataAspect.fundalButon;
        //    //        fundalButon.color = dataAspect.culoareButon;
        //    //    }
        //    //}
        //    //else { return; }
        //}


        public void ActualizeazaCuloriButon()
        {
            textButon.color = template.culoareTitlu;
            textButon.font = template.fontTitlu;
            textButon.fontSize = template.marimeText;

            fundalButon.color = culoareButon;
            iconitaButon.color = culoareIconita;
        }

        public void AtasareAutomataReferinte()
        {
            textButon = GameObject.Find("Titlu_Buton").GetComponent<TextMeshProUGUI>();
            iconitaButon = GameObject.Find("Iconita_Buton").GetComponent<Image>();
            fundalButon = GetComponent<Image>();
            compButon = GetComponent<Button>();

            compButon.onClick.AddListener(delegate { AtasareFunctieButon("Hello"); });
        }

        public void AtasareFunctiePersonalizata(string functie)
        {
            compButon.onClick.AddListener(delegate { AtasareFunctieButon(functie); });
        }


        public void AtasareFunctieButon(string functie)
        {
            Debug.Log("Buton apasat: " + functie);
        }
    }
}