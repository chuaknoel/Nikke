using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform cards;
    public GameObject card;
    // Start is called before the first frame update
    void Start()
    {
        // 1. 배열 크기를 24로 확장 (0~11까지 각 2장씩)
        int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11 };

        // 2. 랜덤 배치 로직 조정 (범위를 더 넓게)
        arr = arr.OrderBy(x => Random.Range(0f, 11f)).ToArray();
        for (int i = 0; i < 24; i++)
        {
            GameObject go = Instantiate(card, this.transform);

            float x = (i % 6) * 1.6f - 3.9f;
            float y = (i / 6) * 2.0f - 3.0f;
            go.transform.position = new Vector2(x, y);
            go.GetComponent<Card>().Setting(arr[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
