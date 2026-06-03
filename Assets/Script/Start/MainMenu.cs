using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // 开始游戏：加载游戏场景
    public void StartGame()
    {
        // "GameScene" 需要替换成你实际游戏场景的名称
        SceneManager.LoadScene("GameScenes");
    }

    public void Setting()
    {

    }

    // 退出游戏
    public void ExitGame()
    {
#if UNITY_EDITOR
        // 在 Unity 编辑器中停止运行
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 在打包后的游戏中退出程序
            Application.Quit();
#endif
    }
}