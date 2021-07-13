using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ItemCollection : MonoBehaviour
{
    //Item Score
    private float tomato = 0;
    private float leaf = 0;
    private float key = 0;
    private float score = 0;


    //Item Worth for Score
    private float tomatoWorth = 100;
    private float leafWorth = 10;


    //Text for Items
    public Text textTomato;
    public Text textLeaf;
    public Text textKey;
    public Text textScore;

    //Game code effecting outside world
    public bool allKeysCollected;
    public bool htpMenu;
    public GameObject menu;
    public GameObject door;


    //Timer UI
    public Text timeTimer;
    private float timeSeconds;
    private int timeMinute;
    private int timeHour;
    private bool timeBool;

    //end game UI
    private bool endGame = false;
    public Text endScore;
    public Text endScoreAddUp;
    public float endGameCredit;
    public GameObject endGameMenu;
    public bool isOnEndGameMenu;
    public bool endGameMenuReload = false;

    void Start()
    {
        textTomato.text = tomato.ToString();
        textLeaf.text = leaf.ToString();
        textKey.text = key.ToString();
        textScore.text = score.ToString();
        htpMenu = true;
        timeBool = false;
        endGameMenu.SetActive(false);
        isOnEndGameMenu = false;
        endGame = false;


    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            timeBool = true;
            if (htpMenu == false)
            {
                menu.SetActive(true);
                htpMenu = true;

            }
            else if (htpMenu == true)
            {
                menu.SetActive(false);
                htpMenu = false;
            }
        }

        if (timeBool == true)
        {
            Timer();
        }
        if (endGame == true)
        {
            if (isOnEndGameMenu == false)
            {
                EndGame();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }

    public void Timer()
    {
        timeSeconds += Time.deltaTime;
        //timeTimer.text = timeHour +":"+ timeMinute +":"+(int)timeSeconds + ":";
        timeTimer.text = timeMinute + ":" + (int)timeSeconds + "";
        if (timeSeconds >= 60)
        {
            timeMinute++;
            timeSeconds = 0;
        }
        if (timeSeconds <= 9)
        {
            timeTimer.text = timeMinute + ":" + "0" +(int)timeSeconds ;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (endGame == false)
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
                   Destroy(door);
                }
        
            }
            else if (other.gameObject.CompareTag("End"))
            {
                timeBool = false;
                endGame = true;
            }
        }


    }

    public void EndGame()
    {
        if (endGameMenuReload == false)
        {
            endGameMenuReload = true;
            endGameMenu.SetActive(true);
            isOnEndGameMenu = true;
            if (timeMinute == 3 && timeSeconds <= 10)
            {
                endGameCredit = 50;
            }
            else if (timeMinute == 2 && timeSeconds >= 50)
            {
                endGameCredit = 200;
            }
            else if (timeMinute == 2 && timeSeconds >= 10)
            {
                endGameCredit = 500;
            }
            else if (timeMinute == 2 && timeSeconds <= 10)
            {
                endGameCredit = 700;
            }
            else if (timeMinute == 1 && timeSeconds >= 50)
            {
                endGameCredit = 800;
            }
            else if (timeMinute == 1 && timeSeconds >= 40)
            {
                endGameCredit = 900;
            }
            else if (timeMinute == 1 && timeSeconds >= 30)
            {
                endGameCredit = 1000;
            }
            else if (timeMinute == 1 && timeSeconds >= 10)
            {
                endGameCredit = 1100;
            }
            else if (timeMinute == 1 && timeSeconds >= 5)
            {
                endGameCredit = 1200;
            }
            else if (timeMinute == 1 && timeSeconds <= 5)
            {
                endGameCredit = 1250;
            }
            else if (timeMinute == 0)
            {
                endGameCredit = 1300;
            }
            Debug.Log("end");
            endGameMenu.SetActive(true);
            endScore.text = score + " + Bonus : " + endGameCredit.ToString();
            score = score + endGameCredit;
            endScoreAddUp.text = score.ToString();
        }
    }

}
