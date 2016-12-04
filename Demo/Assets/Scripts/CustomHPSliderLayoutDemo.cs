using UnityEngine;

public class CustomHPSliderLayoutDemo : MonoBehaviour
{
    public float value1;
    public float value2;
    public float value3;
    public GUIStyle controlStyle;

    void OnGUI()
    {
        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        value1 = CustomHPSliderLayout(value1, controlStyle, GUILayout.Width(500f));
        //value1 = CustomHPSliderLayout(value1, controlStyle, GUILayout.MinWidth(500f));
        //value1 = CustomHPSliderLayout(value1, controlStyle, GUILayout.MaxWidth(500f));
        GUILayout.EndHorizontal();

        value2 = CustomHPSliderLayout(value2, controlStyle);
        value3 = CustomHPSliderLayout(value3, controlStyle);

        GUILayout.BeginVertical();
        GUILayout.Button("Short Button", GUILayout.ExpandWidth(false));
        GUILayout.Button("Very very long Button");
        GUILayout.EndVertical();
    }

    public static float CustomHPSliderLayout(float value, GUIStyle style, params GUILayoutOption[] options)
    {
        // 通过 EventType.Layout 记录所用风格绘制所需的矩形
        // 其他事件的时候，就会返回所记录的矩形
        Rect controlRect = GUILayoutUtility.GetRect(GUIContent.none, style, options);

        return CustomHPSliderDemo.CustomHPSlider(controlRect, value, style);
    }
}
