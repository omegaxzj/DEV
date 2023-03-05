using System;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script
{
    public class Player : MonoBehaviour
    {
        [SerializeField] public float speed;
        [SerializeField] public float jumpSpeed;
        private float _move;
        private bool _isJump;
        private int _counter;
        private bool _ifEnd;
        
        public static int CloudNum;
        
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private BoxCollider2D _boxCollider2D;
        private AudioSource _audioSource;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsFall = Animator.StringToHash("IsFall");
        private static readonly int IsJump = Animator.StringToHash("IsJump");

        public GameObject cloud1;
        public GameObject cloud2;
        public GameObject cloud3;
        public GameObject cloud4;
        public GameObject endline;
        public TextMeshProUGUI score;
        public AudioClip jump;
        public AudioClip die;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _isJump = false;
            _ifEnd = false;
            CloudNum = 0;
            _counter = 0;
        }

        // Update is called once per frame
        void Update()
        {
            _move=Input.GetAxisRaw("Horizontal");
        }

        void FixedUpdate()
        {
            CloudManager();
            Anime();
            Move();
            Jump();
            score.text = _counter+"";
            score.color=Color.red;
            
            //restart when fall
            if (transform.position.y<-2.5)
            {
                endline.SetActive(true);
                End();
            }
        }

        void End()
        {
            if (!_ifEnd)
            {
                _ifEnd = true;
                _audioSource.clip = die;
                _audioSource.Play();
            }
            Invoke(nameof(Reload),2.5f);
        }
        
        void Move()
        {
            if (_rigidbody2D.position.x is <= 3 and >= -3)
            {
                _rigidbody2D.velocity = new Vector2(_move*speed*Time.fixedDeltaTime*50,_rigidbody2D.velocity.y);
            }
            
            //flip the character after move.
            if (_move!=0)
            {
                transform.localScale = new Vector3(_move, 1, 1);
            }
        }

        void Jump()
        {
            if (!_isJump) return;
            _isJump = false;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,jumpSpeed*Time.fixedDeltaTime*50);
            _audioSource.clip = jump;
            _audioSource.Play();
        }

        void Anime()
        {
            _animator.SetFloat(Speed,math.abs(_move));
            _animator.SetBool(IsJump,_isJump);

            if (_rigidbody2D.velocity.y<=0&&!_isJump)
            {
                _animator.SetBool(IsFall,true);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("cloud"))
            {
                _isJump = true;
                _animator.SetBool(IsFall,false);
                _animator.SetBool(IsJump,_isJump);
                _counter++;
            }
        }

        private void Reload()
        {
            _ifEnd = false;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        private void CloudManager()
        {
            while (CloudNum<20)
            {
                //generate position
                float x = Random.Range(-2.5f,2.5f);
                float y = Random.Range(-1.8f, 2f);
                int type = Random.Range(0,4);
                switch (type)
                {
                    case 0:
                        Instantiate(cloud1,new Vector3(x,y,0),Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(cloud2,new Vector3(x,y,0),Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(cloud3,new Vector3(x,y,0),Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(cloud4,new Vector3(x,y,0),Quaternion.identity);
                        break;
                }
                CloudNum++;
            }
        }
    }
}
