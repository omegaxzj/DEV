using System;
using UnityEngine;

namespace Script
{
    public class Cloud : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int IsTrigger = Animator.StringToHash("IsTrigger");

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetBool(IsTrigger,false);
        }

        // Update is called once per frame
        void Update()
        {
            if (_animator.GetBool(IsTrigger))
            {
                Invoke(nameof(Disappear),0.6f);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            try
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    _animator.SetBool(IsTrigger,true);
                    Player.CloudNum--;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void Disappear()
        {
            Destroy(gameObject);
        }
    }
}
