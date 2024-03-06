using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class PreloadSigningAlias {

    static PreloadSigningAlias () {
        PlayerSettings.Android.keystorePass = "ccbussimcc";
        PlayerSettings.Android.keyaliasName = "key";
        PlayerSettings.Android.keyaliasPass = "ccbussimcc";
    }
}