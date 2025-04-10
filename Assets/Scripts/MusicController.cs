using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource ������Ʈ ���� ��������
        audioSource = GetComponent<AudioSource>();

        // �� �ε� �̺�Ʈ�� �Լ� ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // MainScene���� ��ȯ�Ǹ� ���� ����
        if (scene.name == "MainScene")
        {
            audioSource.Stop();
        }
    }

    void OnDestroy()
    {
        // �̺�Ʈ ��� ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}