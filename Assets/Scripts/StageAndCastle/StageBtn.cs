using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StageBtn : MonoBehaviour
{
    public StageCastle stageCastle;

    private Button btn;

    public CastleInfo cInfo;

    private void Awake()
    {
        btn = GetComponent<Button>();

        btn.onClick.AddListener(() =>
        {
            if (!stageCastle.isClear)
            {
                if (stageCastle.isOpen)
                {
                    //�ش� ���������� ����(ü��, ��ַ�, ����� �� �̹��� ��)�� �����ְ� ���� ������ �̵�
                }
                else
                {
                    //'���� ������� ���� ���������Դϴ�' â ���ų� ��ư Ŭ�� �ȵǰ� ���ش�
                }
            }
            else
            {
                //'�̹� Ŭ����� ���������Դϴ�' â ���ų� ��ư�� ��Ӱ� ���ְ� Ŭ�� �ȵǰ� ���ش�
            }
        });
    }
}
