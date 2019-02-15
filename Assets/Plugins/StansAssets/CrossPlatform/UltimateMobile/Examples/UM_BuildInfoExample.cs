using UnityEngine;
using System.Collections;

using SA.CrossPlatform.App;

public class UM_BuildInfoExample : MonoBehaviour
{

  
    void Start() {
        UM_iBuildInfo buildInfo = UM_Build.Info;

        Debug.Log("buildInfo.Identifier: " + buildInfo.Identifier);
        Debug.Log("buildInfo.Version: " + buildInfo.Version);
    }


}
