using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class HandlePostTexture : MonoBehaviour
{
    [SerializeField] private RawImage imageDisplay;
    [SerializeField] private VideoPlayer videoPlayer;
    private AspectRatioFitter parentAspectRatioFitter;

    void Awake()
    {
        // Get the AspectRatioFitter component attached to the parent of RawImage
        parentAspectRatioFitter = imageDisplay.transform.parent.GetComponent<AspectRatioFitter>();
        if (parentAspectRatioFitter == null)
        {
            Debug.LogError("AspectRatioFitter component not found on parent GameObject.");
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

            // Set aspect ratio for image
            SetAspectRatios((float)texture.width / texture.height);
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
        // videoPlayer.Play();
        imageDisplay.texture = videoPlayer.targetTexture;
        imageDisplay.gameObject.SetActive(true);
        videoPlayer.enabled = true;

        // Assuming you have access to the video's aspect ratio
        float videoAspectRatio = (float)videoPlayer.clip.width / videoPlayer.clip.height;

        // Set aspect ratio for video
        SetAspectRatios(videoAspectRatio);
    }

    private void SetAspectRatios(float aspectRatio)
    {
        // Set the aspect ratio of the parent with mask
        if (parentAspectRatioFitter != null)
        {
            parentAspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            parentAspectRatioFitter.aspectRatio = aspectRatio;
        }
    }
}