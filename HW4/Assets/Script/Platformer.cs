using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
    public class Platformer : MonoBehaviour
    {
        [SerializeField] public float speed;
        [SerializeField] public float jumpSpeed;
        public LayerMask ground;
        private float _move;
        private bool _isGround,_isJump,_isCrouch;
        private int _jumpCount;

        public GameObject text;
        
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private BoxCollider2D _boxCollider2D;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsFall = Animator.StringToHash("IsFall");
        private static readonly int IsGround = Animator.StringToHash("IsGround");
        private static readonly int IsJump = Animator.StringToHash("IsJump");
        private static readonly int IsCrouch = Animator.StringToHash("IsCrouch");

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _isGround = true;
            _isCrouch = false;
            _isJump = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.Space))
            {
                _isJump = true;
            }
            _move=Input.GetAxisRaw("Horizontal");
            _isCrouch = Input.GetKey(KeyCode.S);
        }

        void FixedUpdate()
        {
            _isGround = _boxCollider2D.IsTouchingLayers(ground);
            Anime();
            Move();
            Jump();

            if (transform.position.y<-2.5)
            {
                _rigidbody2D.position = new Vector2(0, 0);
            }
        }
        
        void Move()
        {
            if (!_isCrouch)
            {
                _rigidbody2D.velocity = new Vector2(_move*speed*Time.fixedDeltaTime*50,_rigidbody2D.velocity.y);
            }

            //flip the character after move.
            if (_move!=0)
            {
                transform.localScale = new Vector3(_move, 1, 1);
            }

            if (_isCrouch)
            {
                _isJump = false;
            }
        }

        void Jump()
        {
            if (_isJump&&_jumpCount>0)
            {
                _isJump = false;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,jumpSpeed*Time.fixedDeltaTime*50);
                _jumpCount--;
            }

            //lock jump when run out of count
            if (_jumpCount==0)
            {
                _isJump = false;
            }
        }

        void Anime()
        {
            _animator.SetFloat(Speed,math.abs(_move));
            _animator.SetBool(IsGround,_isGround);
            _animator.SetBool(IsJump,_isJump);
            
            if (_rigidbody2D.velocity.y<=0&&!_isJump)
            {
                _animator.SetBool(IsFall,true);
            }

            if (_isGround)
            {
                _animator.SetBool(IsFall,false);
                _animator.SetBool(IsCrouch,_isCrouch);
            }
        }
        
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer==3)
            {
                _jumpCount = 2;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name=="cherry-1")
            {
                SceneManager.LoadScene("Level1");
            }

            if (col.gameObject.name=="cherry-2")
            {
                col.gameObject.SetActive(false);
                text.SetActive(true);
            }
        }
    }
}
