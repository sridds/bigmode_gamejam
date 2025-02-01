using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TireUI : MonoBehaviour
{
    private List<GameObject> tires = new List<GameObject>();

    [SerializeField]
    private GameObject tireUIPrefab;

    private void Start()
    {
        UpdateTireCount(4);
    }

    public void UpdateTireCount(int newCount)
    {
        int count = newCount - tires.Count;

        if(newCount > tires.Count)
        {
            for(int i = 0; i < count; i++)
            {
                GameObject newTire = Instantiate(tireUIPrefab, transform);
                tires.Add(newTire);
            }
        }
    }

    void Update()
    {
        // normal jitter

        // low health jitter
    }
}
