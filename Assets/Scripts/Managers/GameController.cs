using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour{
    

    board m_gameBoard;

    Spawner m_spawner;

    Shape m_activeShape;

    float m_timeToDrop;

    bool gameOver = false;

    public GameObject m_gameOverPanel;

    [Range(0.02f,1f)] public float m_dropInterval = 0.5f;


    float m_timeToNextKeyRight;

    [Range(0.02f,1f)] public float m_keyRepeatRateRight = 0.25f;

    float m_timeToNextKeyLeft;

    [Range(0.02f,1f)] public float m_keyRepeatRateLeft = 0.25f;

    float m_timeToNextKeyDown;

    [Range(0.01f,1f)] public float m_keyRepeatRateDown = 0.01f;

    float m_timeToNextKeyRotate;

    [Range(0.02f,1f)] public float m_keyRepeatRateRotate = 0.15f;

    SoundManager m_soundManager;

    public iconToggle m_rotIconToggle;

    bool m_clockwise = true;

    void Start(){
        if(m_gameOverPanel){
            m_gameOverPanel.SetActive(false);
        }

        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        m_soundManager = GameObject.FindObjectOfType<SoundManager>();

        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
        m_timeToNextKeyLeft = Time.time + m_keyRepeatRateLeft;
        m_timeToNextKeyRight = Time.time + m_keyRepeatRateRight;
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

        if(m_spawner){
             if(m_activeShape == null){
                m_activeShape = m_spawner.SpawnShape();    
             }
            m_spawner.transform.position = VectorF.Round(m_spawner.transform.position);
        }
    }

    void playSound(AudioClip sound, float volMultiplier = 1){
        if(m_soundManager.m_fxEnabled && sound){
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier,0.05f,1f));
        }
    }

    void Movement(string direction,Shape.MoveDirection Movement, Shape.MoveDirection Block){
        Movement();

        if(!m_gameBoard.IsValidPosition(m_activeShape)){
            Block();
            playSound(m_soundManager.m_errorSound);
        }else{
            playSound(m_soundManager.m_moveSound);
        }
    }

    bool checkButton(string direction,float nextKeyTimer, bool onlyButtonDown = false){
        bool biggetTimer = Time.time > nextKeyTimer;
        bool pressingButton = Input.GetButton(direction);
        bool buttonDown = Input.GetButtonDown(direction);

        if(!onlyButtonDown){
            return (pressingButton && biggetTimer || buttonDown);
        }else{
            return buttonDown && biggetTimer;
        }
    }

    void LandShape(){

        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyLeft = Time.time;
        m_timeToNextKeyRight = Time.time;
        m_timeToNextKeyRotate = Time.time;


        m_activeShape.MoveUp();
        
        if(!Input.GetButton("MoveDown")){
            m_gameBoard.StoreShapeInGrid(m_activeShape);
            m_activeShape = m_spawner.SpawnShape();
        }else{
            playSound(m_soundManager.m_dropSound);
        }

        m_gameBoard.ClearAllRows();

        if(m_gameBoard.m_completedRows > 0){
            if(m_gameBoard.m_completedRows > 1){
                AudioClip randomVocal = m_soundManager.GetRamdomClip(m_soundManager.m_vocalSound);
                playSound(randomVocal);
            }
            playSound(m_soundManager.m_clearRowSound);
        }
    }

    public void Restart(){
        Application.LoadLevel(Application.loadedLevel);
    }

        void PlayerInput(){
        if (checkButton("MoveRight", m_timeToNextKeyRight)) {
            m_timeToNextKeyRight = Time.time + m_keyRepeatRateRight;
            Movement("MoveRight", m_activeShape.MoveRight, m_activeShape.MoveLeft);   
        }
        
        if(checkButton("MoveLeft",m_timeToNextKeyLeft)){
            m_timeToNextKeyLeft = Time.time + m_keyRepeatRateLeft;
            Movement("MoveLeft", m_activeShape.MoveLeft,m_activeShape.MoveRight);
        }
        
        if(checkButton("Rotate",m_timeToNextKeyRotate,true)){
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
            m_activeShape.RotateClockWise(m_clockwise);

            if(!m_gameBoard.IsValidPosition(m_activeShape)){
                m_activeShape.RotateClockWise(!m_clockwise);
                playSound(m_soundManager.m_errorSound);
            }else{
                playSound(m_soundManager.m_moveSound);
            }
        }

        if(Input.GetButtonDown("ToggleRot")){
            ToggleRotDirection();
        }
        
        if(Input.GetButton("MoveDown") && Time.time > m_timeToNextKeyDown || Time.time > m_timeToDrop){
            m_timeToDrop = Time.time + m_dropInterval;
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
            m_activeShape.MoveDown();

            if(!m_gameBoard.IsValidPosition(m_activeShape) ){
                if(m_gameBoard.IsOverLimit(m_activeShape)){
                    GameOver();
                }else {
                    LandShape();
                }
                
            }
        }
  }

    void GameOver(){
      m_activeShape.MoveUp();
      gameOver = true;
      playSound(m_soundManager.m_gameOverSound);
      playSound(m_soundManager.m_gameOverVocal);
      if(m_gameOverPanel) m_gameOverPanel.SetActive(true);
    }

    public void ToggleRotDirection(){
        m_clockwise = !m_clockwise;
        if(m_rotIconToggle){
            m_rotIconToggle.ToggleIcon(m_clockwise);
        }
    }

    void Update(){
        if(m_gameBoard && m_spawner && m_activeShape && 
            !gameOver && m_soundManager){
            PlayerInput();
        }
    }
}
