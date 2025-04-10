using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource 컴포넌트 참조 가져오기
        audioSource = GetComponent<AudioSource>();

        // 씬 로드 이벤트에 함수 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // MainScene으로 전환되면 음악 중지
        if (scene.name == "MainScene")
        {
            audioSource.Stop();
        }
    }

    void OnDestroy()
    {
        // 이벤트 등록 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}