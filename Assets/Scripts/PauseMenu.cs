using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // �Ͻ����� �޴� UI (Canvas ��)

    public void Pause()
    {
        Time.timeScale = 0f; // ���� �Ͻ�����
        pauseMenuUI.SetActive(true); // �޴� Ȱ��ȭ
    }

    public void Resume()
    {
        Time.timeScale = 1f; // ���� �簳
        pauseMenuUI.SetActive(false); // �޴� ��Ȱ��ȭ
    }
}

