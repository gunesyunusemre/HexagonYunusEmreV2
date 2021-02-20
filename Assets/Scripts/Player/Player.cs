using System;
using System.Collections;
using System.Collections.Generic;
using CoreScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player _player;

    #region Variables

    #region PlayerID
    [SerializeField]private int moveCounter = 0;
    [SerializeField]private int score = 0;
    [SerializeField]private int highScore = 0;
    [SerializeField] private int bombCounter = 0;
    #endregion

    #region Capsule Methods

    public int MoveCounter
    {
        get => moveCounter;
        set => moveCounter = value;
    }
    
    public int Score
    {
        get => score;
        set => score = value;
    }
    
    public int HighScore
    {
        get => highScore;
        set => highScore = value;
    }
    
    public int BombCounter
    {
        get => bombCounter;
        set => bombCounter = value;
    }

    #endregion

    [SerializeField]private bool isGameover = false;

    #endregion

    #region MonoBehaviour
    private void OnEnable()
    {
        if (_player is null || _player!=this)
        { _player = this; }

        //PlayerPrefs.DeleteAll();

        //bombCounter=PlayerPrefs.GetInt("bomb");
        score= PlayerPrefs.GetInt("score");
        moveCounter=PlayerPrefs.GetInt("moveCounter");
        highScore= PlayerPrefs.GetInt("highScore");

        Cell.GameOverEventHandler += GameOver;
        BoardController.QuickSaveEventHandler += QuickSave;
    }

    private void OnDisable()
    {
        Cell.GameOverEventHandler -= GameOver;
        BoardController.QuickSaveEventHandler -= QuickSave;
    }
    #endregion

    #region Save

    private void QuickSave()
    {
        if (isGameover)
        {
            PlayerPrefs.SetInt("highScore", score);
            PlayerPrefs.Save();
            return;
        }
        
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("moveCounter", moveCounter);
        //Debug.Log(PlayerPrefs.GetInt("bomb"));
        //PlayerPrefs.SetInt("bomb", bombCounter);
        //Debug.Log(PlayerPrefs.GetInt("bomb"));
        PlayerPrefs.Save();
    }
    

    #endregion

    #region GameOver
    private void GameOver()
    {
        Debug.Log("GameOver!");
        isGameover = true;
        
        if (score>PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("highScore", highScore);
        }
        else
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("highScore", score);
        }
        
        Debug.Log(PlayerPrefs.GetInt("highScore"));
        PlayerPrefs.Save();
    }

    #endregion

}
