using UnityEngine;
using TMPro;   // 如果使用 TextMeshPro

public class UIVisibleCounter : MonoBehaviour
{
    public VisibleCounter counter;   // 你的统计脚本
    public TMP_Text displayText;             // UI 文本组件
    public float updateInterval = 0.2f;      // 刷新间隔（秒）

    private float timer;

    void Start()
    {
        // 自动查找统计脚本（如果未手动赋值）
        if (counter == null)
            counter = FindObjectOfType<VisibleCounter>();
        // 自动查找 UI 文本（如果未手动赋值）
        if (displayText == null)
            displayText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (Input.GetButtonDown("A_KEY"))
        {
            displayText.enabled = !displayText.enabled;  
        }
        ShowCount(displayText.enabled);
    }

    private void ShowCount(bool Key)
    {
        if (Key)
        {
            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                timer = 0f;
                UpdateCountDisplay();
            }
        }
    }

    void UpdateCountDisplay()
    {
        if (counter != null && displayText != null)
        {
            int visibleCount = counter.CountVisible();
            displayText.text = $"识别人数: {visibleCount}";
        }
    }
}