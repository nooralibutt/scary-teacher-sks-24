#if UNITY_IPHONE
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using UnityEditor;

public class ChangeIOSBuildNumber {
 
    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject) {
 
        if (buildTarget == BuildTarget.iOS) {
       
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
       
            // Get root
            PlistElementDict rootDict = plist.root;

            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
       
            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());

            string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);
            string target = pbxProject.GetUnityMainTargetGuid();
            string target2 = pbxProject.GetUnityFrameworkTargetGuid();
            //string[] targetGuids = new string[2] { pbxProject.GetUnityMainTargetGuid(), pbxProject.GetUnityFrameworkTargetGuid() };

            pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
            pbxProject.SetBuildProperty(target2, "ENABLE_BITCODE", "NO");
            pbxProject.SetBuildProperty(target2, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            pbxProject.SetTeamId(target2, "NCF83SNTH4");
            pbxProject.WriteToFile(projectPath);
        }
    }
}
#endif