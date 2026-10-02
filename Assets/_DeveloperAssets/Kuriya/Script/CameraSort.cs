using UnityEngine;

public class CameraSort : MonoBehaviour
{
    void Start()
    {
        Camera cam = GetComponent<Camera>();

        //これを使うとカメラの奥行で自動ソートしてくれるらしい
        cam.transparencySortMode = TransparencySortMode.CustomAxis;
        cam.transparencySortAxis = new Vector3(0, 0, 1);
    }
}