using EmotesAPI;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.MottekeSailorFukku
{
    [DefaultExecutionOrder(-10)]
    public class DanceFreeCam : MonoBehaviour
    {
        public static DanceFreeCam instance;
        public GameObject cameraObject;
        private bool _active = false;
        private Vector3 oldCameraPos;
        private Vector3 oldCameraRot;
        Vector2 movementVector;
        Vector2 lookVector;
        float xLookDirection = 0;
        float jump;
        float crouch;
        Transform xRot;
        public bool active
        {
            get => _active;
            set
            {
                _active = value;
                if (_active)
                {
                    oldCameraPos = cameraObject.transform.localPosition;
                    oldCameraRot = cameraObject.transform.localEulerAngles;
                    transform.localPosition = new Vector3(3.5f, 4, 15);
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                    CustomEmotesAPI.localMapper.playerController.thisPlayerModelLOD1.transform.parent.GetComponent<LODGroup>().enabled = false;
                }
                else
                {
                    cameraObject.transform.localPosition = oldCameraPos;
                    cameraObject.transform.localEulerAngles = oldCameraRot;
                    CustomEmotesAPI.localMapper.playerController.thisPlayerModelLOD1.transform.parent.GetComponent<LODGroup>().enabled = true;
                }
            }
        }
        void Start()
        {
            xRot = transform.GetChild(0);
            xRot.transform.localEulerAngles = new Vector3(90, 0, 0);
            instance = this;
        }
        void Update()
        {
            if (active)
            {
                movementVector = IngamePlayerSettings.Instance.playerInput.actions.FindAction("Move", false).ReadValue<Vector2>();

                jump = IngamePlayerSettings.Instance.playerInput.actions.FindAction("Jump", false).ReadValue<float>();
                crouch = IngamePlayerSettings.Instance.playerInput.actions.FindAction("Crouch", false).ReadValue<float>();
                if (jump != 0)
                {
                    transform.position += Vector3.up * Time.deltaTime;
                }
                if (crouch != 0)
                {
                    transform.position -= Vector3.up * Time.deltaTime;
                }
                transform.position += movementVector.y * Time.deltaTime * transform.forward;
                transform.position += movementVector.x * Time.deltaTime * transform.right;


                lookVector = CustomEmotesAPI.localMapper.playerController.playerActions.Movement.Look.ReadValue<Vector2>() * 0.008f * (float)IngamePlayerSettings.Instance.settings.lookSensitivity;
                if (!IngamePlayerSettings.Instance.settings.invertYAxis)
                {
                    lookVector.y *= -1f;
                }
                xLookDirection = Mathf.Clamp(xLookDirection + lookVector.y, -89, 89);
                xRot.localEulerAngles = new Vector3(xLookDirection, 0, 0);
                transform.localEulerAngles += new Vector3(0, lookVector.x, 0);
                if (transform.localPosition.magnitude > 20)
                {
                    transform.localPosition = transform.localPosition.normalized * 20;
                }
            }
        }
        void LateUpdate()
        {
            if (active)
            {
                cameraObject.transform.position = xRot.transform.position;
                cameraObject.transform.rotation = xRot.transform.rotation;
            }
        }
    }
}
