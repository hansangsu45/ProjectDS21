using System.Collections.Generic;
using UnityEngine;

public enum SoundEffectType
{
    CARD_OVERTURN
}

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioClip[] effectClips; //���� Ÿ���� ������� �迭�� ����ִ´�
}
