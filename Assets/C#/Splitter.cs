using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{
    public GameObject splitEnemy;
    public int splitAmmout;
    
    public void Split()
    {
        for (int i = 0; i < splitAmmout; i++)
        {
            var newEnemy = Instantiate(splitEnemy);
            newEnemy.transform.position = transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        }
    }
}
