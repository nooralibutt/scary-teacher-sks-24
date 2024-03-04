using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrivacyPolicyPanel : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Toggle acceptToggle;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(GameConstants.InGameConstants.PRIVACY_POLICY_SHOWN))
        {
            gameObject.SetActive(false);

            SceneTransitionManager.Instance.ShowLoadingScreen(100,100, () =>
            {
                mainMenuPanel.SetActive(true);
            });
        }
        else
        {
            acceptToggle.onValueChanged.AddListener(AcceptToggle);
        }
    }


    public void OpenPrivacyPolicyLink()
    {
        Application.OpenURL("https://sites.google.com/view/saudahmedprivacypolicy/home");
    }

    public void AcceptToggle(bool toggle)
    {
        if (toggle)
        {
            PlayerPrefs.SetInt(GameConstants.InGameConstants.PRIVACY_POLICY_SHOWN, 1);
            PlayerPrefs.Save();
            mainMenuPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
