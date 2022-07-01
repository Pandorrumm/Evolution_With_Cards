using UnityEngine;
using System;

public class ViewRadiusCard : Card
{
    [SerializeField] private float viewRadius = 0f;

    private void OnEnable()
    {
        CardSelection.AssignNPCViewRadiusEvent += TakeViewRadius;
    }

    private void OnDisable()
    {
        CardSelection.AssignNPCViewRadiusEvent -= TakeViewRadius;
    }

    private void Start()
    {
        AssignCardButtonsEvent?.Invoke(key, TakeViewRadius);
    }

    public void TakeViewRadius(string _key, bool _isIncrease)
    {
        CharacterController.CharacterData data = characterController.SearchCharacter(_key);

        if (_isIncrease)
        {
            data.viewRadius += viewRadius;
            data.character.GetComponent<SphereCollider>().radius = data.viewRadius;
        }
        else
        {
            data.viewRadius -= viewRadius;
            data.character.GetComponent<SphereCollider>().radius = data.viewRadius;
        }       
    }
}
