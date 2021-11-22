using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    private WaitForSeconds lWs = new WaitForSeconds(.3f);

    public CanvasGroup loadingPanel;  //로딩창(걍 페이드 인/아웃에 쓰일 검은 화면)
    public Button screenTouchPanelBtn;
    public Ease[] eases;

    public List<GameObject> gameUIs;
    [SerializeField] private List<GameObject> uiList = new List<GameObject>();
    [SerializeField] private List<int> scrPanelIdx;
    private Dictionary<int, bool> scrPanelDic = new Dictionary<int, bool>();

    public Text systemText;

    public TrashCardUI[] trashCardUIArr;

    private IEnumerator Start()
    {
        while (!GameManager.Instance.isReady) yield return lWs;    //yield return null;

        if (GameManager.Instance.scType == SceneType.MAIN)
        {
            while (!RuleManager.Instance.isReady) yield return lWs;
        }

        screenTouchPanelBtn.onClick.AddListener(() => ViewUI(gameUIs.IndexOf(uiList[uiList.Count - 1])) );
        scrPanelIdx.ForEach(x => scrPanelDic.Add(x, true));
        FadeInOut(true);
    }

    private void Update()
    {
        _Input();
    }

    public void FadeInOut(bool fadeIn)
    {
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.DOFade(fadeIn ? 0 : 1, 1).SetEase(eases[0]);
    }

    void _Input()
    {
        if(Input.GetKeyDown(KeyCode.Escape))  //뒤로가기
        {
            if(uiList.Count>0)
            {
                ViewUI(gameUIs.IndexOf(uiList[uiList.Count-1]));
            }
            else
            {
                //종료 창
            }
        }
    }

    public void ViewUI(int num)  //스케일이 점점 변하는 애니메이션으로 적용
    {
        bool active = gameUIs[num].activeSelf;

        if(!active)
        {
            gameUIs[num].SetActive(true);
            uiList.Add(gameUIs[num]);
            gameUIs[num].transform.DOScale(Vector3.one, 0.4f).SetEase(eases[1]);
            if (scrPanelDic.ContainsKey(num)) screenTouchPanelBtn.gameObject.SetActive(true);
        }
        else
        {
            uiList.Remove(gameUIs[num]);
            Sequence seq = DOTween.Sequence();
            seq.Append(gameUIs[num].transform.DOScale(Vector3.zero, 0.3f).SetEase(eases[2]));
            seq.AppendCallback(() =>
            {
                gameUIs[num].SetActive(false);
                screenTouchPanelBtn.gameObject.SetActive(false);
            }).Play();
        }
    }

    public void OnSystemMsg(string msg, int fontSize=81)
    {
        systemText.fontSize = fontSize;
        systemText.text = msg;
        ViewUI(1);
    }

    public void SetTrashUI(CardScript card = null)
    {
        if (card == null)  //초기화
        {
            for (int i = 0; i < trashCardUIArr.Length; i++) trashCardUIArr[i].UpdateUI();
            return;
        }

        int idx = card.Value-1;
        trashCardUIArr[idx].trashCnt++;
        trashCardUIArr[idx].isTrashShape[(int)card.cardShape] = true;
        trashCardUIArr[idx].UpdateUI((int)card.cardShape);
    }
}
