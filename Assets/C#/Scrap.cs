using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LerpPosition(FindObjectOfType<Player>().transform, 0.7f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LerpPosition(Transform target, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (true) // Infinite loop to keep following the target
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 0.1f)
            {
                target.GetComponent<Player>().scrap += 1;
                break;
            }

            float t = time / duration;
            t = t * t * (3f - 2f * t); // Smooth Step easing formula
            transform.position = Vector3.Lerp(startPosition, target.position, t);
            time += Time.deltaTime;
            if (time >= duration)
            {
                startPosition = transform.position;
                time = 0;
            }
            yield return null;
        }

        Destroy(this.gameObject);
    }

}
