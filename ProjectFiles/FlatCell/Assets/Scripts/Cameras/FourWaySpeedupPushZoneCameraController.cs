using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;
using Controller.Player;

namespace Obscura
{
    public class FourWaySpeedupPushZoneCameraController : AbstractCameraController
    {
        [SerializeField] private Vector2 TopLeft;
        [SerializeField] private Vector2 BottomRight;
        [SerializeField] private float PushRatio;

        private Vector3 playerPos;
        private Vector3 cameraPos;
        private Vector3 playerDir;

        private Camera ManagedCamera;
        private LineRenderer CameraLineRenderer;

        private void Awake()
        {
            this.ManagedCamera = this.gameObject.GetComponent<Camera>();
            this.CameraLineRenderer = this.gameObject.GetComponent<LineRenderer>();

        }

        private void Start()
        {
            // Move camera to player position.
            var targetPosition = this.Target.transform.position;
            var cameraPosition = this.ManagedCamera.transform.position;
            cameraPosition.x = targetPosition.x;
            cameraPosition.y = targetPosition.y;
            this.ManagedCamera.transform.position = cameraPosition;

            playerPos = this.Target.transform.position;
            cameraPos = this.ManagedCamera.transform.position;
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            IGeo player = (IGeo) GameObject.Find("Player").GetComponent<PlayerController>();
            playerPos = this.Target.transform.position;
            playerDir = player.GetMovementDirection();
            var playerSpeed = player.GetSpeed();
            bool hitWall = false;

            cameraPos = this.ManagedCamera.transform.position;
            if (playerPos.x >= cameraPos.x + BottomRight.x)
            {
                cameraPos.x = playerPos.x - BottomRight.x;
                //Debug.Log("Hit the right side");
                hitWall = true;
            }
            if (playerPos.x <= cameraPos.x + TopLeft.x)
            {
                cameraPos.x = playerPos.x - TopLeft.x;
                //Debug.Log("Hit the left side!");
                hitWall = true;
            }
            if (playerPos.y >= cameraPos.y + TopLeft.y)
            {
                if(!hitWall)
                {
                    if(PushRatio >= 1.0)
                    {
                        cameraPos.x -= playerSpeed * PushRatio * Time.deltaTime * playerDir.x;
                    } else 
                    {
                        cameraPos.x += playerSpeed * PushRatio * Time.deltaTime * playerDir.x;
                    }
                }
                cameraPos.y = playerPos.y - TopLeft.y;
                hitWall = true;
            }
            if (playerPos.y <= cameraPos.y + BottomRight.y)
            {
                if(!hitWall)
                {
                    if(PushRatio >= 1.0)
                    {
                        cameraPos.x -= playerSpeed * PushRatio * Time.deltaTime * playerDir.x;
                    } else 
                    {
                        cameraPos.x += playerSpeed * PushRatio * Time.deltaTime * playerDir.x;
                    }
                }
                cameraPos.y = playerPos.y - BottomRight.y;
                hitWall = true;
            }

            if(!hitWall)
            {
                if(PushRatio >= 1.0)
                {
                    cameraPos.x -= playerSpeed * PushRatio * Time.deltaTime * playerDir.x;
                } else 
                {
                    cameraPos.x += playerSpeed * PushRatio * Time.deltaTime * playerDir.x;
                }
            }

            this.ManagedCamera.transform.position = cameraPos;

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
