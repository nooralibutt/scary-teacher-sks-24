#if UNITY_IOS
#endif

using EasyMobile;
using UnityEngine;
using UnityEngine.Advertisements;

public class AppTrackingTransparencyRequest : MonoBehaviour
{
    void Start()
    {

#if UNITY_IOS
        // For usage, use following code
        var previousStatus = Privacy.AppTrackingManager.TrackingAuthorizationStatus;

        if (previousStatus == AppTrackingAuthorizationStatus.ATTrackingManagerAuthorizationStatusNotDetermined)
        {
            Privacy.AppTrackingManager.RequestTrackingAuthorization(status =>
            {
                Debug.Log("App Tracking transparency status: " + status);

                // If the user opts out of targeted advertising:
                var gdprMetaData = new MetaData("gdpr");

                gdprMetaData.Set("consent",
                    status == AppTrackingAuthorizationStatus.ATTrackingManagerAuthorizationStatusAuthorized
                        ? "true" : "false");
                Advertisement.SetMetaData(gdprMetaData);
            });
        }
#endif
    }
}