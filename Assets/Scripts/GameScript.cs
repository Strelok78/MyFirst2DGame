using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class GameScript : MonoBehaviour
{
    [SerializeField] public GameObject[] prefabs;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private Falling fallingScript;
    [SerializeField] public int _health;
    private bool _alive = true;
    private void Start()
    {
        StartCoroutine(Spawn());
        fallingScript.enabled = true;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }

        if (_alive == true && _health <= 0)
        {
            Lose();
        }
    }
    private void Click()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.name == prefabs[0].name || hit.collider.name == prefabs[1].name)
            {
                Destroy(hit.collider.gameObject);
            }

            if (hit.collider.name == prefabs[2].name)
            {
                Debug.Log("ENEMY!");
                _health--;
                Destroy(hit.collider.gameObject);
            }
        }
    }
    public void Lose()
    {
        StopAllCoroutines();
        fallingScript.StopAllCoroutines();
        fallingScript.enabled = false;
        _alive = false;
        Debug.Log("YouLose");
    }
    IEnumerator Spawn()
    {
        while (true)
        {            
            float xPos = Random.Range(_minX, _maxX);
            Vector2 pos = new(xPos, transform.position.y);
            GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Length)], pos, Quaternion.identity, transform);
            obj.name = obj.name.Replace("(Clone)", "").Trim();
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}
