using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 재시작 기능을 처리하는 클래스
/// </summary>
public class RetryButton : MonoBehaviour
{
    /// <summary>
    /// 메인 씬을 다시 로드하여 게임을 재시작합니다.
    /// </summary>
    public void Retry()
    {
        // 메인 씬을 다시 로드합니다.
        SceneManager.LoadScene("MainScene");
    }
}