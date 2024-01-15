using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Common;

public class FaceCameraProxy : MonoBehaviour
{
    private Camera? _targetCamera;

    public Transform? targetToRotate;
    public bool copyX;
    public bool copyY;
    public bool copyZ;

    private void Update()
    {
        if (targetToRotate is null)
            return;
        
        if (_targetCamera is null)
            _targetCamera = FindBestCamera();

        if (_targetCamera is null)
            return;
        
        transform.LookAt(_targetCamera.transform);
        transform.Rotate(0, 180, 0, Space.Self);
    }

    private void LateUpdate()
    {
        if (targetToRotate is null)
            return;

        var rotation = transform.eulerAngles;
        var targetRotation = targetToRotate.eulerAngles;

        if (!copyX)
            rotation = new Vector3(targetRotation.x, rotation.y, rotation.z);

        if (!copyY)
            rotation = new Vector3(rotation.x, targetRotation.y, rotation.z);

        if (!copyZ)
            rotation = new Vector3(rotation.x, rotation.y, targetRotation.z);

        targetToRotate.eulerAngles = rotation;
    }

    private Camera? FindBestCamera()
    {
        if (StartOfRound.Instance.localPlayerController is not null)
            return StartOfRound.Instance.localPlayerController.gameplayCamera;
        
        if (Camera.current is not null)
            return Camera.current;

        return null;
    }
}