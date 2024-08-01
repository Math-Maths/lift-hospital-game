using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LiftHospital
{

public class PlayerController : MonoBehaviour
{
    #region Private Fields

    private Rigidbody _playerRb;
    private PlayerInput _playerActions;
    private Vector2 _moveInput;
    private bool _canPick;
    private bool _isGrounded;
    private bool _canMove;
    Transform _objToPick;
    Transform _holdingObj;

    //Actions
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction pickAction;

    #endregion

    #region Editor Private FIelds

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _groundDistance = .1f;
    [SerializeField] private float _trowForce = 5f;
    [SerializeField] private float impactForce = 15;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform pickThreshold;

    #endregion

    void Awake()
    {
        _playerActions = GetComponent<PlayerInput>();
        _playerRb = GetComponent<Rigidbody>();

        moveAction = _playerActions.actions["move"];
        jumpAction = _playerActions.actions["jump"];
        pickAction = _playerActions.actions["pick"];
    }

    void Start()
    {
        _canPick = false;
        _canMove = true;
    }

    void Update()
    {
        CheckGrounded();
        Move();
        OnJump();
        CheckInteraction();
    }

    void OnJump()
    {
        if (_isGrounded && jumpAction.triggered)
        {
            _playerRb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    void CheckInteraction()
    {
        if(_objToPick != null && _canPick && pickAction.triggered && _holdingObj == null)
        {
            _canPick = false;
            var pickObj = _objToPick.GetComponent<PatientBehaviour>();
            pickObj.GotPicked();
            _holdingObj = _objToPick;
            _holdingObj.position = pickThreshold.position;
            _holdingObj.rotation = pickThreshold.rotation;
            _holdingObj.parent = pickThreshold;
        }
        else if(_holdingObj != null && pickAction.triggered)
        {
            ThrowObject(transform.forward * _trowForce);
        }
    }

    void ThrowObject(Vector3 dir)
    {
        var pickObj = _holdingObj.GetComponent<PatientBehaviour>();
        Rigidbody rb = _holdingObj.gameObject.AddComponent<Rigidbody>();
        rb.AddForce(dir, ForceMode.Impulse);
        _holdingObj.parent = null;
        _holdingObj = null;
        pickObj.GotLoose();
    }

    void Move()
    {
        if(_canMove)
        {
            Vector2 moveRaw = moveAction.ReadValue<Vector2>();

            Vector3 move = new Vector3(moveRaw.x, 0f, moveRaw.y) * _speed * Time.deltaTime;
            //_playerRb.velocity += move;
            if (move != Vector3.zero)
            {
                // Ajusta a rotação do objeto para apontar na direção do movimento
                Quaternion targetRotation = Quaternion.LookRotation(move);
                _playerRb.MoveRotation(targetRotation);
            }
            _playerRb.MovePosition(_playerRb.position + move);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        PatientBehaviour patient = null;

        try
        {
            patient = other.gameObject.GetComponent<PatientBehaviour>();
        }
        catch (Exception e)
        {
            Debug.Log("It's not a patient. Error message: " + e);
        }

        if(patient != null  && patient.currentState == PatientBehaviour.PatientState.HELPLESS && _holdingObj == null)
        {
            _canPick = true;
            _objToPick = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        PatientBehaviour patient = null;

        try
        {
            patient = other.gameObject.GetComponent<PatientBehaviour>();
        }
        catch (Exception e)
        {
            Debug.Log("It's not a patient. Error message: " + e);
        }

        if(patient != null && patient.currentState == PatientBehaviour.PatientState.HELPLESS || other.CompareTag(ConstantsValues.RestSpot))
            _canPick = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == ConstantsValues.MovingObstacle)
        {
            StartCoroutine(DisableControlsShortly());
            Debug.Log("call");
            Vector3 collisionDir = collision.GetContact(0).normal;
            _playerRb.AddForce(new Vector3(collisionDir.x, .2f, collisionDir.z) * impactForce, ForceMode.Impulse);
            _playerRb.AddTorque(Vector3.up * impactForce, ForceMode.Impulse);

            if(_holdingObj != null) 
                ThrowObject(new Vector3(UnityEngine.Random.Range(.1f, .3f),UnityEngine.Random.Range(.1f, .3f),UnityEngine.Random.Range(.1f, .3f)) * impactForce);
        }
    }

    IEnumerator DisableControlsShortly()
    {
        _canMove = false;
        yield return new WaitForSeconds(2f);
        _canMove = true;
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundDistance, _groundMask);
    }
}
}
