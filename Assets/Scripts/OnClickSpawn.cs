using UnityEngine;
using UnityEngine.InputSystem;

public class OnClickSpawn : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SpawnMedia spawnMedia; // Reference to your SpawnMedia script

    private InputAction clickAction;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Initialize the input action
        clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        clickAction.performed += ctx => OnClick();
        clickAction.Enable();
    }

    private void OnDestroy()
    {
        clickAction.Disable();
    }

    private void OnClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 spawnPoint = hit.point;
            Vector3 spawnNormal = hit.normal;

            // Call TrySpawnObject from your SpawnMedia script
            spawnMedia.TrySpawnObject(spawnPoint, spawnNormal);
        }
    }
}