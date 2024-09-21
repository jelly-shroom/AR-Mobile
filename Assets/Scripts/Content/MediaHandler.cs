using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MediaHandler : MonoBehaviour
{
    public void PickMediaFromGallery()
    {
        if (NativeGallery.CanSelectMultipleMediaTypesFromGallery())
        {
            NativeGallery.Permission permission = NativeGallery.GetMixedMediaFromGallery((path) =>
            {
                Debug.Log("Media path: " + path);
                if (path != null)
                {
                    // Determine if user has picked an image, video or neither of these
                    switch (NativeGallery.GetMediaTypeOfFile(path))
                    {
                        case NativeGallery.MediaType.Image: Debug.Log("Picked image"); break;
                        case NativeGallery.MediaType.Video: Debug.Log("Picked video"); break;
                        default: Debug.Log("Probably picked something else"); break;
                    }
                }
            }, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select an image or video");

            Debug.Log("Permission result: " + permission);
        }
    }

    public void CapturePhoto()
    {
        // Use NatDevice or another plugin to capture a photo
        // Example with hypothetical API:
        // NatDevice.CapturePhoto((Texture2D photo) => {
        //     // Handle captured photo
        // });
    }

    public void CaptureVideo()
    {
        // Use NatDevice or another plugin to capture video
        // Example with hypothetical API:
        // NatDevice.CaptureVideo((string videoPath) => {
        //     // Handle captured video
        // });
    }
}