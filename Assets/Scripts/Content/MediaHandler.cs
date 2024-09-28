using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MediaHandler : MonoBehaviour
{
    public static string mediaPath;
    [SerializeField] GameObject postConfirmationPanel; // Reference to the post confirmation UI panel
    [SerializeField] GameObject mediaOptionsPanel; // Reference to the media options UI panel
    [SerializeField] GameObject postPreview;

    public delegate void ImageSelectedEvent(string path);
    public static event ImageSelectedEvent OnImageSelected;

    public void PickMediaFromGallery()
    {
        if (NativeGallery.CanSelectMultipleMediaTypesFromGallery())
        {
            NativeGallery.Permission permission = NativeGallery.GetMixedMediaFromGallery((path) =>
            {
                Debug.Log("Media path: " + path);
                if (path != null)
                {
                    mediaPath = path;
                    GoToPostScreen();

                    // Trigger the event
                    OnImageSelected?.Invoke(path);
                }
            }, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select an image or video");

            Debug.Log("Permission result: " + permission);
        }
    }

    public void GoToPostScreen()
    {
        // Activate the post confirmation panel
        postConfirmationPanel.SetActive(true);
        mediaOptionsPanel.SetActive(false);
    }

    public void CapturePhoto()
    {
        // Implement photo capture logic here
    }

    public void CaptureVideo()
    {
        // Implement video capture logic here
    }
}