using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LockDifficulty
{
    EASY,
    MEDIUM,
    HARD,
    VERY_HARD
}

public class LockController : MonoBehaviour
{
    [SerializeField] LockDifficulty lockDifficulty;
    
    float difficultyModifier;

    float lockpickRangeMin;
    float lockpickRangeMax;



    // Start is called before the first frame update
    void Start()
    {
        difficultyModifier = 20f - (int)lockDifficulty * 5;

        lockpickRangeMin = Random.Range(20f, 160f);
        lockpickRangeMax = lockpickRangeMin + difficultyModifier;

        Debug.Log("Lock Min: " + lockpickRangeMin);
        Debug.Log("Lock Max: " + lockpickRangeMax);
    }

    public float GetLockpickRangeMin() {
        return lockpickRangeMin;
    }

    public float GetLockpickRangeMax() {
        return lockpickRangeMax;
    }

    public LockDifficulty GetLockDifficulty() {
        return lockDifficulty;
    }
}
