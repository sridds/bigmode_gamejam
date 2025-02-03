using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    //[SerializeField] TMP_Text text;
    public float _comboTime;

    [SerializeField]
    private ComboValue _comboPrefab;

    [SerializeField]
    private Sprite[] _comboSprites;

    [SerializeField]
    private Sprite _xSpr;

    [SerializeField]
    private AudioClip _comboSound;

    [SerializeField]
    private float pitchIncrement = 0.06f;

    [SerializeField]
    private float minPitch = 0.9f;

    [SerializeField]
    private float maxPitch = 2.5f;

    [SerializeField]
    private float _spacing = 0.105f;
    [SerializeField]
    private AudioSource _source;

    [SerializeField]
    private bool _endLevelOnKillingEverythingOrShouldILetSomethingElseHandleItLikeMaybeTheSpartacusScriptInstead = true;

    public List<GameObject> enemies;
    private int combo;
    private float comboTimer;
    private bool isComboInitiated;

    public delegate void ComboUpdated(int combo);
    public delegate void ComboEnded(int combo);
    public ComboUpdated OnComboUpdated;
    public ComboEnded OnComboEnded;

    [SerializeField] Transform enemyContainer;
    public int Combo { get { return combo; } }
    public int EnemiesRemaining { get { return enemies.Count; } }
    private float pitch;

    public static EnemyManager instance;

    private void Start()
    {
        pitch = minPitch;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void RegisterEnemyDeath(GameObject enemy)
    {
        Vector2 spawnPos = enemy.transform.position;
        enemies.Remove(enemy);

        Destroy(enemy.gameObject);

        UpdateCombo();
        UpdateCount();

        SpawnComboMeter(spawnPos);

        if (enemyContainer.childCount-1 == 0 && _endLevelOnKillingEverythingOrShouldILetSomethingElseHandleItLikeMaybeTheSpartacusScriptInstead)
        {
            GameObject.Find("LevelTransition").GetComponent<LevelTransitions>().StartTransition();
            relaxIllHandleIt = true;
        }
    }

    private bool relaxIllHandleIt;

    public void UpdateCount()
    {
        //text.text = (enemyContainer.childCount -1).ToString();
    }

    private void SpawnComboMeter(Vector2 pos)
    {
        if (combo > 1)
        {
            pitch += pitchIncrement;
            if (pitch > maxPitch) pitch = maxPitch;

            _source.pitch = pitch;
            _source.PlayOneShot(_comboSound);

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

    bool HELPME;

    private void LateUpdate()
    {
        if (!relaxIllHandleIt && FindObjectsOfType<EnemyHealthScript>().Length == 0 && !_endLevelOnKillingEverythingOrShouldILetSomethingElseHandleItLikeMaybeTheSpartacusScriptInstead && !HELPME)
        {

            Debug.Log("i am the angry pumkin");
            GameObject.Find("LevelTransition").GetComponent<LevelTransitions>().StartTransition();
            HELPME = true;
        }

    }

    public void StopCombo()
    {
        Debug.Log("Combo ended");

        OnComboEnded?.Invoke(combo);
        isComboInitiated = false;
        combo = 0;
        comboTimer = 0.0f;
        pitch = minPitch;
    }
}
