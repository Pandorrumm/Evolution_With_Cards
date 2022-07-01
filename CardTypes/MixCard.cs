using UnityEngine;
using UnityEngine.AI;
using System;

public class MixCard : Card
{
    [SerializeField] private float viewRadius = 0f;
    [SerializeField] private float speed = 0f;

    private void OnEnable()
    {
        CardSelection.AssignNPCMixParametersEvent += TakeSpeedAndRadius;
    }

    private void OnDisable()
    {
        CardSelection.AssignNPCMixParametersEvent -= TakeSpeedAndRadius;
    }

    private void Start()
    {
        AssignCardButtonsEvent?.Invoke(key, TakeSpeedAndRadius);
    }

    public void TakeSpeedAndRadius(string _key, bool _isIncrease)
    {
        CharacterController.CharacterData data = characterController.SearchCharacter(_key);

        if (_isIncrease)
        {
            data.speed += speed;
            data.viewRadius += viewRadius;

            if (data.character.GetComponent<NavMeshAgent>() != null)
            {
                data.character.GetComponent<NavMeshAgent>().speed = data.speed;
            }

            data.character.GetComponent<SphereCollider>().radius = data.viewRadius;

            if (data.character.GetComponent<CharacterJoystickController>() != null)
            {
                data.character.GetComponent<CharacterJoystickController>().moveSpeed += speed;
            }
        }
        else
        {
            data.speed -= speed;
            data.viewRadius -= viewRadius;

            if (data.character.GetComponent<NavMeshAgent>() != null)
            {
                data.character.GetComponent<NavMeshAgent>().speed = data.speed;
            }

            data.character.GetComponent<SphereCollider>().radius = data.viewRadius;

            if (data.character.GetComponent<CharacterJoystickController>() != null)
            {
                data.character.GetComponent<CharacterJoystickController>().moveSpeed -= speed;
            }
        }       
    }
}
