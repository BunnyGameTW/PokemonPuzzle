using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerControl : MonoBehaviour
{
    public GameObject [] catcher;
    public GameObject [] checkPoint;
    public GameObject text;
    public float speedX = 10.0f;
    public float speedY = 10.0f;
    public AudioClip put;
    public AudioClip win;
    public GameObject [] effects;
    private bool canRightControl = true;
    private bool canUpControl = true;
    private int controlCounter = 0;
    private Vector3 startPosition;
    private int i = 0;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canUpControl || canRightControl) control();
        else releaseObject();
    }

    private void control()
    {
       
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            canRightControl = false;
            audio.PlayOneShot(put);
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            canUpControl = false;
            audio.PlayOneShot(put);
        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && canUpControl)
        {
           transform.position += new Vector3(0.0f, 0.0f, speedY * Time.deltaTime);
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && canRightControl)
        {
            transform.position += new Vector3(speedX * Time.deltaTime, 0.0f, 0.0f);
        }
        

    }

    private void releaseObject()
    {
        if (i <= catcher.Length - 1)
        {
            //put catcher down
            catcher[i].transform.parent = null;
            catcher[i].GetComponent<Rigidbody>().useGravity = true;
           // audio.PlayOneShot(put);
            //go to zero
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speedX * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPosition) <= 0.01f)
            {
                canRightControl = canUpControl = true;
                i += 1;
              //  Debug.Log("length" + catcher.Length);
                if (i > catcher.Length - 1)
                {
                    canRightControl = canUpControl = false;
                    gameEnd();
                    Debug.Log("game over");

                }
                else catcher[i].SetActive(true);

            }
        }

    }

    public void restartButtonEvent()
    {
        canRightControl = canUpControl = true;
        transform.position = startPosition;
        text.SetActive(false);
        for(int i = 0; i <= catcher.Length - 1; i++)
        {
            catcher[i].GetComponent<Rigidbody>().isKinematic = true;
            catcher[i].SetActive(false);
            catcher[i].GetComponent<Rigidbody>().useGravity = false;
            catcher[i].transform.SetParent(transform);
            catcher[i].transform.localPosition = new Vector3(0, 0.2f, 0);
            catcher[i].transform.rotation = new Quaternion(0, 0, 0, 0);
            catcher[i].GetComponent<Rigidbody>().isKinematic = false;
        }
        catcher[0].SetActive(true);
        i = 0;
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].SetActive(false);
        }
    }
        
    public void giftButtonEvent()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GiftScene");
    }
    public void homeButtonEvent()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
    public void playButtonEvent(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    void gameEnd()
    {
        float totalDistance = 0.0f;
        for (int i = 0; i <= catcher.Length - 1; i++)
        {
            totalDistance += Mathf.Pow(checkPoint[i].transform.position.x - catcher[i].transform.position.x, 2) +
            Mathf.Pow(checkPoint[i].transform.position.z - catcher[i].transform.position.z, 2);
            //node math.pow(i.x - catcher[i].x) + math.pow(i.x - catcher[i].x)
        }
        Debug.Log("totalDistance: " + totalDistance);
        if (totalDistance < 0.1) { text.GetComponent <TMP_Text>().text = "太神啦！你是寶可夢大師！"; }
        else if (totalDistance < 0.5) { text.GetComponent<TMP_Text>().text = "你很強！快靠近巔峰了！"; }
        else if (totalDistance < 3) { text.GetComponent<TMP_Text>().text = "蠻準的！有87%像了！"; }
        else if (totalDistance < 5) { text.GetComponent<TMP_Text>().text = "不錯喔！快對到點了！"; }
        else if (totalDistance < 10) { text.GetComponent<TMP_Text>().text = "呃...再加把勁吧！"; }
        else { text.GetComponent<TMP_Text>().text = "強烈建議你去看看眼科！"; }
        text.SetActive(true);
        audio.PlayOneShot(win);
        for(int i = 0; i < effects.Length; i++)
        {
            effects[i].SetActive(true);
        }
    }
}
