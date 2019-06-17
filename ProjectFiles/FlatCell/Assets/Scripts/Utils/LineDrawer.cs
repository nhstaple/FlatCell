﻿using UnityEngine;
using Geo.Command;

namespace Utils.Debug
{
    // source
    // https://stackoverflow.com/questions/42819071/debug-drawline-not-showing-in-the-gameview
    // Used to draw debug line.
    public struct LineDrawer
    {
        private LineRenderer lineRenderer;
        private float lineSize;
        public IGeo parent;

        public LineDrawer(IGeo p, float lineSize = 0.2f)
        {
            parent = p;
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));
            lineRenderer.transform.SetParent(p.GetGameObject().transform);
            this.lineSize = lineSize;
        }

        private void Init(float lineSize = 0.2f)
        {
            if (lineRenderer == null)
            {
                GameObject lineObj = new GameObject("LineObj");
                lineRenderer = lineObj.AddComponent<LineRenderer>();
                //Particles/Additive
                lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

                this.lineSize = lineSize;
            }
        }

        //Draws lines through the provided vertices
        public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
        {
            if (lineRenderer == null)
            {
                Init(0.2f);
            }
            //Set color
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            //Set width
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;

            //Set line count which is 2
            lineRenderer.positionCount = 2;

            //Set the postion of both two lines
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }

        public void Destroy()
        {
            if (lineRenderer != null)
            {
                UnityEngine.Object.Destroy(lineRenderer.gameObject);
            }
        }
    }
}
