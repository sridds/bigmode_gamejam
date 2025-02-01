using UnityEngine;
using System.Collections;
public class SteamAchievement : MonoBehaviour
{
    [SerializeField] AudioClip soundEffect;
    EnemyManager enemyManager;
    bool displayedAchievement = false;
    float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (enemyManager.enemies.Count <= 7 && !displayedAchievement && timer >= 1)
        {
            displayedAchievement = true;
            StartCoroutine(DisplayAchievement());
        }
    }
    IEnumerator DisplayAchievement()
    {
        float elapsed = 0.0f;

        AudioManager.instance.PlaySound(soundEffect, 0.8f, 1, 1);


        Vector3 startPos = transform.localPosition;
        Vector3 endPos = new Vector3(startPos.x, startPos.y + 53, startPos.z);
        while (elapsed < 0.5f)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / 0.5f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3);

        elapsed = 0.0f;

        while (elapsed < 0.5f)
        {
            transform.localPosition = Vector3.Lerp(endPos, startPos, elapsed / 0.5f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

}
