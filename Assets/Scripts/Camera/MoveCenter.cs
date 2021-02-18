using UnityEngine;

namespace CameraWork
{
    public class MoveCenter : MonoBehaviour
    {
        public static  MoveCenter CamScript;

        [SerializeField] private float tolerance=0.2f;

        #region MonoBehaviour
        private void Start()
        {
            CamScript = this;
        }
        #endregion

        #region CameraSettings
        public void SetCam(Vector3 targetPos)
        {
            transform.position = targetPos;

            UnityEngine.Camera.main.orthographicSize = ((720 * (16f / 9f) / 2) / 100) + tolerance;
 
            UnityEngine.Camera.main.aspect = 9f / 16f;
        }
        #endregion
    
    
    
    }
}
