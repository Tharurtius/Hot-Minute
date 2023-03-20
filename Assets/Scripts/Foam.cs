using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foam : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb.velocity = (transform.right + transform.up * Random.value * 0.25f) * 10f;//normalise before multiplying by 10?
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Time.deltaTime * Vector3.one;
    }
}
