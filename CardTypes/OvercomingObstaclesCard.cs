using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class OvercomingObstaclesCard : Card
{
    public static bool _isTakeOvercomingObstaclesCard;

    private void Start()
    {
        AssignCardButtonsEvent?.Invoke(key, OvercomingObstacles);
    }

    public void OvercomingObstacles(string _key, bool _isIncrease)
    {
        CharacterController.CharacterData data = characterController.SearchCharacter(_key);

        if (_isIncrease)
        {
            data.character.GetComponent<CharacterPatrol>().isMoveOnNavMesh = false;
            data.character.GetComponent<CharacterPatrol>().travelling = true;
            data.isMoveOnNavMesh = data.character.GetComponent<CharacterPatrol>().isMoveOnNavMesh;
            data.character.GetComponent<NavMeshAgent>().enabled = false;

            _isTakeOvercomingObstaclesCard = true;
        }
        else
        {
            data.character.GetComponent<CharacterPatrol>().isMoveOnNavMesh = true;
            data.character.GetComponent<CharacterPatrol>().travelling = true;
            data.isMoveOnNavMesh = data.character.GetComponent<CharacterPatrol>().isMoveOnNavMesh;
            data.character.GetComponent<NavMeshAgent>().enabled = true;

            _isTakeOvercomingObstaclesCard = false;
        }
    }
}
