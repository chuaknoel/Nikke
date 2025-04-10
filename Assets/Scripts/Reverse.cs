using System.Collections;
using UnityEngine;

/// <summary>
/// 카드의 앞면과 뒷면 사이의 뒤집기 애니메이션을 처리하는 클래스
/// </summary>
public class Reverse : MonoBehaviour
{
    public GameObject front; // 카드의 앞면 (이미지를 보여주는 면)
    public GameObject back;  // 카드의 뒷면 (기본 상태)

    private bool isFlipped = false; // 카드 상태 추적: false = 뒷면 표시, true = 앞면 표시

    void Start()
    {
        // 카드 초기 상태를 뒷면으로 설정
        if (front != null && back != null)
        {
            front.SetActive(false); // 시작 시 앞면 숨기기
            back.SetActive(true);   // 시작 시 뒷면 표시
        }
    }

    /// <summary>
    /// 카드 뒤집기 애니메이션을 트리거합니다.
    /// </summary>
    /// <param name="target">애니메이션을 적용할 트랜스폼</param>
    /// <param name="time">뒤집기 애니메이션의 총 시간</param>
    public void FlipCard(Transform target, float time)
    {
        StartCoroutine(FlipAnimation(target, time));
    }

    /// <summary>
    /// 카드 뒤집기 애니메이션을 처리하는 코루틴
    /// </summary>
    private IEnumerator FlipAnimation(Transform target, float time)
    {
        float halfTime = time / 2f;
        float elapsedTime = 0.0f;
        Vector3 originalScale = target.localScale;

        // 1단계: 카드를 가로로 축소 (x 스케일을 1에서 0으로)
        while (elapsedTime < halfTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / halfTime);
            float currentX = Mathf.Lerp(1f, 0f, t);

            Vector3 scale = originalScale;
            scale.x = currentX;
            target.localScale = scale;

            yield return null;
        }

        // 2단계: 카드가 완전히 축소되었을 때 앞/뒷면 전환
        isFlipped = !isFlipped; // 상태 토글

        if (front != null && back != null)
        {
            if (isFlipped)
            {
                // 앞면 표시, 뒷면 숨김
                front.SetActive(true);
                back.SetActive(false);
            }
            else
            {
                // 뒷면 표시, 앞면 숨김
                front.SetActive(false);
                back.SetActive(true);
            }
        }

        // 3단계: 카드를 가로로 확장 (x 스케일을 0에서 1로)
        elapsedTime = 0.0f;
        while (elapsedTime < halfTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / halfTime);
            float currentX = Mathf.Lerp(0f, 1f, t);

            Vector3 scale = originalScale;
            scale.x = currentX;
            target.localScale = scale;

            yield return null;
        }

        // 최종 스케일이 올바른지 확인
        Vector3 finalScale = originalScale;
        finalScale.x = 1f;
        target.localScale = finalScale;
    }
}