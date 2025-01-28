using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class ComboMeterUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _comboMeterText;

    private EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.OnComboUpdated += ComboUpdate;
        enemyManager.OnComboEnded += ComboEnded;

        // hide text
        _comboMeterText.text = "";
    }

    private void ComboUpdate(int combo)
    {
        StopAllCoroutines();
        _comboMeterText.rectTransform.DOKill(true);
        _comboMeterText.rectTransform.DOShakeAnchorPos(0.3f, 10, 50, 90, false, true, ShakeRandomnessMode.Full);
    }

    private void ComboEnded(int combo)
    {
        if (combo < 2) return;

        StartCoroutine(IComboEnded());
    }

    private IEnumerator IComboEnded()
    {
        _comboMeterText.text = $"COMBO ENDED";

        yield return new WaitForSeconds(3.0f);

        _comboMeterText.text = $"";

        yield return null;
    }
}
