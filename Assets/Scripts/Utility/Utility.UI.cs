using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameFramework
{
public static partial class Utility
{
    public static class UI
    {
        /// <summary>
        /// 创建UI主画布，带UICamera
        /// </summary>
        /// <returns></returns>
        public static GameObject CreateCanvas(float depth = 1f)
        {
            GameObject ui = new GameObject("UICanvas");
            ui.transform.position = Vector3.zero;
            ui.transform.rotation = Quaternion.identity;
            ui.transform.localScale = Vector3.one;

            Canvas canvas = ui.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.gameObject.layer = LayerMask.NameToLayer("UI");

            CanvasScaler scaler = ui.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1136, 640);
            scaler.matchWidthOrHeight = 1f;
            scaler.referencePixelsPerUnit = 100;

            ui.AddComponent<GraphicRaycaster>();

            var evts = GameObject.FindObjectOfType<EventSystem>();
            if (evts == null) {
                GameObject ev = new GameObject("EventSystem");
                ev.AddComponent<EventSystem>();
                ev.AddComponent<StandaloneInputModule>();
            }

            GameObject camera = new GameObject("UICamera");
            camera.transform.SetParent(ui.transform, false);
            camera.transform.localPosition = new Vector3(0, 0, -100);
            camera.transform.localScale = Vector3.one;
            camera.transform.localRotation = Quaternion.identity;
            camera.layer = LayerMask.NameToLayer("UI");

            Camera cam = camera.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.Depth;
            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 3f;
            cam.cullingMask = 1 << LayerMask.NameToLayer("UI");
            cam.orthographic = true;
            cam.orthographicSize = 5;
            cam.depth = depth;
            cam.allowHDR = false;
            cam.allowMSAA = true;

            canvas.worldCamera = cam;

            return ui;
        }
    }
}
}