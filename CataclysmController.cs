using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CataclysmController : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles = null;
    [SerializeField] private Button overcomingObstaclesButton = null;

    private void OnEnable()
    {
       DayManager.EnableCataclysmEvent += ActivateCataclism;
    }

    private void OnDisable()
    {
        DayManager.EnableCataclysmEvent -= ActivateCataclism;
    }

    private void ActivateCataclism()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].SetActive(true);
            overcomingObstaclesButton.interactable = true;
        }
    }
}