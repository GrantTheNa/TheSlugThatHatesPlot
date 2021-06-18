using UnityEngine;
using UnityEngine.UI;
public class ItemCollection : MonoBehaviour
{

    private float tomato = 0;
    private float leaf = 0;
    private float key = 0;
    private float score = 0;

    private float tomatoWorth = 100;
    private float leafWorth = 10;

    public Text textTomato;
    public Text textLeaf;
    public Text textKey;
    public Text textScore;
    public bool allKeysCollected;

    void Start()
    {
        textTomato.text = tomato.ToString();
        textLeaf.text = leaf.ToString();
        textKey.text = key.ToString();
        textScore.text = score.ToString();

    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tomato"))
        {
            tomato++;
            score += tomatoWorth;

            textTomato.text = tomato.ToString();
            textScore.text = score.ToString();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Leaf"))
        {
            leaf ++;
            score += leafWorth;

            textLeaf.text = leaf.ToString();
            textScore.text = score.ToString();

            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Key"))
        {
            key ++;
            textKey.text = key.ToString();

            Destroy(other.gameObject);
            if (key == 5)
            {
                allKeysCollected = true;
                Debug.Log("All Keys have been collected");
            }

        }

    }
}
