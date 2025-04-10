using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 게임 보드에 카드 생성 및 배치를 처리하는 클래스
/// </summary>
public class Board : MonoBehaviour
{
    public GameObject card; // 생성할 카드 프리팹

    void Start()
    {
        GenerateBoard();
    }

    /// <summary>
    /// 카드를 생성하고 무작위 값으로 그리드에 배치합니다.
    /// </summary>
    private void GenerateBoard()
    {
        int cardCount = GameManager.instance.Cards;

        // 카드 값을 저장할 배열 생성 (쌍을 이루는 숫자)
        int[] cardValues = new int[cardCount];

        // 각 카드 값을 두 번씩 할당 (쌍 생성)
        for (int i = 0; i < cardCount / 2; i++)
        {
            cardValues[i * 2] = i;     // 쌍의 첫 번째 카드
            cardValues[i * 2 + 1] = i; // 쌍의 두 번째 카드
        }

        // 카드 순서 무작위화 (셔플)
        cardValues = cardValues.OrderBy(x => Random.value).ToArray();

        // 카드 생성 및 배치
        for (int i = 0; i < cardCount; i++)
        {
            // 카드 생성
            GameObject cardObject = Instantiate(card, transform);

            // 그리드 내 위치 계산 (6x4 그리드 기준)
            float x = (i % 6) * 1.6f - 3.9f;
            float y = (i / 6) * 2.0f - 3.0f;

            // 카드 위치 설정
            cardObject.transform.position = new Vector2(x, y);

            // 카드 값 설정
            cardObject.GetComponent<Card>().Setting(cardValues[i]);
        }

        // GameManager에 예상 매치 수 설정 (카드 쌍의 개수)
        GameManager.instance.totalMatches = cardCount / 2;
    }
}