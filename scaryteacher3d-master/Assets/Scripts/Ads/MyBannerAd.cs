using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBannerAd : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(ShowBannerAd());
    }

    private IEnumerator ShowBannerAd()
    {
        yield return new WaitForSeconds(5);
        AdController.Instance.ShowAdmobBanner();
    }
}
