using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerUI playerUI;
    [Header("ÊÂ¼þ¼àÌý")]
    public CharacterObjectSO healthEvent;
    public CharacterObjectSO xiadunEvent;

    private void OnEnable()
    {
        healthEvent.onEventRaised += OnHealthChange;
        xiadunEvent.onEventRaised += OnXiadunChange;
    }

    private void OnDisable()
    {
        healthEvent.onEventRaised -= OnHealthChange;
        xiadunEvent.onEventRaised -= OnXiadunChange;
    }

    private void OnHealthChange(Character character)
    {
        var persentage = character.nowLife / character.maxLife;
        playerUI.OnHealthChange(persentage);
    }
    private void OnXiadunChange(Character character)
    {
        var persentage = character.nowCrouchTime / character.crouchTime;
        playerUI.OnXiadunChange(persentage);
    }
}
