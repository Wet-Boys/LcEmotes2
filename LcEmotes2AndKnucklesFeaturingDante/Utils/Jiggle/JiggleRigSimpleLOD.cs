#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Emotes2JigglePhysics
{
    public class JiggleRigSimpleLOD : JiggleRigLOD {

        [Tooltip("Distance to disable the jiggle rig.")]
        [SerializeField] float distance = 20f;
        [Tooltip("Distance past distance from which it blends out rather than instantly disabling.")]
        [SerializeField] float blend = 5f;
        
        private static Camera currentCamera;


        protected virtual bool TryGetCamera(out Camera camera) {
            #if UNITY_EDITOR
            if (EditorWindow.focusedWindow is SceneView view) {
                camera = view.camera;
                return camera;
            }
            #endif
            if (!currentCamera || !currentCamera.CompareTag("MainCamera")) {
                currentCamera = Camera.main;
            }
            camera = currentCamera;
            return currentCamera;
        }
        protected override bool CheckActive() {
            if (!TryGetCamera(out Camera camera)) {
                return false;
            }

            var position = transform.position;
            var cameraDistance = Vector3.Distance(camera.transform.position, position);
            var currentBlend = (cameraDistance - distance + blend) / blend;
            currentBlend = Mathf.Clamp01(1f-currentBlend);
            foreach (var jiggle in jiggles) {
                jiggle.blend = currentBlend;
            }
            return cameraDistance < distance;
        }

    }

}