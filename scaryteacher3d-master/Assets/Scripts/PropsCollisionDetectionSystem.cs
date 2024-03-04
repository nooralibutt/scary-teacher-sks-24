using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsCollisionDetectionSystem : MonoBehaviour
{
    [SerializeField] private GameConstants.InGameConstants.LevelProps PropName;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameConstants.InGameConstants.GameCharacters.SchoolBoy.ToString()))
            FindObjectOfType<SchoolBoyController>().OnCollisionDetection(PropName.ToString());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(GameConstants.InGameConstants.GameCharacters.SchoolBoy.ToString()))
            FindObjectOfType<SchoolBoyController>().OnCollisionLeft(PropName.ToString());
    }
}
