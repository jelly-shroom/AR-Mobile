using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PostConfirmation : MonoBehaviour
{
    [SerializeField] RawImage imageDisplay; // UI element for displaying images
    [SerializeField] VideoPlayer videoPlayer; // Component for playing videos

    void OnEnable()
    {
        string path = MediaHandler.mediaPath;

        if (!string.IsNullOrEmpty(path))
        {
            NativeGallery.MediaType mediaType = NativeGallery.GetMediaTypeOfFile(path);

            if (mediaType == NativeGallery.MediaType.Image)
            {
                StartCoroutine(LoadImage(path));
            }
            else if (mediaType == NativeGallery.MediaType.Video)
            {
                LoadVideo(path);
            }
        }
    }

    private IEnumerator LoadImage(string path)
    {
        Texture2D texture = NativeGallery.LoadImageAtPath(path);
        if (texture != null)
        {
            imageDisplay.texture = texture;
            imageDisplay.gameObject.SetActive(true);
            videoPlayer.gameObject.SetActive(false);
            // Position imageDisplay in world space if needed
        }
        yield return null;
    }

    private void LoadVideo(string path)
    {
        videoPlayer.url = path;
        videoPlayer.Play();
        videoPlayer.gameObject.SetActive(true);
        imageDisplay.gameObject.SetActive(false);
        // Position videoPlayer's RenderTexture in world space if needed
    }
}