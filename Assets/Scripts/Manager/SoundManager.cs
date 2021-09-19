using System.Collections.Generic;
using UnityEngine;

public enum SoundEffectType
{
    CARD_OVERTURN
}

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioClip[] effectClips; //사운드 타입의 순서대로 배열에 집어넣는다
}
