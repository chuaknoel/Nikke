using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���� ����� ����� ó���ϴ� Ŭ����
/// </summary>
public class RetryButton : MonoBehaviour
{
    /// <summary>
    /// ���� ���� �ٽ� �ε��Ͽ� ������ ������մϴ�.
    /// </summary>
    public void Retry()
    {
        // ���� ���� �ٽ� �ε��մϴ�.
        SceneManager.LoadScene("MainScene");
    }
}