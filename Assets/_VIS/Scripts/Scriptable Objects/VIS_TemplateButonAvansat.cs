using UnityEngine;
/***********************************
 * CopyRight 2019
 * Programmer: Buraca Dorin
 * Programmer: Socea Tiberiu
 * Website: http://www.VirtualInfinityStudios.ro
 * Application: Climber
 ***********************************/
namespace VirtualInfinityStudios.ElementeUI
{
    [ExecuteInEditMode()]
    public class VIS_TemplateButonAvansat : MonoBehaviour
    {
        public VIS_DataAspectButonAvansat dataAspect;

        public virtual void OnSkinUI()
        {

        }

        public virtual void Awake()
        {
            if (dataAspect != null)
                OnSkinUI();
        }


#if UNITY_EDITOR
        public virtual void Update()
        {
            //Costisitor in RUNTIME , trebuie de adaugat aceasta exectutie doar in editor
            if (dataAspect != null)
                OnSkinUI();
        }
#endif
    }
}
