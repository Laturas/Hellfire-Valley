using UnityEngine;

public class tower_jack : MonoBehaviour
{
    public int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void hurt()
    {
        health--;
    }
}
