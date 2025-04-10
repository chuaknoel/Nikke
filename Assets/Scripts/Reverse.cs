using System.Collections;
using UnityEngine;

/// <summary>
/// ī���� �ո�� �޸� ������ ������ �ִϸ��̼��� ó���ϴ� Ŭ����
/// </summary>
public class Reverse : MonoBehaviour
{
    public GameObject front; // ī���� �ո� (�̹����� �����ִ� ��)
    public GameObject back;  // ī���� �޸� (�⺻ ����)

    private bool isFlipped = false; // ī�� ���� ����: false = �޸� ǥ��, true = �ո� ǥ��

    void Start()
    {
        // ī�� �ʱ� ���¸� �޸����� ����
        if (front != null && back != null)
        {
            front.SetActive(false); // ���� �� �ո� �����
            back.SetActive(true);   // ���� �� �޸� ǥ��
        }
    }

    /// <summary>
    /// ī�� ������ �ִϸ��̼��� Ʈ�����մϴ�.
    /// </summary>
    /// <param name="target">�ִϸ��̼��� ������ Ʈ������</param>
    /// <param name="time">������ �ִϸ��̼��� �� �ð�</param>
    public void FlipCard(Transform target, float time)
    {
        StartCoroutine(FlipAnimation(target, time));
    }

    /// <summary>
    /// ī�� ������ �ִϸ��̼��� ó���ϴ� �ڷ�ƾ
    /// </summary>
    private IEnumerator FlipAnimation(Transform target, float time)
    {
        float halfTime = time / 2f;
        float elapsedTime = 0.0f;
        Vector3 originalScale = target.localScale;

        // 1�ܰ�: ī�带 ���η� ��� (x �������� 1���� 0����)
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

        // 2�ܰ�: ī�尡 ������ ��ҵǾ��� �� ��/�޸� ��ȯ
        isFlipped = !isFlipped; // ���� ���

        if (front != null && back != null)
        {
            if (isFlipped)
            {
                // �ո� ǥ��, �޸� ����
                front.SetActive(true);
                back.SetActive(false);
            }
            else
            {
                // �޸� ǥ��, �ո� ����
                front.SetActive(false);
                back.SetActive(true);
            }
        }

        // 3�ܰ�: ī�带 ���η� Ȯ�� (x �������� 0���� 1��)
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

        // ���� �������� �ùٸ��� Ȯ��
        Vector3 finalScale = originalScale;
        finalScale.x = 1f;
        target.localScale = finalScale;
    }
}