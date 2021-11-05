
using UnityEngine;

public class Test1 : MonoBehaviour
{
    public Animator ani;
    bool open = true;

    public void Click()
    {
        ani.SetInteger("open", open?1:2);
        open = !open;
    }
}
