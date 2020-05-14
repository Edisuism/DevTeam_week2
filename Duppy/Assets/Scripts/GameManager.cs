using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Set in inspector
    public GameObject completeLevelUI;
    public GameObject deathUI;

    private GameObject player;
    public AudioManager audioManager;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void GameOver()
    {
        RemoveGhosts();
        StartCoroutine("ShowDeathUI");
        //TODO: Make character look forward, perform death animation
    }

    public void Win()
    {
        RemoveGhosts();
        audioManager.Play("escape");
        completeLevelUI.SetActive(true);
        StartCoroutine("LoadMenu");
    }

    private void RemoveGhosts()
    {
        GameObject[] ghostArray = GameObject.FindGameObjectsWithTag("Ghost");
        foreach(GameObject ghost in ghostArray)
        {
            Destroy(ghost);
        }
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(7.0f);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator ShowDeathUI()
    {
        yield return new WaitForSeconds(7.6f);
        deathUI.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        deathUI.GetComponentInChildren<Image>().color = Color.black;
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
