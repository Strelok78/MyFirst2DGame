using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Falling : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private float _dropSpeed = 1f;
    [SerializeField] private float _dropTime = 0.05f;
    [SerializeField] private GameScript _gameScript;
    [SerializeField] private GameObject _water;
    RaycastHit2D _hit;
    void Start()
    {
        StartCoroutine(Fall());
    }

    private void Update()
    {
        _hit = Physics2D.Raycast(transform.position, transform.forward, 2);
    }

    IEnumerator Fall()
    {
        while (true)
        {
            Vector2 pos = transform.position;
            pos.y -= _dropSpeed;
            transform.position = pos;
            Destroy(gameObject, _lifeTime);
            yield return new WaitForSeconds(_dropTime);
        }
    }
}