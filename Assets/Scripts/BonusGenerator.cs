using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _hpBonusPrefab;

    public void Generate()
    {
        Instantiate(_hpBonusPrefab, transform.position, new Quaternion());
    }

}
