using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PrefabModifier : MonoBehaviour
{
    [SerializeField] private SpawnMedia objectSpawner; // Reference to your ObjectSpawner
    private Texture2D loadedTexture;

    void Start()
    {
        // Load your texture here
        string path = MediaHandler.mediaPath;
        loadedTexture = NativeGallery.LoadImageAtPath(path);

        if (loadedTexture == null)
        {
            Debug.LogError("Failed to load image");
            return;
        }

        // Subscribe to the objectSpawned event
        objectSpawner.objectSpawned += ModifySpawnedObject;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        objectSpawner.objectSpawned -= ModifySpawnedObject;
    }

    public void ModifySpawnedObject(GameObject spawnedObject)
    {
        if (spawnedObject == null || loadedTexture == null) return;

        MeshRenderer meshRenderer = spawnedObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null && meshRenderer.material != null)
        {
            meshRenderer.material.mainTexture = loadedTexture;
            Debug.Log("Texture applied to spawned object");

            // Adjust scale to maintain aspect ratio
            float aspectRatio = (float)loadedTexture.width / loadedTexture.height;
            Vector3 newScale = spawnedObject.transform.localScale;

            if (aspectRatio > 1) // Wider than tall
            {
                newScale.x = newScale.y * aspectRatio;
            }
            else // Taller than wide or square
            {
                newScale.y = newScale.x / aspectRatio;
            }

            spawnedObject.transform.localScale = newScale;
            Debug.Log("Prefab scale adjusted to maintain aspect ratio");
        }
    }
}