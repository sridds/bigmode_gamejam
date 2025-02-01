using UnityEngine;

public class DummySpearEnemy : MonoBehaviour
{
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] private GameObject decapitatedHead;
    [SerializeField] private GameObject myBlood;
    [SerializeField] private Animator animator;
    [SerializeField] private bool explodeOnTrigger;
    [SerializeField] private bool makeLogoBloody;

    public void Explode()
    {
        if (hasExploded) return;

        myBlood.SetActive(true);
        myBlood.transform.SetParent(null);
        GameObject g = Instantiate(decapitatedHead, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySound(deathSounds[Random.Range(0, deathSounds.Length - 1)], 1.0f, 0.95f, 1.1f);

        Destroy(gameObject, 0.01f);
        hasExploded = true;

        if (makeLogoBloody) MakeLogoBloody();
    }

    public void MakeLogoBloody()
    {
        FindObjectOfType<PanelController>().MakeLogoBloody();
    }

    public void Yell()
    {
        animator.SetBool("Scream", true);

        if (GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().Play();
        }
    }

    bool yelling;
    bool hasExploded;

    private void Update()
    {
        if (explodeOnTrigger)
        {
            if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 15.0f && !yelling)
            {
                Yell();
                yelling = true;
            }

            if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 3.0f)
            {
                Explode();
            }
        }
    }
}
