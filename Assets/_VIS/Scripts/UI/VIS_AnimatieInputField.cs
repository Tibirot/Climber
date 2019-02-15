using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VirtualInfinityStudios
{
    public class VIS_AnimatieInputField : MonoBehaviour
    {

        public bool cazSpecial = false;
        public TextMeshProUGUI textCamp;
        public TextMeshProUGUI textCount;
        public TextMeshProUGUI textInfo;

        public Image iconita;  //* de adaugat referinta

        public Color culoareIconitaInitial;
        public Color culoareIconitaAnimata;

        private TMP_InputField campInput;





        private void Awake()
        {
            campInput = GetComponent<TMP_InputField>();

            if (!cazSpecial)
            {
                textInfo.enabled = false;
                textCamp.text = "Username (max 30)";

                textCount.text = "0/30";
                textCount.enabled = false;
            }
            else
            {
                textInfo.enabled = false;
                textCamp.text = "Password";

                textCount.text = "";
                textCount.enabled = false;
            }
        }



        public void LaSchimbareaTextului()
        {
            if (!cazSpecial)
            {
                CalculText(campInput.text.Length, campInput.characterLimit);
            }
            else
            {
                return;
            }
        }

        public void LaSelectareaTextului()
        {
            AnimeazaText();
        }

        public void LaDeselectareaTextului()
        {
            OprireAnimatie();
        }






        #region FUNCTII TEXT

        private void AnimeazaText()
        {
            textInfo.enabled = true;
            textCamp.text = "";

            textCount.enabled = true;
        }



        private void CalculText(int nrCaractereCurent, int nrCaractereMax)
        {
            textCount.text = nrCaractereCurent + "/" + nrCaractereMax;
        }



        private void OprireAnimatie()
        {
            textInfo.enabled = false;
            textCount.enabled = false;

        }

        #endregion
    }
}
