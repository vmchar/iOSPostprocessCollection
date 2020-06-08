#if UNITY_IOS && UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace PostprocessCollection
{
    /// <summary>
    /// Main class of the PostprocessCollection.
    /// Hold the setting scriptable object and
    /// starts all the other postprocess actions.
    /// </summary>
    public class PostProcessCollectionExecutor
    {
        public CustomXcodeProjectModifications XcodeModificationsScriptableObject;

        [PostProcessBuild(999)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            var xcodeModifications = FindModification();
            if(xcodeModifications == null)
                Debug.LogError("PostProcessCollection error: Custom Xcode modification file with name 'MainXcodeModification' not found");
            var projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            var target_name = PBXProject.GetUnityTargetName();
            var target = project.TargetGuidByName(target_name);
            
            var plistPath = Path.Combine(path, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            
            AddFrameworksPostprocess.AddFrameworks(project, target, xcodeModifications.Frameworks);
            ProjectPropertyPostprocess.AddProperty(project, target, xcodeModifications.Flags);
            AddPropertiesPostprocess.AddProperties(plist, xcodeModifications.PlistKeys);
            CopyFilesPostprocess.CopyAllFilesFromDirectory(xcodeModifications.CopyFilesDirectory, path, project, target);
            AddEntitlementsPostprocess.AddEntitlements(xcodeModifications.EntitlementsFile, project, path, target, target_name);
            ReplaceDelegatePostprocess.ReplaceDelegate(xcodeModifications.NewDelegateFile, path);
            
            project.WriteToFile(projectPath);
            File.WriteAllText(projectPath, project.WriteToString());
            File.WriteAllText(plistPath, plist.WriteToString());
        }

        public static CustomXcodeProjectModifications FindModification()
        {
            var result = AssetDatabase.FindAssets("t:CustomXcodeProjectModifications");
            foreach (var s in result)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(s);
                if (assetPath.Contains("MainXcodeModification.asset"))
                {
                    return (CustomXcodeProjectModifications) AssetDatabase.LoadAssetAtPath
                            (AssetDatabase.GUIDToAssetPath(s), typeof(CustomXcodeProjectModifications));
                }
            }
            return null;
        }
    }
}

#endif