using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] float _comboTime;

    public List<GameObject> enemies;
    private int combo;
    private float comboTimer;
    private bool isComboInitiated;

    public delegate void ComboUpdated();
    public delegate void ComboEnded();
    public ComboUpdated OnComboUpdated;
    public ComboEnded OnComboEnded;

    public int Combo { get { return combo; } }

    public void RegisterEnemyDeath(GameObject enemy)
    {
        enemies.Remove(gameObject);

        UpdateCombo();
        UpdateCount();
    }

    public void UpdateCount()
    {
        text.text = enemies.Count.ToString();
    }

    /// <summary>
    /// Add one to the combo
    /// </summary>
    private void UpdateCombo()
    {
        isComboInitiated = true;
        combo += 1;
        comboTimer = 0.0f;

        OnComboUpdated?.Invoke();
    }

    private void Update()
    {
        if(isComboInitiated && comboTimer > _comboTime)
        {
            isComboInitiated = false;
            combo = 0;
            comboTimer = 0.0f;

            OnComboEnded?.Invoke();
        }
    }
}
