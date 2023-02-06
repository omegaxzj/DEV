using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Script
{
    public class Character : MonoBehaviour
    {
        [SerializeField] public float speed;
        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private int _direction;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _movement;
        private static Vector2 _position;
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int LastX = Animator.StringToHash("LastX");
        private static readonly int LastY = Animator.StringToHash("LastY");

        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            //初始化正脸位置
            _animator.SetFloat(Horizontal,0);
            _animator.SetFloat(Vertical,1);
        }

        // Update is called once per frame
        void Update()
        {
            //获取输入参数
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _animator.SetFloat(Horizontal,_movement.x);
            _animator.SetFloat(Vertical,_movement.y);
            _animator.SetFloat(Speed,_movement.sqrMagnitude);
            _position = _rigidbody2D.position;
            //保留最后朝向并重置另一轴参数
            if (_movement.x!=0)
            {
                _animator.SetFloat(LastX,_movement.x);
                _animator.SetFloat(LastY,0);
            }
            if (_movement.y!=0)
            {
                _animator.SetFloat(LastY,_movement.y);
                _animator.SetFloat(LastX,0);
            }
        }

        public static Vector2 FindPosition()
        {
            return _position;
        }
        
        private void FixedUpdate()
        {
            //移动
            _rigidbody2D.MovePosition(_rigidbody2D.position+_movement * (speed * Time.fixedDeltaTime));
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag($"Item"))
            {
                Debug.Log("Hit");
            }
        }
    }
}
