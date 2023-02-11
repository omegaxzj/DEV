using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Rigid : MonoBehaviour
{
    public Rigidbody2D legLeft;
    public Rigidbody2D legRight;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _movement;
    public float speed;
    public GameObject end;
    public GameObject hint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        legLeft.AddForce(_movement* (speed * Time.fixedDeltaTime),ForceMode2D.Impulse);
        legRight.AddForce(_movement* (-speed * Time.fixedDeltaTime),ForceMode2D.Impulse);
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name==($"Flag"))
        {
            hint.SetActive(false);
            end.SetActive(true);
        }
    }
}
