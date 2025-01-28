using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public float _comboTime;
    public int maxCombo = 20;

    public List<GameObject> enemies;
    private int combo;
    private float comboTimer;
    private bool isComboInitiated;

    public delegate void ComboUpdated(int combo);
    public delegate void ComboEnded(int combo);
    public ComboUpdated OnComboUpdated;
    public ComboEnded OnComboEnded;

    public int Combo { get { return combo; } }
    public int EnemiesRemaining { get { return enemies.Count; } }

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

        OnComboUpdated?.Invoke(combo);
    }

    private void Update()
    {
        if(isComboInitiated && comboTimer > _comboTime)
        {
            OnComboEnded?.Invoke(combo);

            isComboInitiated = false;
            combo = 0;
            comboTimer = 0.0f;
        }
        else if (isComboInitiated)
        {
            comboTimer += Time.deltaTime;
        }
    }
}
