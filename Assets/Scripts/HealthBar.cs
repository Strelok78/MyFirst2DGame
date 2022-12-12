using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    [SerializeField] List<GameObject> heartContainers;
    int totalHearts;
    float currentHearts;
    HeartContainer currentContainer;

    public HealthBar instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        heartContainers = new List<GameObject>();        
    }

    public void SetupHearts(int heartIn)
    {
        heartContainers.Clear();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        totalHearts = heartIn;
        currentHearts = (float)totalHearts;

        for(int i = 0; i < totalHearts; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, transform);
            heartContainers.Add(newHeart);
            if(currentContainer != null)
            {
                currentContainer.next = newHeart.GetComponent<HeartContainer>();
            }
            currentContainer = newHeart.GetComponent<HeartContainer>();
        }
        currentContainer = heartContainers[0].GetComponent<HeartContainer>();
    }
    public void RemoveHearts(float healthDown)
    {
        currentHearts -= healthDown;
        if (currentHearts < 0)
        {
            currentHearts = 0f;
        }
        currentContainer.SetHeart(currentHearts);
    }

    public void SetCurrentHealth(float health)
    {
        currentHearts= health;
        currentContainer.SetHeart(currentHearts);
    }

    public void AddHearts(float healthUp)
    {
        currentHearts += healthUp;
        if (currentHearts > totalHearts) 
        {
            currentHearts = (float)totalHearts;
        }
        currentContainer.SetHeart(currentHearts);
    }

    public void AddContainer()
    {
        GameObject newHeart = Instantiate(heartPrefab, transform);
        currentContainer= heartContainers[heartContainers.Count-1].GetComponent<HeartContainer>();
        heartContainers.Add(newHeart);

        if (currentContainer != null)
        {
            currentContainer.next = newHeart.GetComponent<HeartContainer>();
        }

        currentContainer = heartContainers[0].GetComponent<HeartContainer>();

        totalHearts++;
        currentHearts= totalHearts;
        SetCurrentHealth(currentHearts);
    }
}
