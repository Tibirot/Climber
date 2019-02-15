using UnityEngine;
using SA.CrossPlatform.App;
using UnityEngine.UI;


using SA.iOS.Foundation;
using SA.iOS.AVFoundation;
using SA.iOS.AVKit;


public class UM_GalleryExample : MonoBehaviour {

    [SerializeField] Button m_saveImageBtn;
    [SerializeField] Button m_pickImageBtn;
    [SerializeField] Button m_pickVideoBtn;

    private void Awake() {
        m_saveImageBtn.onClick.AddListener(SaveImage);
        m_pickImageBtn.onClick.AddListener(PickImage);
        m_pickVideoBtn.onClick.AddListener(PickVideo);
    }

    private void SaveImageToStorage(Texture2D image) {
        var gallery = UM_Application.GalleryService;
        gallery.SaveImage(image, "MyImage", (result) => {
            if (result.IsSucceeded) {
                Debug.Log("Saved");
            } else {
                Debug.Log("Failed: " + result.Error.FullMessage);
            }
        });
    }

    private void SaveImage() {
        Debug.Log("SaveImage test");
        var camera = UM_Application.CameraService;
        int maxThumbnailSize = 1024;
        camera.TakePicture(maxThumbnailSize, (result) => {
            if (result.IsSucceeded) {
                SaveImageToStorage(result.Media.Thumbnail);
            } else {
                Debug.Log("failed to take a picture: " + result.Error.FullMessage);
            }
        });


        /*
        Texture2D tex = new Texture2D(1, 1);// Your image. Image should be redeable.
        var gallery = UM_Application.GalleryService;
        gallery.SaveImage(tex, "MyImage", result => {
            if(result.IsSucceeded) {
                Debug.Log("Saved");
            } else {
                Debug.Log("Failed: " + result.Error.FullMessage);
            }
        });

        gallery.SaveScreenshot("MySceeen", result => {
            if (result.IsSucceeded) {
                Debug.Log("Saved");
            } else {
                Debug.Log("Failed: " + result.Error.FullMessage);
            }
        });
        */
    }

    private void PickImage() {
        var gallery = UM_Application.GalleryService;
        var maxThumbnailSize = 1024;
        gallery.PickImage(maxThumbnailSize, result => {
            if (result.IsSucceeded) {
                UM_Media media = result.Media;
                Texture2D image = media.Thumbnail;
                Debug.Log("Thumbnail width: " + image.width + " / height: " + image.height);
                Debug.Log("media.Type: " + media.Type);
                Debug.Log("media.Path: " + media.Path);
            } else {
                Debug.Log("Failed to pick an image: " + result.Error.FullMessage);
            }
        });
    }

    private void PickVideo() {
        var gallery = UM_Application.GalleryService;
        var maxThumbnailSize = 1024;
        gallery.PickVideo(maxThumbnailSize, result => {
            if (result.IsSucceeded) {
                UM_Media media = result.Media;
                Texture2D image = media.Thumbnail;
                Debug.Log("Thumbnail width: " + image.width + " / height: " + image.height);
                Debug.Log("media.Type: " + media.Type);
                Debug.Log("media.Path: " + media.Path);

            } else {
                Debug.Log("Failed to pick an image: " + result.Error.FullMessage);
            }
        });
    }
}
