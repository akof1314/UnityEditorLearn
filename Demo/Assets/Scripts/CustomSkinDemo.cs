using UnityEngine;

public class CustomSkinDemo : MonoBehaviour
{
    public GUISkin skin;
    private GUISkin m_Skin;

    void OnGUI()
    {
        GUI.Button(new Rect(0, 0, 100f, 80f), new GUIContent("New Button"), skin.button);

        // 查找自定义样式
        GUIStyle boxStyle = skin.FindStyle("BoxCustom");
        GUI.Button(new Rect(120f, 0, 100f, 80f), new GUIContent("New Button2"), boxStyle);

#if UNITY_EDITOR
        if (!m_Skin)
        {
            // 从"Assets/Editor Default Resources/"目录获取资源
            // 实质调用AssetDatabase.LoadAssetAtPath，注意带后缀名
            m_Skin = (GUISkin)UnityEditor.EditorGUIUtility.LoadRequired("CustomSkin2.guiskin");
        }
        GUI.Button(new Rect(Screen.width - 100f, 0, 100f, 80f), new GUIContent("New Button3"), m_Skin.button);
#endif
    }
}
