using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField]float timer = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

}
