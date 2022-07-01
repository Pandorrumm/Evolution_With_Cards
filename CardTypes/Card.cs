using UnityEngine;
using UnityEngine.UI;
using System;


public class Card : MonoBehaviour
{
    public string key;
    public CharacterController characterController;

    public static Action<string, Action<string, bool>> AssignCardButtonsEvent;
}
