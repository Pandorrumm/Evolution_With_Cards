using UnityEngine;
using UnityEngine.AI;
using System;

public class SpeedCard : Card
{
    [SerializeField] private float speed = 0f;

    private void OnEnable()
    {
        CardSelection.AssignNPCSpeedEvent += TakeSpeed;
    }

    private void OnDisable()
    {
        CardSelection.AssignNPCSpeedEvent -= TakeSpeed;
    }

    private void Start()
    {
        AssignCardButtonsEvent?.Invoke(key, TakeSpeed);
    }

    public void TakeSpeed(string _key, bool _isIncrease)
    {
        CharacterController.CharacterData data = characterController.SearchCharacter(_key);

        if (_isIncrease)
        {
            data.speed += speed;

            if (data.character.GetComponent<NavMeshAgent>() != null)
            {
                data.character.GetComponent<NavMeshAgent>().speed = data.speed;
            }            

            if (data.character.GetComponent<CharacterJoystickController>() != null)
            {
                data.character.GetComponent<CharacterJoystickController>().moveSpeed += speed;
            }
        }
        else
        {
            data.speed -= speed;

            if (data.character.GetComponent<NavMeshAgent>() != null)
            {
                data.character.GetComponent<NavMeshAgent>().speed = data.speed;
            }

            if (data.character.GetComponent<CharacterJoystickController>() != null)
            {
                data.character.GetComponent<CharacterJoystickController>().moveSpeed -= speed;
            }
        }       
    }
}
