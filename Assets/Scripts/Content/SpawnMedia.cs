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


    void Awake()
    {
        EnsureFacingCamera();
    }

    void EnsureFacingCamera()
    {
        if (m_CameraToFace == null)
            m_CameraToFace = Camera.main;
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
        newObject.transform.localScale = newObject.transform.localScale.x * 0.1f * Vector3.one;

        BurstMathUtility.ProjectOnPlane(forward, spawnNormal, out var projectedForward);
        newObject.transform.rotation = Quaternion.LookRotation(projectedForward, spawnNormal);

        if (m_ApplyRandomAngleAtSpawn)
        {
            var randomRotation = Random.Range(-m_SpawnAngleRange, m_SpawnAngleRange);
            newObject.transform.Rotate(Vector3.up, randomRotation);
        }

        return true;
    }
}