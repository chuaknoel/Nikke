using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ���� ���忡 ī�� ���� �� ��ġ�� ó���ϴ� Ŭ����
/// </summary>
public class Board : MonoBehaviour
{
    public GameObject card; // ������ ī�� ������

    void Start()
    {
        GenerateBoard();
    }

    /// <summary>
    /// ī�带 �����ϰ� ������ ������ �׸��忡 ��ġ�մϴ�.
    /// </summary>
    private void GenerateBoard()
    {
        int cardCount = GameManager.instance.Cards;

        // ī�� ���� ������ �迭 ���� (���� �̷�� ����)
        int[] cardValues = new int[cardCount];

        // �� ī�� ���� �� ���� �Ҵ� (�� ����)
        for (int i = 0; i < cardCount / 2; i++)
        {
            cardValues[i * 2] = i;     // ���� ù ��° ī��
            cardValues[i * 2 + 1] = i; // ���� �� ��° ī��
        }

        // ī�� ���� ������ȭ (����)
        cardValues = cardValues.OrderBy(x => Random.value).ToArray();

        // ī�� ���� �� ��ġ
        for (int i = 0; i < cardCount; i++)
        {
            // ī�� ����
            GameObject cardObject = Instantiate(card, transform);

            // �׸��� �� ��ġ ��� (6x4 �׸��� ����)
            float x = (i % 6) * 1.6f - 3.9f;
            float y = (i / 6) * 2.0f - 3.0f;

            // ī�� ��ġ ����
            cardObject.transform.position = new Vector2(x, y);

            // ī�� �� ����
            cardObject.GetComponent<Card>().Setting(cardValues[i]);
        }

        // GameManager�� ���� ��ġ �� ���� (ī�� ���� ����)
        GameManager.instance.totalMatches = cardCount / 2;
    }
}