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
        GameObject g = Instantiate(decapitatedHead, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySound(deathSounds[Random.Range(0, deathSounds.Length - 1)], 1.0f, 0.95f, 1.1f);

        Destroy(gameObject);
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

            if (GameObject.FindGameObjectWithTag("Player").transform.position.x > transform.position.x)
            {
                Explode();
            }
        }
    }
}
