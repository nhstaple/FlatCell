using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class PositionLockCameraController : AbstractCameraController
    {
        // Positions to draw on the HUD.
        public float cam_offset = 50;
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
            cameraPosition.y = targetPosition.y + cam_offset;
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
            cameraPosition.y = targetPosition.y + cam_offset;
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
            Vector3 Center = new Vector3(0, 0, cam_offset);
            Vector3 Top = new Vector3(0, 5, cam_offset);
            Vector3 Bottom = new Vector3(0, -5, cam_offset);
            Vector3 Right = new Vector3(5, 0, cam_offset);
            Vector3 Left = new Vector3(-5, 0, cam_offset);

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
