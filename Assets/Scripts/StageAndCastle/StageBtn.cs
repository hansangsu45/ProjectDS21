using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StageBtn : MonoBehaviour
{
    public StageCastle stageCastle;

    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();

        btn.onClick.AddListener(() =>
        {
            if (!stageCastle.isClear)
            {

            }
            else
            {

            }
        });
    }
}
