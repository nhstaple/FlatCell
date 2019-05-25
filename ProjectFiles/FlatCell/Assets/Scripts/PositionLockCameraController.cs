using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class PositionLockCameraController : AbstractCameraController
    {
        // Positions to draw on the HUD.
        private Vector3 Center = new Vector3(0, 0, 85);
        private Vector3 Top    = new Vector3(0, 5, 85);
        private Vector3 Bottom = new Vector3(0, -5, 85); 
        private Vector3 Right  = new Vector3(5, 0, 85);
        private Vector3 Left   = new Vector3(-5, 0, 85);

        private Camera ManagedCamera;
        private LineRenderer CameraLineRenderer;

        private void Awake()
        {
            this.ManagedCamera = this.gameObject.GetComponent<Camera>();
            this.CameraLineRenderer = this.gameObject.GetComponent<LineRenderer>();

            // Move camera to player location.
            var targetPosition = this.Target.transform.position;
            var cameraPosition = this.ManagedCamera.transform.position;
            cameraPosition.x = targetPosition.x;
            cameraPosition.y = targetPosition.y;
            this.ManagedCamera.transform.position = cameraPosition;
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            var targetPosition = this.Target.transform.position;
            var cameraPosition = this.ManagedCamera.transform.position;

            cameraPosition.x = targetPosition.x;
            cameraPosition.y = targetPosition.y;

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
            this.CameraLineRenderer.SetPosition(0, Center);
            this.CameraLineRenderer.SetPosition(1, Top);
            this.CameraLineRenderer.SetPosition(2, Center);
            this.CameraLineRenderer.SetPosition(3, Bottom);
            this.CameraLineRenderer.SetPosition(4, Center);
            this.CameraLineRenderer.SetPosition(5, Right);
            this.CameraLineRenderer.SetPosition(6, Center);
            this.CameraLineRenderer.SetPosition(7, Left);
        }
    }
}
