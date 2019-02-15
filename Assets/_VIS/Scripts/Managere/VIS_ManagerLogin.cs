using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SA.Facebook;
using TMPro;
/***********************************
 * CopyRight 2019
 * Programmer: Buraca Dorin
 * Programmer: Socea Tiberiu
 * Website: http://www.VirtualInfinityStudios.ro
 * Application: Climber
 *  ***********************************/
namespace VirtualInfinityStudios
{
    public class VIS_ManagerLogin : MonoBehaviour
    {
        [Header("DEBUG")]
        public bool testLive = false;

        [Header("LOGARE")]
        public TMP_InputField numeUtilizator;
        public TMP_InputField parolaUtilizator;

        [Space(5)]

        [Header("INREGISTRARE")]
        //public VIS_ButonAvansat butonTest;

        [Header("RETELE DE SOCIALIZARE")]
        public TextMeshProUGUI numeTest;
        public TextMeshProUGUI emailTest;
        public RawImage pozaAvatar;
        public bool fbInitiat = false;
        public bool utilizatorLogatCuFb = false;

        private void Awake()
        {
            SA_FB.Init(() =>
            {
                fbInitiat = SA_FB.IsInitialized;
                //Debug.Log("Init Completed");

                if (testLive)
                {
                    utilizatorLogatCuFb = true;
                    ExtrageInfoUtilizatorDePeFb();
                }
            });
        }

        public void SignInFb()
        {
            if (fbInitiat)
            {
                SA_FB.Login((result) =>
                {
                    if (result.IsSucceeded)
                    {
                        Debug.Log("Login Succeeded");
                        utilizatorLogatCuFb = true;
                        Invoke("ExtrageInfoUtilizatorDePeFb", 1f);
                    }
                    else
                    {
                        Debug.Log("Failed to login: " + result.Error.FullMessage);
                    }
                });
            }
        }

        private void ExtrageInfoUtilizatorDePeFb()
        {
            if (utilizatorLogatCuFb)
            {
                SA_FB.GetLoggedInUserInfo((rezultat) =>
                {
                    if (rezultat.IsSucceeded)
                    {
                        ActualizareDateUtilizator(rezultat.User);

                        //Debug.Log("result.User.Id: " + rezultat.User.Id);
                        //Debug.Log("result.User.Name: " + rezultat.User.Name);
                        //Debug.Log("result.User.UserName: " + rezultat.User.UserName);
                        //Debug.Log("result.User.FirstName: " + rezultat.User.FirstName);
                        //Debug.Log("result.User.LastName: " + rezultat.User.LastName);

                        //Debug.Log("result.User.Locale: " + rezultat.User.Locale);
                        //Debug.Log("result.User.Location: " + rezultat.User.Location);
                        //Debug.Log("result.User.PictureUrl: " + rezultat.User.PictureUrl);
                        //Debug.Log("result.User.ProfileUrl: " + rezultat.User.ProfileUrl);
                        //Debug.Log("result.User.AgeRange: " + rezultat.User.AgeRange);
                        //Debug.Log("result.User.Birthday: " + rezultat.User.Birthday);
                        //Debug.Log("result.User.Gender: " + rezultat.User.Gender);
                        //Debug.Log("result.User.AgeRange: " + rezultat.User.AgeRange);
                        //Debug.Log("result.User.RawJSON: " + rezultat.User.RawJSON);
                    }
                    else
                    {
                        Debug.Log("Failed to load user Info: " + rezultat.Error.FullMessage);
                    }
                });
            }
            else
            {
                Debug.Log("Ups: extragere info FB esuat!");
                utilizatorLogatCuFb = false;
            }
        }

        private void ActualizareDateUtilizator(SA_FB_User utilizator)
        {
            numeTest.text = utilizator.Name;
            emailTest.text = utilizator.Email;

            SA_FB_Analytics.LogAppEvent("Utilizator conectat cu succes", null, null);

            utilizator.GetProfileImage(SA_FB_ProfileImageSize.large, (texture) =>
            {
                pozaAvatar.texture = texture;
            });

        }

        [ContextMenu("FUNC BTN TEST")]
        public void AtasareFuncTest()
        {
            //butonTest.AtasareFunctiePersonalizata("Test reusit!");
        }
    }
}
