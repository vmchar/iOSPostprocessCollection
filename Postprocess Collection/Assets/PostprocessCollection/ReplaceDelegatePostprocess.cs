#if UNITY_EDITOR && UNITY_IOS

using UnityEditor;

namespace PostprocessCollection
{
    
    public static class ReplaceDelegatePostprocess
    {
        private const string DefaultDelegateName = "UnityAppController.mm";

        public static void ReplaceDelegate(DefaultAsset newDelegateFile, string buildPath)
        {
            if(newDelegateFile == null) return;
            //get paths to new and old delegate
            var newDelegatePath = AssetDatabase.GetAssetPath(newDelegateFile);
            var delegatePath = buildPath + "/Classes/";
            var oldDelegatePath = delegatePath + DefaultDelegateName;
			
            //remove old delegate, add new one with default unity name
            FileUtil.DeleteFileOrDirectory(oldDelegatePath);
            FileUtil.CopyFileOrDirectory(newDelegatePath, delegatePath + DefaultDelegateName);
        }
    }
}

#endif