using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class PushBoxCamera : AbstractCameraController
    {
        [SerializeField] public Vector3 TopLeft;
        [SerializeField] public Vector3 BottomRight;
        private Camera ManagedCamera;
        private LineRenderer CameraLineRenderer;

        private void Awake()
        {
            ManagedCamera = gameObject.GetComponent<Camera>();
            CameraLineRenderer = gameObject.GetComponent<LineRenderer>();
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            var targetPosition = Target.transform.position;
            var cameraPosition = ManagedCamera.transform.position;
            if (targetPosition.y >= cameraPosition.y + TopLeft.y)
            {
                cameraPosition = new Vector3(cameraPosition.x, cameraPosition.z, targetPosition.y - TopLeft.y);
            }
            if (targetPosition.y <= cameraPosition.y + BottomRight.y)
            {
                cameraPosition = new Vector3(cameraPosition.x, cameraPosition.z, targetPosition.y - BottomRight.y);
            }
            if (targetPosition.x >= cameraPosition.x + BottomRight.x)
            {
                cameraPosition = new Vector3(targetPosition.x - BottomRight.x, cameraPosition.z, cameraPosition.y);
            }
            if (targetPosition.x <= cameraPosition.x + TopLeft.x)
            {
                cameraPosition = new Vector3(targetPosition.x - TopLeft.x, cameraPosition.z, cameraPosition.y);
            }

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
            CameraLineRenderer.positionCount = 5;
            CameraLineRenderer.useWorldSpace = false;
            CameraLineRenderer.SetPosition(0, new Vector3(TopLeft.x, TopLeft.z, TopLeft.y));
            CameraLineRenderer.SetPosition(1, new Vector3(BottomRight.x, TopLeft.z, TopLeft.y));
            CameraLineRenderer.SetPosition(2, new Vector3(BottomRight.x, BottomRight.z, BottomRight.y));
            CameraLineRenderer.SetPosition(3, new Vector3(TopLeft.x, BottomRight.z, BottomRight.y));
            CameraLineRenderer.SetPosition(4, new Vector3(TopLeft.x, TopLeft.z, TopLeft.y));
        }
    }
}
