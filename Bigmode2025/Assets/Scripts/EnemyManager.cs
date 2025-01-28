using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] TMP_Text text;

    public List<GameObject> enemies;

    public void UpdateCount()
    {
        text.text = enemies.Count.ToString();
    }

}
