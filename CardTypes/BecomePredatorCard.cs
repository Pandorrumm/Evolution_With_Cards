using UnityEngine;
using System;

public class BecomePredatorCard : Card
{
    private void OnEnable()
    {
        CardSelection.AssignNPCSBecomePredatorEvent += BecomePredator;
    }

    private void OnDisable()
    {
        CardSelection.AssignNPCSBecomePredatorEvent -= BecomePredator;
    }

    private void Start()
    {
        AssignCardButtonsEvent?.Invoke(key, BecomePredator);
    }

    public void BecomePredator(string _key, bool _isIncrease)
    {
        CharacterController.CharacterData data = characterController.SearchCharacter(_key);

        if (_isIncrease)
        {
            data.characterType = CharacterController.CharacterData.СharacterType.PREDATOR;
        }
        else
        {
            data.characterType = CharacterController.CharacterData.СharacterType.HERBIVORE;
        }                        
    }
}
