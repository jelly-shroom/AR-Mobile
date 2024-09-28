using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

public class SpawnMedia : MonoBehaviour
{
    [SerializeField] private Camera m_CameraToFace;
    [SerializeField] private List<GameObject> m_ObjectPrefabs = new List<GameObject>();
    [SerializeField] private int m_SpawnOptionIndex = -1;
    [SerializeField] private bool m_OnlySpawnInView = true;
    [SerializeField] private float m_ViewportPeriphery = 0.15f;
    [SerializeField] private bool m_ApplyRandomAngleAtSpawn = true;
    [SerializeField] private float m_SpawnAngleRange = 45f;
    [SerializeField] private bool m_SpawnAsChildren;
    [SerializeField] private bool isSpawnOptionRandomized;


    private Texture2D loadedTexture;

    void Awake()
    {
        EnsureFacingCamera();
    }

    void EnsureFacingCamera()
    {
        if (m_CameraToFace == null)
            m_CameraToFace = Camera.main;
    }

    public void LoadTexture()
    {
        string path = MediaHandler.mediaPath;
        loadedTexture = NativeGallery.LoadImageAtPath(path);

        if (loadedTexture == null)
        {
            Debug.LogError("Failed to load image");
        }
    }

    public bool TrySpawnObject(Vector3 spawnPoint, Vector3 spawnNormal)
    {
        if (m_OnlySpawnInView)
        {
            var inViewMin = m_ViewportPeriphery;
            var inViewMax = 1f - m_ViewportPeriphery;
            var pointInViewportSpace = m_CameraToFace.WorldToViewportPoint(spawnPoint);
            if (pointInViewportSpace.z < 0f || pointInViewportSpace.x > inViewMax || pointInViewportSpace.x < inViewMin ||
                pointInViewportSpace.y > inViewMax || pointInViewportSpace.y < inViewMin)
                return false;
        }

        var objectIndex = isSpawnOptionRandomized ? Random.Range(0, m_ObjectPrefabs.Count) : m_SpawnOptionIndex;
        var newObject = Instantiate(m_ObjectPrefabs[objectIndex]);

        if (m_SpawnAsChildren)
            newObject.transform.parent = transform;

        newObject.transform.position = spawnPoint;

        EnsureFacingCamera();
        var facePosition = m_CameraToFace.transform.position;
        var forward = facePosition - spawnPoint;

        BurstMathUtility.ProjectOnPlane(forward, spawnNormal, out var projectedForward);
        newObject.transform.rotation = Quaternion.LookRotation(projectedForward, spawnNormal);

        if (m_ApplyRandomAngleAtSpawn)
        {
            var randomRotation = Random.Range(-m_SpawnAngleRange, m_SpawnAngleRange);
            newObject.transform.Rotate(Vector3.up, randomRotation);
        }

        ModifySpawnedObject(newObject);

        return true;
    }

    private void ModifySpawnedObject(GameObject spawnedObject)
    {
        if (spawnedObject == null || loadedTexture == null) return;

        MeshRenderer meshRenderer = spawnedObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null && meshRenderer.material != null)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetTexture("_MainTex", loadedTexture);
            meshRenderer.SetPropertyBlock(propertyBlock);

            float aspectRatio = (float)loadedTexture.width / loadedTexture.height;
            Vector3 newScale = spawnedObject.transform.localScale;

            if (aspectRatio > 1) // Wider than tall
                newScale.x = newScale.y * aspectRatio;
            else // Taller than wide or square
                newScale.y = newScale.x / aspectRatio;

            spawnedObject.transform.localScale = newScale;
            Debug.Log("Prefab scale adjusted to maintain aspect ratio");
        }
    }
}