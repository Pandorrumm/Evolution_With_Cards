using UnityEngine;
using System;

public class DefenceCard : Card
{
    [SerializeField] private float defense = 0f;

    private void OnEnable()
    {
        CardSelection.AssignNPCDefenseEvent += TakeDefence;
    }

    private void OnDisable()
    {
        CardSelection.AssignNPCDefenseEvent -= TakeDefence;
    }

    private void Start()
    {
        AssignCardButtonsEvent?.Invoke(key, TakeDefence);
    }

    public void TakeDefence(string _key, bool _isIncrease)
    {
        CharacterController.CharacterData data = characterController.SearchCharacter(_key);

        if (_isIncrease)
        {
            data.defense += defense;
        }
        else
        {
            data.defense -= defense;
        }
    }
}
