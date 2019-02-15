using SA.CrossPlatform.App;
using UnityEngine;

public class UM_CameraExamples : MonoBehaviour {


	void Capture () {
        var cameraService = UM_Application.CameraService;

        int maxThumbnailSize = 1024;
        cameraService.TakePicture(maxThumbnailSize, (result) => {
           if(result.IsSucceeded) {
                UM_Media media = result.Media;

                Texture2D image = media.Thumbnail;
                Debug.Log("Thumbnail width: " + image.width + " / height: " + image.height);
                Debug.Log("mdeia.Type: " + media.Type);
                Debug.Log("mdeia.Path: " + media.Path);
            } else {
                Debug.Log("failed to take a picture: " + result.Error.FullMessage);
            }
        });
	}

    void CaptureVideo() {
        var cameraService = UM_Application.CameraService;

        int maxThumbnailSize = 1024;
        cameraService.TakeVideo(maxThumbnailSize, (result) => {
            if (result.IsSucceeded) {
                UM_Media media = result.Media;

                Texture2D image = media.Thumbnail;
                Debug.Log("Thumbnail width: " + image.width + " / height: " + image.height);
                Debug.Log("mdeia.Type: " + media.Type);
                Debug.Log("mdeia.Path: " + media.Path);
            } else {
                Debug.Log("failed to take a picture: " + result.Error.FullMessage);
            }
        });
    }

}
