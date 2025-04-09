using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    static public GameManager instance;

    // UI ��� ����
    public Text timeText;            // ���� �ð��� ǥ���� �ؽ�Ʈ
    public GameObject clearPanel;    // ���� Ŭ���� �� ǥ���� �г�
    public GameObject gameOverPanel; // ���� ���� �� ǥ���� �г�
    public Text bestScoreText;       // �ְ� ����(�ð�)�� ǥ���� �ؽ�Ʈ
    public Text cardNumText;         // ī�� ��ġ ���� ǥ���� �ؽ�Ʈ

    // ���� ����
    public float time = 0.0f;        // ���� �ð�
    public int totalMatches;         // �¸��� �ʿ��� �� ��ġ ��

    // PlayerPrefs Ű �̸�
    public string timeKey = "BestTime"; // �ְ� �ð� ����� Ű
    public string cardsKey = "Cards";   // ī�� �� ����� Ű

    // ���� ����
    public float bestScore;           // �ְ�(����) �ð� ����
    public int Cards = 0;             // ���� ������ ī�� �� ����
    private int cardScore = 0;        // ���� ���ӿ��� ���� ī�� �� ��
    private int minCardScore = 0;     // ���� ���ӿ����� �ְ� ī�� ����

    // ���� ����
    private bool isGameOver = false;  // ���� ���� ����
    private Card firstCard;           // ù ��°�� ���õ� ī��
    private Card secondCard;          // �� ��°�� ���õ� ī��
    private bool isChecking = false;  // ���� ī�� �� üũ ������ ����
    private int matchesFound = 0;     // ã�� ��ġ ��

    void Start()
    {
        // �̱��� ����
        if (instance == null)
        {
            instance = this;
        }

        // ���� ���� �ʱ�ȭ
        Time.timeScale = 1f;          // ���� �ð� ���� �帧 ����
        isGameOver = true;            // ���� ���� �� ���·� ����

        // UI �ʱ�ȭ
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
        // Ÿ�̸� ������Ʈ
        time -= Time.deltaTime;
        if (timeText != null)
        {
            timeText.text = time.ToString("N2"); // �Ҽ��� �� �ڸ����� ǥ��
        }

        // ���� ���� ���� üũ
        if (time <= 0f && matchesFound < totalMatches)
        {
            GameOver(); // �ð� �ʰ� �� ���� ����
        }
    }

    // ���� ī�� üũ ������ Ȯ���ϴ� �޼ҵ�
    public bool IsCheckingCards()
    {
        return isChecking;
    }

    // ������ ����Ǿ����� Ȯ���ϴ� �޼ҵ� (�ð� ���� �Ǵ� ��� ��ġ ã��)
    public bool IsGameOver()
    {
        return time <= 0 || matchesFound >= totalMatches;
    }

    // ī�尡 �������� �� ȣ��Ǵ� �޼ҵ�
    public void CardFlipped(Card card)
    {
        if (firstCard == null)
        {
            // ù ��°�� ������ ī��
            firstCard = card;
        }
        else if (secondCard == null && firstCard != card)
        {
            // �� ��°�� ������ ī��
            secondCard = card;

            // ��ġ Ȯ�� ���μ��� ����
            isChecking = true;
            StartCoroutine(CheckMatch());
        }
    }

    // �� ī���� ��ġ ���θ� Ȯ���ϴ� �ڷ�ƾ
    private IEnumerator CheckMatch()
    {
        // �÷��̾ ī�带 �� �� �ֵ��� ��� ���
        yield return new WaitForSeconds(1.0f);

        if (firstCard.GetIndex() == secondCard.GetIndex())
        {
            // ī�� ��ġ ����!
            cardScore++;
            matchesFound++;

            // ��� ��ġ�� ã�Ҵ��� Ȯ��
            if (matchesFound >= totalMatches)
            {
                Clear(); // ���� Ŭ����
            }
        }
        else
        {
            // ī�尡 ��ġ���� ����, �� ī�� ��� ������
            firstCard.ResetCard();
            secondCard.ResetCard();
        }

        // ���� ���� ���� ����
        firstCard = null;
        secondCard = null;
        isChecking = false;
    }

    // ���� ���� ���� ó��
    private void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null)
        {
            ScoreUI();     // �ð� ���� ������Ʈ
            CardnumUI();   // ī�� ��ġ ���� ������Ʈ
            gameOverPanel.SetActive(true);
            Time.timeScale = 0.0f; // ���� �Ͻ� ����
        }
    }

    // ���� Ŭ����(�¸�) ���� ó��
    private void Clear()
    {
        isGameOver = true;
        if (clearPanel != null)
        {
            // ���ھ� ������Ʈ �� ���
            ScoreUI();     // �ð� ���� ������Ʈ
            CardnumUI();   // ī�� ��ġ ���� ������Ʈ
            clearPanel.SetActive(true);
            Time.timeScale = 0.0f; // ���� �Ͻ� ����
        }
    }

    // ī�� ��ġ UI ������Ʈ �� �ְ� ���� ����
    public void CardnumUI()
    {
        if (PlayerPrefs.HasKey(cardsKey))
        {
            minCardScore = PlayerPrefs.GetInt(cardsKey, cardScore);
            if (minCardScore < cardScore)
            {
                // ���ο� �ְ� ��� �޼�
                PlayerPrefs.SetInt(cardsKey, cardScore);
                cardNumText.text = Cards.ToString();
            }
            else
            {
                // ���� �ְ� ��� ����
                cardNumText.text = minCardScore.ToString();
            }
        }
        else
        {
            // ù �÷��� �� ��� ����
            PlayerPrefs.SetInt(timeKey, Cards);
            cardNumText.text = Cards.ToString();
        }
    }

    // �ð� ���� UI ������Ʈ �� �ְ� ���� ����
    public void ScoreUI()
    {
        if (PlayerPrefs.HasKey(timeKey))
        {
            bestScore = PlayerPrefs.GetFloat(timeKey, time);

            if (bestScore > time)
            {
                // ���ο� �ְ� ���(�� ���� �ð�) �޼�
                PlayerPrefs.SetFloat(timeKey, time);
                bestScoreText.text = time.ToString("N2");
            }
            else
            {
                // ���� �ְ� ��� ����
                bestScoreText.text = bestScore.ToString("N2");
            }
        }
        else
        {
            // ù �÷��� �� ��� ����
            PlayerPrefs.SetFloat(timeKey, time);
            bestScoreText.text = time.ToString("N2");
        }
    }

    // ���� ����� �޼ҵ�
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}