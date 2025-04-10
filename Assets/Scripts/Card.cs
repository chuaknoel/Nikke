using UnityEngine;
using UnityEngine.InputSystem; // 새 Input System 사용

/// <summary>
/// 메모리 매칭 게임의 카드를 표현하고 상호작용을 처리하는 클래스
/// </summary>
public class Card : MonoBehaviour
{
    public int idx = 0;             // 카드의 매칭 값
    public SpriteRenderer frontImage; // 카드 앞면의 스프라이트 렌더러

    private GameManager gameManager;  // 게임 매니저 참조
    private Reverse reverseComponent; // 카드 뒤집기 컴포넌트 참조
    private bool isFlipped = false;   // 카드가 현재 뒤집혀 있는지 여부

    void Start()
    {
        // 필요한 컴포넌트 가져오기
        gameManager = FindObjectOfType<GameManager>();
        reverseComponent = GetComponent<Reverse>();

        // 콜라이더가 없으면 추가
        EnsureCollider();
    }

    void Update()
    {
        // 이 카드에 대한 마우스 클릭 확인
        CheckForCardClick();
    }

    /// <summary>
    /// 카드에 적절한 크기의 콜라이더가 있는지 확인합니다.
    /// </summary>
    private void EnsureCollider()
    {
        if (GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(1.5f, 1.5f);
        }
    }

    /// <summary>
    /// 이 카드에 대한 마우스 클릭을 확인하고 처리합니다.
    /// </summary>
    private void CheckForCardClick()
    {
        // 왼쪽 마우스 버튼 클릭 확인
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 마우스 스크린 위치를 월드 위치로 변환
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // 마우스가 이 카드 위에 있는지 확인
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);

            if (hitCollider != null && hitCollider.gameObject == this.gameObject)
            {
                // 카드가 이미 뒤집혔거나 게임이 종료되었거나 다른 카드를 확인 중이면 무시
                if (isFlipped || gameManager.IsGameOver() || gameManager.IsCheckingCards())
                    return;

                // 카드 뒤집기
                reverseComponent.FlipCard(transform, 0.5f);
                isFlipped = !isFlipped;

                // 게임 매니저에 알림
                gameManager.CardFlipped(this);
            }
        }
    }

    /// <summary>
    /// 카드를 매칭 값과 스프라이트로 설정합니다.
    /// </summary>
    /// <param name="number">이 카드에 할당할 값</param>
    public void Setting(int number)
    {
        idx = number;
        frontImage.sprite = Resources.Load<Sprite>($"nk{idx}");
    }

    /// <summary>
    /// 카드의 매칭 값을 반환합니다.
    /// </summary>
    public int GetIndex()
    {
        return idx;
    }

    /// <summary>
    /// 카드를 기본 상태(뒷면)로 되돌립니다.
    /// </summary>
    public void ResetCard()
    {
        if (isFlipped)
        {
            reverseComponent.FlipCard(transform, 0.5f);
            isFlipped = false;
        }
    }
}