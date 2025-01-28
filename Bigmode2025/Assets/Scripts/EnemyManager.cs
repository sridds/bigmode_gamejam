using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public float _comboTime;

    [SerializeField]
    private ComboValue _comboPrefab;

    [SerializeField]
    private Sprite[] _comboSprites;

    [SerializeField]
    private Sprite _xSpr;

    [SerializeField]
    private float _spacing = 0.105f;

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
        Vector2 spawnPos = enemy.transform.position;
        enemies.Remove(enemy);

        Destroy(enemy.gameObject);

        UpdateCombo();
        UpdateCount();

        SpawnComboMeter(spawnPos);
    }

    public void UpdateCount()
    {
        text.text = enemies.Count.ToString();
    }

    private void SpawnComboMeter(Vector2 pos)
    {
        if (combo > 1)
        {
            ComboValue xSpr = Instantiate(_comboPrefab, pos, Quaternion.identity);
            xSpr.UpdateSprite(_xSpr);
            xSpr.DelayedHop(0.0f);

            if (combo > 9)
            {
                ComboValue tensSpr = Instantiate(_comboPrefab, new Vector2(pos.x + _spacing, pos.y), Quaternion.identity);
                ComboValue onesSpr = Instantiate(_comboPrefab, new Vector2(pos.x + (_spacing * 2), pos.y), Quaternion.identity);

                tensSpr.UpdateSprite(_comboSprites[Mathf.FloorToInt((float)combo / 10)]);
                onesSpr.UpdateSprite(_comboSprites[combo % 10]);

                tensSpr.DelayedHop(0.1f);
                onesSpr.DelayedHop(0.2f);
            }
            else
            {
                ComboValue onesSpr = Instantiate(_comboPrefab, new Vector2(pos.x + _spacing, pos.y), Quaternion.identity);
                onesSpr.UpdateSprite(_comboSprites[combo % 10]);

                onesSpr.DelayedHop(0.1f);
            }

            FindObjectOfType<PlayerMovement>().AddSpeedBoost();
        }
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
            StopCombo();
        }
        else if (isComboInitiated)
        {
            comboTimer += Time.deltaTime;
        }
    }

    public void StopCombo()
    {
        Debug.Log("Combo ended");

        OnComboEnded?.Invoke(combo);
        isComboInitiated = false;
        combo = 0;
        comboTimer = 0.0f;
    }
}
