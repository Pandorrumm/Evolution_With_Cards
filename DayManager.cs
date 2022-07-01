using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DayManager : Singleton<DayManager>
{
    public int dayCounter = 1;
    [SerializeField] private int dayOfCataclism = 0;

    public static Action EnableCataclysmEvent;

    private void OnEnable()
    {
        UIController.DayCounterEvent += IncreaseDays;
    }

    private void OnDisable()
    {
        UIController.DayCounterEvent -= IncreaseDays;
    }

    private void Awake()
    {
        if (HasInstance)
        {
            Destroy(gameObject);
        }
        else
        {
            InitInstance();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void IncreaseDays()
    {
        dayCounter++;

        if (dayCounter == dayOfCataclism)
        {
            EnableCataclysmEvent?.Invoke();
        }
    }
}
