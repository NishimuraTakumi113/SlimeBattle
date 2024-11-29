using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEffectPlayer : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform;
    public AudioSource audioSource;
    public AudioClip smash;
    public AudioClip death;
    public AudioClip tackle;
    private bool isSmashed = false;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_rigidbody.velocity.magnitude >= 100.0f && !isSmashed){
            audioSource.PlayOneShot(smash);
            isSmashed = true;
        }else if(_rigidbody.velocity.magnitude <= 100.0f && isSmashed){
            isSmashed = false;
        }

        if(_transform.position.y<=-20.0f){
            audioSource.PlayOneShot(death);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
           
            if(_rigidbody.velocity.magnitude >= 10.0f && _rigidbody.velocity.magnitude<=100.0f){
            audioSource.PlayOneShot(tackle);
            }
        }
    }
}
