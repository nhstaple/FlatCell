using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class PositionLockLerpCameraController : AbstractCameraController
    {
        [SerializeField] private float LerpDuration; 
        private Camera ManagedCamera;
        private LineRenderer CameraLineRenderer;
        private Vector3 previousLoc;
        private float timeCounter = 0.0f;

        private void Awake()
        {
            this.ManagedCamera = this.gameObject.GetComponent<Camera>();
            this.CameraLineRenderer = this.gameObject.GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            // Move camera to player position.
            var targetPosition = this.Target.transform.position;
            var cameraPosition = this.ManagedCamera.transform.position;
            cameraPosition.x = targetPosition.x;
            cameraPosition.y = targetPosition.y;
            this.ManagedCamera.transform.position = cameraPosition;
            previousLoc = this.Target.transform.position;
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            var cameraPosition = this.ManagedCamera.transform.position;

            if(this.Target.transform.hasChanged)
            {
                previousLoc = this.Target.transform.position;
                timeCounter = 0.0f;
            }

            timeCounter += Time.deltaTime;

            if(timeCounter < LerpDuration)
            {
                timeCounter += Time.deltaTime;
            }

            var adjust = Vector2.Lerp(new Vector2(cameraPosition.x, cameraPosition.y), new Vector2(previousLoc.x, previousLoc.y), timeCounter/ LerpDuration);
            cameraPosition = new Vector3(adjust.x, adjust.y, cameraPosition.z);

            this.ManagedCamera.transform.position = cameraPosition;

            if (this.DrawLogic)
            {
                this.CameraLineRenderer.enabled = true;
                this.DrawCameraLogic();
            }
            else
            {
                this.CameraLineRenderer.enabled = false;
            }
        }

        public override void DrawCameraLogic()
        {
            this.CameraLineRenderer.positionCount = 8;
            this.CameraLineRenderer.useWorldSpace = false;
            this.CameraLineRenderer.SetPosition(0, new Vector3(0, 0, 85));
            this.CameraLineRenderer.SetPosition(1, new Vector3(5, 0, 85));
            this.CameraLineRenderer.SetPosition(2, new Vector3(0, 0, 85));
            this.CameraLineRenderer.SetPosition(3, new Vector3(-5, 0, 85));
            this.CameraLineRenderer.SetPosition(4, new Vector3(0, 0, 85));
            this.CameraLineRenderer.SetPosition(5, new Vector3(0, 5, 85));
            this.CameraLineRenderer.SetPosition(6, new Vector3(0, 0, 85));
            this.CameraLineRenderer.SetPosition(7, new Vector3(0, -5, 85));
        }
    }
}
