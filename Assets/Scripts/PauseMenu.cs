using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // 일시정지 메뉴 UI (Canvas 등)

    public void Pause()
    {
        Time.timeScale = 0f; // 게임 일시정지
        pauseMenuUI.SetActive(true); // 메뉴 활성화
    }

    public void Resume()
    {
        Time.timeScale = 1f; // 게임 재개
        pauseMenuUI.SetActive(false); // 메뉴 비활성화
    }
}

