using UnityEngine;

public class CustomHPSliderDemo : MonoBehaviour
{
    public float value1;
    public float value2;
    public float value3;
    public GUIStyle controlStyle;

    void OnGUI()
    {
        value1 = CustomHPSlider(new Rect(0, 0, 500f, 20f), value1, controlStyle);
        value2 = CustomHPSlider(new Rect(0, 30f, 500f, 20f), value2, controlStyle);
        value3 = CustomHPSlider(new Rect(0, 60f, 500f, 20f), value3, controlStyle);
    }

    public static float CustomHPSlider(Rect controlRect, float value, GUIStyle style)
    {
        // 获取控件ID，以便响应事件
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        // 以ID过滤事件
        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.Repaint:
                {
                    // 通过插值计算当前值的像素宽度
                    int pixelWidth = (int)Mathf.Lerp(1, controlRect.width, value);

                    // 目标矩形
                    Rect targetRect = new Rect(controlRect) { width = pixelWidth };

                    // 红色到绿色之间的插值
                    GUI.color = Color.Lerp(Color.red, Color.green, value);

                    // 绘制纹理
                    GUI.DrawTexture(targetRect, style.normal.background);

                    // 重置颜色
                    GUI.color = Color.white;
                }
                break;
            case EventType.MouseDown:
                {
                    // 只有鼠标左键真正点到控件矩形区域的时候
                    if (controlRect.Contains(Event.current.mousePosition) && GUIUtility.hotControl == 0 && Event.current.button == 0)
                    {
                        GUIUtility.hotControl = controlID;
                    }
                }
                break;
            case EventType.MouseUp:
                {
                    // 释放
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                    }
                }
                break;
        }

        // 任何鼠标事件（按下、抬起、拖动）
        if (Event.current.isMouse && GUIUtility.hotControl == controlID)
        {
            // 计算水平方向相对距离
            float relativeX = Event.current.mousePosition.x - controlRect.x;

            // 除以控件宽度得到0和1之间的值
            value = Mathf.Clamp01(relativeX / controlRect.width);

            // ★ 标记GUI改变
            GUI.changed = true;

            // ★ 标记事件已被使用 EventType.Used，其他控件不再被响应
            Event.current.Use();
        }

        // 返回最终值
        return value;
    }
}
