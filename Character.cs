using UnityEngine;
using UnityEngine.AI;
using System;
using DG.Tweening;
using TMPro;

public class Character : MonoBehaviour
{   
    [SerializeField] private int life = 0;
    [SerializeField] private bool isManualControl = false;
    [SerializeField] private GameObject cursor = null;
    [SerializeField] private string key = null;

    private TMP_Text lifeText;
    private Vector3 startPosition;
    private CharacterJoystickController characterJoystickController;
    private SphereCollider sphereCollider;
    private NavMeshAgent agent;
    private CharacterController characterController;
   
    public static Action<Vector3> TargetPositionEvent;
    public static Action<string> ReproductionObjectCountEvent;
    public static Action<string> ReductionObjectCountEvent;

    public static Action DeactivateJoystickEvent;
    public static Action DecreaseFoodEvent;

    public static Action <string, int> SumLifeEvent;

    private void OnEnable()
    {
        Timer.EndDayEvent += EndDay;
        FoodController.EndDayEvent += EndDay;
        SpawnerObjects.OffCharacterControllerEvent += ObjectController;
        SpawnerObjects.OnCharacterControllerEvent += ObjectController;
    }

    private void OnDisable()
    {
        Timer.EndDayEvent -= EndDay;
        FoodController.EndDayEvent += EndDay;
        SpawnerObjects.OffCharacterControllerEvent -= ObjectController;
        SpawnerObjects.OnCharacterControllerEvent -= ObjectController;
    }

    private void Awake()
    {
        characterJoystickController = GetComponent<CharacterJoystickController>();

        if (characterJoystickController != null)
        {
            characterJoystickController.enabled = false;
        }        
    }

    private void Start()
    {
        characterController = FindObjectOfType<CharacterController>();
        sphereCollider = GetComponent<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();
        lifeText = GetComponentInChildren<TMP_Text>();
        life = 0;
        startPosition = gameObject.transform.position;
    }

    public void AddFood(int _numberFood)
    {
        life += _numberFood;

        ShowLifeCharacter(life);
        SumLifeEvent?.Invoke(key, _numberFood);

        if (life == 2)
        {          
            ReproductionObjectCountEvent?.Invoke(key);
        }
    }

    private void ShowLifeCharacter(int _life)
    {
        lifeText.text = "" + _life; 
    }

    private void EndDay()
    {
        if (life == 0)
        {
            ReductionObjectCountEvent?.Invoke(key);
        }

        transform.DOKill();
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void ObjectController(GameObject _character, bool _enabledController, bool _enableAgent)
    {
        if (_character == gameObject)
        {
            if (characterJoystickController != null)
            {
                cursor.SetActive(_enabledController);
                characterJoystickController.enabled = _enabledController;                
                isManualControl = _enabledController;
            }         
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController.CharacterData character = characterController.SearchCharacter(key);
        CharacterController.CharacterData otherCharacter = new CharacterController.CharacterData();

        if (other.gameObject.GetComponent<Character>() != null)
        {
            otherCharacter = characterController.SearchCharacter(other.gameObject.GetComponent<Character>().key);
        }

        if (character.characterType == CharacterController.CharacterData.СharacterType.HERBIVORE)
        {
            if (other.transform.gameObject.GetComponent<Food>())
            {
                CollisionWithHerbivoreFood(other.gameObject);
            }
        }

        if (character.characterType == CharacterController.CharacterData.СharacterType.PREDATOR)
        {
            if (other.transform.gameObject.GetComponent<Food>())
            {
                CollisionWithHerbivoreFood(other.gameObject);
            }

            if (other.transform.gameObject.GetComponent<Character>() && character.key != otherCharacter.key)
            {
                if (character.attack > otherCharacter.defense)
                {
                    //Debug.Log(gameObject.name);
                    //Debug.Log(other.gameObject.name);
                    CollisionWithVictim(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.GetComponent<CharacterJoystickController>() && other.transform.gameObject.GetComponent<NavMeshObstacle>())
        {
            if (gameObject.GetComponent<CharacterJoystickController>().enabled == true)
            {
                gameObject.GetComponent<SphereCollider>().isTrigger = true;
            }
        }
    }

    private void CollisionWithHerbivoreFood(GameObject _food)
    {
        sphereCollider.enabled = false;
        _food.transform.gameObject.GetComponent<SphereCollider>().enabled = false;

        if (!isManualControl && _food != null)
        {
            Vector3 target = _food.transform.position;
            gameObject.transform.DOMove(target, 1.5f).OnComplete(() => EatFood(_food.transform.gameObject));
        }
        else
        {
            EatFood(_food.transform.gameObject);
        }
    }

    private void CollisionWithVictim(GameObject _victim)
    {
        sphereCollider.enabled = false;

        if (_victim.transform.gameObject.GetComponent<SphereCollider>())
        {
            _victim.transform.gameObject.GetComponent<SphereCollider>().enabled = false;
        }

        if (_victim.GetComponent<Character>())
        {
            if (_victim.GetComponent<Character>().life == 0)
            {
                ReductionObjectCountEvent?.Invoke(_victim.gameObject.GetComponent<Character>().key);
            }
        }
        
        gameObject.transform.DOMove(_victim.transform.position, 0.5f).OnComplete(() => EatVictim(_victim.transform.gameObject));
    }

    private void EatVictim(GameObject _victim)
    {
        _victim.SetActive(false);
        AddFood(2);
        sphereCollider.enabled = true;
    }

    private void EatFood(GameObject _food)
    {
        _food.SetActive(false);
        AddFood(1);
        sphereCollider.enabled = true;

        DecreaseFoodEvent?.Invoke();
    }
}