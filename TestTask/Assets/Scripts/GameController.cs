using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject _menu;
    [SerializeField] private TMP_Text _resoultText;
    

    public void ButtonStart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver(string resoult){
        _menu.SetActive(true);
        if (resoult == "win") _resoultText.text = "Вы выиграли!";
        if (resoult == "loss") _resoultText.text = "Вы проиграли!";
    }
}
