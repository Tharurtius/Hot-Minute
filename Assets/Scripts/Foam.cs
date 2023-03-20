using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foam : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb.velocity = gameObject.transform.up * 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
