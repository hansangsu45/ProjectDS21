using System.Collections.Generic;
using UnityEngine;

public enum SoundEffectType
{
    CARD_OVERTURN,
    CARD_TAKEOUT
}

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioClip[] effectClips; //���� Ÿ���� ������� �迭�� ����ִ´�
}
