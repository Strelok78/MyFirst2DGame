using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;


public class GameScript : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public GameObject[] prefabs;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private float _dropSpeed = 1f;

    [Header("Player + UI")]
    [SerializeField] public int _health;
    [SerializeField] private Text _pointsText;
    [SerializeField] private HealthBar _hBar;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Button _restartButton;

    [Header("Audio")]
    [SerializeField] AudioSource _bangSound;
    [SerializeField] AudioSource _popSound;
    [SerializeField] AudioSource _endGameSound;
    
    private int _points;
    private bool _alive = true;

    private void Awake()
    {
        _restartButton.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false); 
    }
    //setups UI and starts the Spawn Coroutine
    private void Start()
    {
        StartCoroutine(Spawn());
        _points = 0;
        _pointsText.text = $"Points: {_points}";
        _hBar.instance.SetupHearts(_health);
    }

    //catches button clicks and updates the score 
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Click();
        }
        _pointsText.text = $"Points: {_points}";
        if (_alive == true && _health <= 0) {
            Lose();
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Checks if the correct game object was clicked
    private void Click()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.name == prefabs[0].name)
            {
                Destroy(hit.collider.gameObject);
                _points++;
                _popSound.Play();
            }

            if (hit.collider.name == prefabs[1].name)
            {
                Destroy(hit.collider.gameObject);
                _points += 2;
                _popSound.Play();
            }

            if (hit.collider.name == prefabs[2].name)
            {
                Debug.Log($"It's an Enemy! You have {_health} Helth points left!");
                _health--;
                _hBar.instance.RemoveHearts(1);
                Destroy(hit.collider.gameObject);
                _bangSound.Play();
            }
        }
    }

    //Deletes all gameObbjects and stops all coroutines whe gameEnds
    public void Lose()
    {
        StopAllCoroutines();
        _alive = false;
        _pointsText.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(true);
        _gameOverText.gameObject.SetActive(true);
        _gameOverText.text = $"You Lose!\r\nYour  score: {_points}";
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        _endGameSound.Play();
    }

    //Creates new objects and starts fall coroutine for each object
    IEnumerator Spawn()
    {
        while (true)
        {            
            float xPos = Random.Range(_minX, _maxX);
            Vector2 pos = new(xPos, transform.position.y);
            GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Length)], pos, Quaternion.identity, transform);
            obj.name = obj.name.Replace("(Clone)", "").Trim();
            StartCoroutine(Fall(obj));
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    //Makes objects fall - avoids fps dependece 
    IEnumerator Fall(GameObject newObj)
    {
        while (newObj != null)
        {
            Vector2 pos = newObj.transform.position;
            pos.y -= _dropSpeed * Time.deltaTime;
            newObj.transform.position = pos;
            StartCoroutine(Destr(newObj));
            yield return null;
        }
    }

    //Checks which object was destroyed by the time
    IEnumerator Destr(GameObject nObj)
    {
        yield return new WaitForSeconds(_lifeTime);
        if (nObj != null)
        {
            if (nObj.name != prefabs[2].name)
            {
                Destroy(nObj);
                _health--;
                _hBar.instance.RemoveHearts(1);
                Debug.Log($"You lost coin or apple, be careful! You have only: {_health} Health points left!");
            }
            else
            {
                Destroy(nObj);
                Debug.Log("Enemy goes to the seabed! Good job!");
            }
        }        
    }


}
