using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    [SerializeField] float targetWidth = 17.78f;
    [SerializeField] float minHeight = 10f;

    Camera cam;
    int lastW, lastH;

    void Awake() { cam = GetComponent<Camera>(); Apply(); }

    void Update()
    {
        if (Screen.width != lastW || Screen.height != lastH) Apply();
    }

    void Apply()
    {
        lastW = Screen.width; lastH = Screen.height;
        float aspect = (float)Screen.width / Screen.height;
        float size = targetWidth / aspect / 2f;
        cam.orthographicSize = Mathf.Max(size, minHeight / 2f);
    }
}