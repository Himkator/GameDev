using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    public float targetWidth = 20f;
    public float targetHeight = 11.25f;

    Camera cam;
    int lastW, lastH;

    void Awake() { cam = GetComponent<Camera>(); Apply(); }
    void Update() { if (Screen.width != lastW || Screen.height != lastH) Apply(); }

    void Apply()
    {
        lastW = Screen.width;
        lastH = Screen.height;
        float aspect = (float)Screen.width / Screen.height;
        float sizeByHeight = targetHeight / 2f;
        float sizeByWidth = targetWidth / aspect / 2f;
        cam.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
    }
}