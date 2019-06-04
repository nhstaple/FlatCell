using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class FrameAutoScrollCameraController : AbstractCameraController
    {
        [SerializeField] private Vector2 TopLeft;
        [SerializeField] private Vector2 BottomRight;
        [SerializeField] private float AutoScrollSpeed;

        private Camera ManagedCamera;
        private LineRenderer CameraLineRenderer;

        private void Awake()
        {
            this.ManagedCamera = this.gameObject.GetComponent<Camera>();
            this.CameraLineRenderer = this.gameObject.GetComponent<LineRenderer>();
        }

        private void Start()
        {
            var targetPosition = this.Target.transform.position;
            var cameraPosition = this.ManagedCamera.transform.position;
            targetPosition.x = cameraPosition.x;
            targetPosition.y = cameraPosition.y;
            this.Target.transform.SetPositionAndRotation(targetPosition, this.Target.transform.rotation);
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            var cameraPosition = this.ManagedCamera.transform.position;
            cameraPosition.x += Time.deltaTime * AutoScrollSpeed;   

            var playerVar = this.Target;
            var posView = this.ManagedCamera.WorldToViewportPoint(this.Target.transform.position);

            // Passed the left side of the box.
            if (playerVar.transform.position.x <= ( cameraPosition.x + TopLeft.x))
            {
                //Debug.Log("Edge on left side!");
                playerVar.transform.SetPositionAndRotation(new Vector3(cameraPosition.x + TopLeft.x, playerVar.transform.position.y, playerVar.transform.position.z), playerVar.transform.rotation);
            }
            // Passed the right side of the box.
            else if (playerVar.transform.position.x >= (cameraPosition.x + BottomRight.x))
            {
                //Debug.Log("Edge on right side!");
                playerVar.transform.SetPositionAndRotation(new Vector3(cameraPosition.x + BottomRight.x, playerVar.transform.position.y, playerVar.transform.position.z), playerVar.transform.rotation);
            }

            // Passed the top of the box.
            if (playerVar.transform.position.y >= (cameraPosition.y + TopLeft.y))
            {
                //Debug.Log("Edge on bottom side!");
                playerVar.transform.SetPositionAndRotation(new Vector3(playerVar.transform.position.x, cameraPosition.y + TopLeft.y, playerVar.transform.position.z), playerVar.transform.rotation);
            }
            // Passed the bottom of the box.
            else if (playerVar.transform.position.y <= (cameraPosition.y + BottomRight.y))
            {
                //Debug.Log("Edge on top side!");
                playerVar.transform.SetPositionAndRotation(new Vector3(playerVar.transform.position.x, cameraPosition.y + BottomRight.y, playerVar.transform.position.z), playerVar.transform.rotation);
            }

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
            this.CameraLineRenderer.positionCount = 5;
            this.CameraLineRenderer.useWorldSpace = false;
            this.CameraLineRenderer.SetPosition(0, new Vector3(TopLeft.x, TopLeft.y, 85));
            this.CameraLineRenderer.SetPosition(1, new Vector3(BottomRight.x, TopLeft.y, 85));
            this.CameraLineRenderer.SetPosition(2, new Vector3(BottomRight.x, BottomRight.y, 85));
            this.CameraLineRenderer.SetPosition(3, new Vector3(TopLeft.x, BottomRight.y, 85));
            this.CameraLineRenderer.SetPosition(4, new Vector3(TopLeft.x, TopLeft.y, 85));
        }
    }
}
