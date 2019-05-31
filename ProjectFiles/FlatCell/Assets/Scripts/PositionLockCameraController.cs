using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class PositionLockCameraController : AbstractCameraController
    {
        // Positions to draw on the HUD.
        private Vector3 Center = new Vector3(0, 85, 0);
        private Vector3 Top = new Vector3(0, 85, 5);
        private Vector3 Bottom = new Vector3(0, 85, -5);
        private Vector3 Right = new Vector3(5, 85, 0);
        private Vector3 Left = new Vector3(-5, 85, 0);

        private Camera ManagedCamera;
        private LineRenderer CameraLineRenderer;

        private void Awake()
        {
            ManagedCamera = gameObject.GetComponent<Camera>();
            CameraLineRenderer = gameObject.GetComponent<LineRenderer>();

            // Move camera to player location.
            var targetPosition = Target.transform.position;
            var cameraPosition = ManagedCamera.transform.position;
            cameraPosition.x = targetPosition.x;
            cameraPosition.z = targetPosition.z;
            ManagedCamera.transform.position = cameraPosition;
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            var targetPosition = Target.transform.position;
            var cameraPosition = ManagedCamera.transform.position;

            cameraPosition.x = targetPosition.x;
            cameraPosition.z = targetPosition.z;

            ManagedCamera.transform.position = cameraPosition;

            if (DrawLogic)
            {
                CameraLineRenderer.enabled = true;
                DrawCameraLogic();
            }
            else
            {
                CameraLineRenderer.enabled = false;
            }
        }

        public override void DrawCameraLogic()
        {
            CameraLineRenderer.positionCount = 8;
            CameraLineRenderer.useWorldSpace = false;
            CameraLineRenderer.SetPosition(0, Center);
            CameraLineRenderer.SetPosition(1, Top);
            CameraLineRenderer.SetPosition(2, Center);
            CameraLineRenderer.SetPosition(3, Bottom);
            CameraLineRenderer.SetPosition(4, Center);
            CameraLineRenderer.SetPosition(5, Right);
            CameraLineRenderer.SetPosition(6, Center);
            CameraLineRenderer.SetPosition(7, Left);
        }
    }
}
