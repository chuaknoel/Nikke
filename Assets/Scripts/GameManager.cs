using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    static public GameManager instance;

    // UI 요소 참조
    public Text timeText;            // 남은 시간을 표시할 텍스트
    public GameObject clearPanel;    // 게임 클리어 시 표시할 패널
    public GameObject gameOverPanel; // 게임 오버 시 표시할 패널
    public Text bestScoreText;       // 최고 점수(시간)를 표시할 텍스트
    public Text cardNumText;         // 카드 매치 수를 표시할 텍스트

    // 게임 설정
    public float time = 0.0f;        // 남은 시간
    public int totalMatches;         // 승리에 필요한 총 매치 수

    // PlayerPrefs 키 이름
    public string timeKey = "BestTime"; // 최고 시간 저장용 키
    public string cardsKey = "Cards";   // 카드 수 저장용 키

    // 점수 추적
    public float bestScore;           // 최고(최저) 시간 점수
    public int Cards = 0;             // 현재 게임의 카드 총 개수
    private int cardScore = 0;        // 현재 게임에서 맞춘 카드 쌍 수
    private int minCardScore = 0;     // 이전 게임에서의 최고 카드 점수

    // 게임 상태
    private bool isGameOver = false;  // 게임 종료 여부
    private Card firstCard;           // 첫 번째로 선택된 카드
    private Card secondCard;          // 두 번째로 선택된 카드
    private bool isChecking = false;  // 현재 카드 쌍 체크 중인지 여부
    private int matchesFound = 0;     // 찾은 매치 수

    void Start()
    {
        // 싱글톤 설정
        if (instance == null)
        {
            instance = this;
        }

        // 게임 상태 초기화
        Time.timeScale = 1f;          // 게임 시간 정상 흐름 설정
        isGameOver = true;            // 게임 시작 전 상태로 설정

        // UI 초기화
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (clearPanel != null)
        {
            clearPanel.SetActive(false);
        }
    }

    void Update()
    {
        // 타이머 업데이트
        time -= Time.deltaTime;
        if (timeText != null)
        {
            timeText.text = time.ToString("N2"); // 소수점 두 자리까지 표시
        }

        // 게임 오버 조건 체크
        if (time <= 0f && matchesFound < totalMatches)
        {
            GameOver(); // 시간 초과 시 게임 오버
        }
    }

    // 현재 카드 체크 중인지 확인하는 메소드
    public bool IsCheckingCards()
    {
        return isChecking;
    }

    // 게임이 종료되었는지 확인하는 메소드 (시간 종료 또는 모든 매치 찾음)
    public bool IsGameOver()
    {
        return time <= 0 || matchesFound >= totalMatches;
    }

    // 카드가 뒤집혔을 때 호출되는 메소드
    public void CardFlipped(Card card)
    {
        if (firstCard == null)
        {
            // 첫 번째로 뒤집힌 카드
            firstCard = card;
        }
        else if (secondCard == null && firstCard != card)
        {
            // 두 번째로 뒤집힌 카드
            secondCard = card;

            // 매치 확인 프로세스 시작
            isChecking = true;
            StartCoroutine(CheckMatch());
        }
    }

    // 두 카드의 매치 여부를 확인하는 코루틴
    private IEnumerator CheckMatch()
    {
        // 플레이어가 카드를 볼 수 있도록 잠시 대기
        yield return new WaitForSeconds(1.0f);

        if (firstCard.GetIndex() == secondCard.GetIndex())
        {
            // 카드 매치 성공!
            cardScore++;
            matchesFound++;

            // 모든 매치를 찾았는지 확인
            if (matchesFound >= totalMatches)
            {
                Clear(); // 게임 클리어
            }
        }
        else
        {
            // 카드가 매치되지 않음, 두 카드 모두 뒤집기
            firstCard.ResetCard();
            secondCard.ResetCard();
        }

        // 다음 턴을 위해 리셋
        firstCard = null;
        secondCard = null;
        isChecking = false;
    }

    // 게임 오버 상태 처리
    private void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null)
        {
            ScoreUI();     // 시간 점수 업데이트
            CardnumUI();   // 카드 매치 점수 업데이트
            gameOverPanel.SetActive(true);
            Time.timeScale = 0.0f; // 게임 일시 정지
        }
    }

    // 게임 클리어(승리) 상태 처리
    private void Clear()
    {
        isGameOver = true;
        if (clearPanel != null)
        {
            // 스코어 업데이트 및 계산
            ScoreUI();     // 시간 점수 업데이트
            CardnumUI();   // 카드 매치 점수 업데이트
            clearPanel.SetActive(true);
            Time.timeScale = 0.0f; // 게임 일시 정지
        }
    }

    // 카드 매치 UI 업데이트 및 최고 점수 저장
    public void CardnumUI()
    {
        if (PlayerPrefs.HasKey(cardsKey))
        {
            minCardScore = PlayerPrefs.GetInt(cardsKey, cardScore);
            if (minCardScore < cardScore)
            {
                // 새로운 최고 기록 달성
                PlayerPrefs.SetInt(cardsKey, cardScore);
                cardNumText.text = Cards.ToString();
            }
            else
            {
                // 기존 최고 기록 유지
                cardNumText.text = minCardScore.ToString();
            }
        }
        else
        {
            // 첫 플레이 시 기록 저장
            PlayerPrefs.SetInt(timeKey, Cards);
            cardNumText.text = Cards.ToString();
        }
    }

    // 시간 점수 UI 업데이트 및 최고 점수 저장
    public void ScoreUI()
    {
        if (PlayerPrefs.HasKey(timeKey))
        {
            bestScore = PlayerPrefs.GetFloat(timeKey, time);

            if (bestScore > time)
            {
                // 새로운 최고 기록(더 빠른 시간) 달성
                PlayerPrefs.SetFloat(timeKey, time);
                bestScoreText.text = time.ToString("N2");
            }
            else
            {
                // 기존 최고 기록 유지
                bestScoreText.text = bestScore.ToString("N2");
            }
        }
        else
        {
            // 첫 플레이 시 기록 저장
            PlayerPrefs.SetFloat(timeKey, time);
            bestScoreText.text = time.ToString("N2");
        }
    }

    // 게임 재시작 메소드
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}