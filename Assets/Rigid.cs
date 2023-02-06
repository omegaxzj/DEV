using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Rigid : MonoBehaviour
{
    public Rigidbody2D armLeft;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _movement;
    public AudioClip jump;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _rigidbody2D.AddForce(_movement* (speed * Time.fixedDeltaTime),ForceMode2D.Impulse);
    }
}
