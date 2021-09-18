using UnityEngine;
using DG.Tweening;

public class CardScript : MonoBehaviour
{
    [SerializeField] private int value;
    public int Value { get { return value; } set { this.value = value; } }
    public JQK jqk = JQK.NONE;

    public SpriteRenderer spriteRenderer;
    private Sprite firstSpr;  //ī���� �ո� ��������Ʈ

    private Vector3 rot1;
    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        firstSpr = spriteRenderer.sprite;

        rot1 = new Vector3(0, -90, 0);
    }

    public void SetSprite(bool back=true)
    {
        if (!back) spriteRenderer.sprite = firstSpr;
        else spriteRenderer.sprite = RuleManager.Instance.ruleData.backSprite;
    }

    public void RotateCard()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(rot1, 0.12f));
        seq.AppendCallback(() =>
        {
            SetSprite(false);
            transform.DOLocalRotate(Vector3.zero, 0.12f);
        });
    }
}
