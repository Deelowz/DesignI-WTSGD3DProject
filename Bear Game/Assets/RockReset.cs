using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockReset : MonoBehaviour
{

    public Item[] rock;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rock.Length; i++)
        {
            rock[i].amount = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
