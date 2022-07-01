using UnityEngine;
using System;

public class AttackCard : Card
{
    [SerializeField] private float attack = 0f;

    private void OnEnable()
    {
        CardSelection.AssignNPCAttackEvent += TakeAttack;
    }

    private void OnDisable()
    {
        CardSelection.AssignNPCAttackEvent -= TakeAttack;
    }

    private void Start()
    {
        AssignCardButtonsEvent?.Invoke(key, TakeAttack);
    }

    public void TakeAttack(string _key, bool _isIncrease)
    {
        CharacterController.CharacterData data = characterController.SearchCharacter(_key);
      
        if (_isIncrease)
        {
            data.attack += attack;
        }
        else
        {
            data.attack -= attack;
        }        
    }
}
