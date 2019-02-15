using UnityEngine;
using UnityEditor;

namespace SA.CrossPlatform
{
    public class UM_AssetPostprocessor : AssetPostprocessor
    {

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            foreach (string assetPath in importedAssets) {
                UM_DefinesResolver.ProcessAssetImport(assetPath);
            }


            foreach (string assetPath in deletedAssets) {
                UM_DefinesResolver.ProcessAssetDelete(assetPath);
            }
        }
    }
}