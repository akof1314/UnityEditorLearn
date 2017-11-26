using UnityEditor;
using UnityEngine;

public class SceneMouseWindow : EditorWindow
{
    [MenuItem("Tool/Window/Scene Mouse")]
    public static void ShowWindow()
    {
        var window = GetWindow<SceneMouseWindow>();
        window.Show();
    }

    void OnFocus()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
        Repaint();
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        if (capSphere != null)
        {
            DestroyImmediate(capSphere.gameObject);
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        // 当前屏幕坐标，左上角是（0，0）右下角（camera.pixelWidth，camera.pixelHeight）
        Vector2 mousePosition = Event.current.mousePosition;

        // Retina 屏幕需要拉伸值
        float mult = 1;
#if UNITY_5_4_OR_NEWER
        mult = EditorGUIUtility.pixelsPerPoint;
#endif

        // 转换成摄像机可接受的屏幕坐标，左下角是（0，0，0）右上角是（camera.pixelWidth，camera.pixelHeight，0）
        mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y * mult;
        mousePosition.x *= mult;

        // 近平面往里一些，才能看得到摄像机里的位置
        Vector3 fakePoint = mousePosition;
        fakePoint.z = 20;
        Vector3 point = sceneView.camera.ScreenToWorldPoint(fakePoint);

        Ray ray = sceneView.camera.ScreenPointToRay(mousePosition);
        MeshFilter[] componentsInChildren = GameObject.FindObjectsOfType<MeshFilter>();
        float num = float.PositiveInfinity;
        foreach (MeshFilter meshFilter in componentsInChildren)
        {
            Mesh sharedMesh = meshFilter.sharedMesh;
            RaycastHit hit;
            if (sharedMesh
                && RXLookingGlass.IntersectRayMesh(ray, sharedMesh, meshFilter.transform.localToWorldMatrix, out hit)
                && hit.distance < num)
            {
                point = hit.point;
                num = hit.distance;
            }
        }

        //Handles.SphereCap(0, point, Quaternion.identity, 2);
        SphereCapPos(point);

        // 刷新界面，才能让球一直跟随
        sceneView.Repaint();
        HandleUtility.Repaint();
    }

    private static Transform capSphere;

    private void SphereCapPos(Vector3 point)
    {
        if (capSphere == null)
        {
            GameObject go = GameObject.Find("[SphereCapPos]");
            if (go == null)
            {
                go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.name = "[SphereCapPos]";

                Collider collider = go.GetComponent<Collider>();
                DestroyImmediate(collider);

                Material mat = new Material(Shader.Find("Unlit/Color"));
                mat.SetColor("_Color", Color.cyan);
                mat.hideFlags = HideFlags.HideAndDontSave;

                Renderer renderer = go.GetComponent<Renderer>();
                renderer.sharedMaterial = mat;
            }

            go.hideFlags = HideFlags.HideAndDontSave;
            capSphere = go.transform;
            capSphere.rotation = Quaternion.identity;
            capSphere.localScale = Vector3.one * 0.5f;
        }
        capSphere.position = point;
    }
}
