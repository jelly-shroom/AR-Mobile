using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PostConfirmation : MonoBehaviour
{
    [SerializeField] private RawImage imageDisplay;
    [SerializeField] private VideoPlayer videoPlayer;
    private AspectRatioFitter aspectRatioFitter;

    void Awake()
    {
        // Get the AspectRatioFitter component attached to the same GameObject as the RawImage
        aspectRatioFitter = imageDisplay.GetComponent<AspectRatioFitter>();
        if (aspectRatioFitter == null)
        {
            Debug.LogError("AspectRatioFitter component not found on RawImage GameObject.");
        }
    }

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
            Debug.Log("Image loaded successfully");

            imageDisplay.texture = texture;
            imageDisplay.gameObject.SetActive(true);
            videoPlayer.enabled = false;

            // Set the aspect ratio of the AspectRatioFitter to match the texture
            if (aspectRatioFitter != null)
            {
                aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
                aspectRatioFitter.aspectRatio = (float)texture.width / texture.height;
            }
        }
        else
        {
            Debug.LogError("Failed to load image");
        }
        yield return null;
    }

    private void LoadVideo(string path)
    {
        videoPlayer.url = path;
        videoPlayer.Play();
        imageDisplay.texture = videoPlayer.targetTexture;
        imageDisplay.gameObject.SetActive(true);
        videoPlayer.enabled = true;

        // Optionally, set aspect ratio for video as well if needed
        // Assuming you have a way to know or calculate video's aspect ratio
    }
}