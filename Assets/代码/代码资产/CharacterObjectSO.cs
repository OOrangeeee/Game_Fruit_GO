using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/CharacterObjectSO")]
public class CharacterObjectSO : ScriptableObject
{
    public UnityAction<Character> onEventRaised;

    public void RaiseEvent(Character character)
    {
        onEventRaised?.Invoke(character);
    }
}
