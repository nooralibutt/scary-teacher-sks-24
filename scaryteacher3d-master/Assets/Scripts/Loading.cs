using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Image loadingFillImge;             // Fill image of loading bar.
    [SerializeField] private float loadingProgressRate;             // It is the rate of loading progress.

    void Start()
    {
        if (loadingFillImge != null)
        {
            StartCoroutine(StartLoading());
        }
    }

    // this co-routine fill the loading bar with some loading progress rate.
    IEnumerator StartLoading()
    {
        float currentValue = 0f;
        if (loadingFillImge != null)
        {
            while (currentValue < 1)    // As total fill Image value is 1.
            {
                currentValue += (40 * loadingProgressRate * Time.deltaTime) / 100;
                loadingFillImge.fillAmount = currentValue;
                yield return null;
            }
            SceneManager.LoadScene(1);              // After completion of loading progress, it load next scene.
        }
    }
}
