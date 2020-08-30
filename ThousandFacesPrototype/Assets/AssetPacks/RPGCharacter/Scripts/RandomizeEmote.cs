using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeEmote : MonoBehaviour
{
    // Start is called before the first frame update
    public string[] bools;
    public string current;

    void Start()
    {
        int rand = Random.Range(0, bools.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitFunc()
    {
        yield return new WaitForSeconds(5);
       // int rand = 
    }
}
