#if UNITY_IOS && UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor.iOS.Xcode;

namespace PostprocessCollection
{

    /// <summary>
    /// Adds properties to XCode project.
    /// For example it's common case to add
    /// a Linker Flag -Objc in OTHER_LDFLAGS to project.
    /// Also Facebook plugin may want you to add -lxml2 flag to OTHER_LDFLAGS.
    /// Both actions can be done using this post process.
    /// </summary>
    public static class ProjectPropertyPostprocess //: MonoBehaviour
    {
        public static void AddProperty(PBXProject proj, string targetGUID, List<BuildProperties> properties)
        {
            if(properties == null) return;
            foreach (var property in properties)
            {
                proj.AddBuildProperty(targetGUID, property.Name, property.Value);
            }
        }
    }
}

#endif