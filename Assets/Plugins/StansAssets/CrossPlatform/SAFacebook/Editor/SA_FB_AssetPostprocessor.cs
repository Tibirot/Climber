using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SA.Facebook
{
    public class SA_FB_AssetPostprocessor : AssetPostprocessor
    {


        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {

            //Deactivate when lib deleted
            foreach (var lib in importedAssets) {
                SA_FB_InstallationProcessing.ProcessAssetImport(lib);
            }

            //Activate when lib added
            foreach (var lib in deletedAssets) {
                SA_FB_InstallationProcessing.ProcessAssetDelete(lib);
            }
        }


    }
}