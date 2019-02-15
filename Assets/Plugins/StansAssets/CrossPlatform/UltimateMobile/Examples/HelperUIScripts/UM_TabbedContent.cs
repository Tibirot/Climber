using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UM_TabbedContent : MonoBehaviour
{

    [SerializeField] GameObject m_buttonsContainer;
    [SerializeField] GameObject m_panlesContainer;

    [SerializeField] Color m_activeButtonColor;

    private List<Button> m_buttons = null;
    private List<GameObject> m_panels = new List<GameObject>();


    private void Awake() {
        m_buttons = new List<Button>(m_buttonsContainer.GetComponentsInChildren<Button>());
        foreach (var button in m_buttons) {
            button.onClick.AddListener(() => {
                ActivateTabAtIndex(m_buttons.IndexOf(button));
            });
        }

        foreach (Transform child in m_panlesContainer.transform) {
            m_panels.Add(child.gameObject);
        }

        ActivateTabAtIndex(0);
    }



    private void ActivateTabAtIndex(int index) {
        foreach(var panel in m_panels) {
            panel.SetActive(false);
        }

        m_panels[index].SetActive(true);

        foreach(var button in m_buttons) {
            button.GetComponent<Image>().color = button.colors.normalColor;
        }

        m_buttons[index].GetComponent<Image>().color = m_activeButtonColor;



    }
}
