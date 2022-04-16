using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    Titan, Gun, GunSpeacial
}
public class Item : MonoBehaviour
{
    public static Item instance;

    public GameObject prefab;

    public ItemTypes itemType;

    public float multiplier = 1.4f;

    public bool isSpeacial;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && itemType != ItemTypes.GunSpeacial)
        {
            GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity); Destroy(obj, 0.5f);
            AudioManager.Instance.CaseSoundSFX("picking");

            StartCoroutine(PowerUp(collision));
        }
        else if (collision.tag == "Player" && itemType == ItemTypes.GunSpeacial)
        {
            GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity); Destroy(obj, 0.5f);
            AudioManager.Instance.CaseSoundSFX("picking");

            PowerUpSpeacial(collision);
        }
    }

    IEnumerator PowerUp(Collider2D player)
    {
        if(itemType == ItemTypes.Titan)
        {
            player.transform.localScale *= multiplier;
            player.GetComponent<SpriteRenderer>().color = Color.yellow;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            PlayerController.instance.isTitan = true;
            yield return new WaitForSeconds(5.0f);
            player.transform.localScale /= multiplier;
            player.GetComponent<SpriteRenderer>().color = Color.white;
            PlayerController.instance.isTitan = false;
            Destroy(gameObject);
        }
        else
        {
            player.GetComponent<SpriteRenderer>().color = Color.cyan;
            PlayerController.instance.isShooting = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(40.0f);
            player.GetComponent<SpriteRenderer>().color = Color.white;
            PlayerController.instance.isShooting = false;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
            gameObject.SetActive(false);
        }
    }

    void PowerUpSpeacial(Collider2D player)
    {
        player.GetComponent<SpriteRenderer>().color = Color.red;
        PlayerController.instance.isShooting = true;
        isSpeacial = true;
        GameObject.Find("Player").GetComponent<Shoot>().startShootBtwTime = 0.05f;
        Destroy(gameObject);
    }
}
