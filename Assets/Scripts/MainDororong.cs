using UnityEngine;
using System.Collections.Generic;

public class MainDororong : MonoBehaviour
{
    public int type;
    float speed = 0.05f;
    static List<float> usedYPositions = new List<float>();

    float currentY;

    void Start()
    {
        Application.targetFrameRate = 60;

        float x = 10.0f;
        currentY = GetNonOverlappingY(); // y 값을 미리 저장해둠
        transform.position = new Vector2(x, currentY);

        switch (type)
        {
            case 1: speed = 2.0f; break;
            case 2: speed = 2.3f; break;
            case 3: speed = 2.7f; break;
            case 4: speed = 2.4f; break;
            case 5: speed = 2.5f; break;
            case 6: speed = 2.8f; break;
            default: speed = 0.05f; break;
        }
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // 왼쪽 화면을 벗어나면 오브젝트 제거
        if (transform.position.x < -9.5f)
        {
            usedYPositions.Remove(currentY); // y값도 해제
            Destroy(gameObject);
        }
    }

    float GetNonOverlappingY()
    {
        float y;
        int attempts = 0;
        bool found = false;

        do
        {
            y = Random.Range(-4.0f, 4.0f);
            found = true;

            foreach (float usedY in usedYPositions)
            {
                if (Mathf.Abs(usedY - y) < 1.0f)
                {
                    found = false;
                    break;
                }
            }

            attempts++;
        } while (!found && attempts < 100);

        usedYPositions.Add(y);
        return y;
    }
}
