using UnityEngine;

public class FlashingButtonDemo : MonoBehaviour
{
    void OnGUI()
    {
        FlashingButton(new Rect(0, 0, 100f, 80f), new GUIContent("Flashing"), GUI.skin.button);
    }

    public class FlashingButtonInfo
    {
        public float mouseDownAt;

        // 存储按下时刻的时间
        public void MouseDownNow()
        {
            mouseDownAt = Time.timeSinceLevelLoad;
        }

        // 判断控件是否可以闪烁
        public bool IsFlashing(int controlID)
        {
            if (GUIUtility.hotControl != controlID)
            {
                return false;
            }

            // 至少按下超过2秒
            float elapsedTime = Time.timeSinceLevelLoad - mouseDownAt;
            if (elapsedTime < 2f)
            {
                return false;
            }

            // 每0.1秒闪烁一次
            return (int)((elapsedTime - 2f) / 0.1f) % 2 == 0;
        }
    }

    public static bool FlashingButton(Rect controlRect, GUIContent content, GUIStyle style)
    {
        int controlID = GUIUtility.GetControlID(FocusType.Native);

        // 获取或创建状态对象
        FlashingButtonInfo state =
            (FlashingButtonInfo)GUIUtility.GetStateObject(typeof(FlashingButtonInfo), controlID);

        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.Repaint:
                {
                    GUI.color = state.IsFlashing(controlID) ? Color.red : Color.white;
                    style.Draw(controlRect, content, controlID);
                }
                break;
            case EventType.MouseDown:
                {
                    if (controlRect.Contains(Event.current.mousePosition) && GUIUtility.hotControl == 0 && Event.current.button == 0)
                    {
                        GUIUtility.hotControl = controlID;
                        state.MouseDownNow();
                    }
                }
                break;
            case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                    }
                }
                break;
        }

        return GUIUtility.hotControl == controlID;
    }
}
