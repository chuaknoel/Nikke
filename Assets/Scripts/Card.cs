using UnityEngine;
using UnityEngine.InputSystem; // �� Input System ���

/// <summary>
/// �޸� ��Ī ������ ī�带 ǥ���ϰ� ��ȣ�ۿ��� ó���ϴ� Ŭ����
/// </summary>
public class Card : MonoBehaviour
{
    public int idx = 0;             // ī���� ��Ī ��
    public SpriteRenderer frontImage; // ī�� �ո��� ��������Ʈ ������

    private GameManager gameManager;  // ���� �Ŵ��� ����
    private Reverse reverseComponent; // ī�� ������ ������Ʈ ����
    private bool isFlipped = false;   // ī�尡 ���� ������ �ִ��� ����

    void Start()
    {
        // �ʿ��� ������Ʈ ��������
        gameManager = FindObjectOfType<GameManager>();
        reverseComponent = GetComponent<Reverse>();

        // �ݶ��̴��� ������ �߰�
        EnsureCollider();
    }

    void Update()
    {
        // �� ī�忡 ���� ���콺 Ŭ�� Ȯ��
        CheckForCardClick();
    }

    /// <summary>
    /// ī�忡 ������ ũ���� �ݶ��̴��� �ִ��� Ȯ���մϴ�.
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
    /// �� ī�忡 ���� ���콺 Ŭ���� Ȯ���ϰ� ó���մϴ�.
    /// </summary>
    private void CheckForCardClick()
    {
        // ���� ���콺 ��ư Ŭ�� Ȯ��
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // ���콺 ��ũ�� ��ġ�� ���� ��ġ�� ��ȯ
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // ���콺�� �� ī�� ���� �ִ��� Ȯ��
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);

            if (hitCollider != null && hitCollider.gameObject == this.gameObject)
            {
                // ī�尡 �̹� �������ų� ������ ����Ǿ��ų� �ٸ� ī�带 Ȯ�� ���̸� ����
                if (isFlipped || gameManager.IsGameOver() || gameManager.IsCheckingCards())
                    return;

                // ī�� ������
                reverseComponent.FlipCard(transform, 0.5f);
                isFlipped = !isFlipped;

                // ���� �Ŵ����� �˸�
                gameManager.CardFlipped(this);
            }
        }
    }

    /// <summary>
    /// ī�带 ��Ī ���� ��������Ʈ�� �����մϴ�.
    /// </summary>
    /// <param name="number">�� ī�忡 �Ҵ��� ��</param>
    public void Setting(int number)
    {
        idx = number;
        frontImage.sprite = Resources.Load<Sprite>($"nk{idx}");
    }

    /// <summary>
    /// ī���� ��Ī ���� ��ȯ�մϴ�.
    /// </summary>
    public int GetIndex()
    {
        return idx;
    }

    /// <summary>
    /// ī�带 �⺻ ����(�޸�)�� �ǵ����ϴ�.
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