using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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


    private void Awake()
    {

    }
    void Start()
    {
        // �̱��� ����
        if (instance == null)
        {
            instance = this;
        }

        UpdateBestRecord(); //�����Ҷ� �ְ����� ���̱�

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
            UpdateBestRecord();
            gameOverPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }


    // ���� Ŭ����(�¸�) ���� ó��
    private void Clear()
    {
        isGameOver = true;
        if (clearPanel != null)
        {
            UpdateBestRecord();
            clearPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }



    public void UpdateBestRecord()
    {
       

        int savedCardScore = PlayerPrefs.GetInt(cardsKey, 0);
        float savedTime = PlayerPrefs.GetFloat(timeKey, float.MaxValue);

        bool isBetterRecord = false;

        
        if (cardScore > savedCardScore)// ī�� ���� �� ������ ����
        {
            PlayerPrefs.SetInt(cardsKey, cardScore);
            PlayerPrefs.SetFloat(timeKey, time);
            isBetterRecord = true;
        }
      
        else if (cardScore == savedCardScore && time > savedTime)  // ī�� ���� ���� �ð��� �� ª���� ����
        {
            PlayerPrefs.SetFloat(timeKey, time);
            isBetterRecord = true;
        }
       

        if (isBetterRecord) //  (���� �� ��) ī�� ���� �۰ų� �ð��� �� ������ �н� �� �ȵſ���
        {
            PlayerPrefs.Save();
          
        }
        
        

        // UI �ֽ�ȭ
        int bestCards = PlayerPrefs.GetInt(cardsKey, 0);
        float bestTime = PlayerPrefs.GetFloat(timeKey, 0f);

        cardNumText.text = bestCards.ToString();
        bestScoreText.text = bestTime.ToString("N2");

      
    }


   
}