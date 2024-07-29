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
    }

    void Update()
    {
        CheckGrounded();
        Rotate();
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
            _objToPick.position = pickThreshold.position;
            _objToPick.rotation = pickThreshold.rotation;
            _objToPick.parent = pickThreshold;
            _holdingObj = _objToPick;
            var pickObj = _holdingObj.GetComponent<PickObjectBehaviour>();
            pickObj.GotPicked();
        }
        else if(_holdingObj != null && pickAction.triggered)
        {
            Rigidbody rb = _holdingObj.gameObject.AddComponent<Rigidbody>();
            rb.AddForce(rb.transform.forward * _trowForce, ForceMode.Impulse);
            var pickObj = _holdingObj.GetComponent<PickObjectBehaviour>();
            pickObj.GotLoose();
            _holdingObj.parent = null;
            _holdingObj = null;
            
        }
    }

    void Rotate()
    {
        Vector2 moveRaw = moveAction.ReadValue<Vector2>();

        float rotInput = moveRaw.x * _rotationSpeed * Time.deltaTime;
        Quaternion deltaRot = Quaternion.Euler(0f, rotInput, 0f);

        _playerRb.MoveRotation(_playerRb.rotation * deltaRot);
    }

    void Move()
    {
        Vector2 moveRaw = moveAction.ReadValue<Vector2>();

        Vector3 move = new Vector3(0f, 0f, moveRaw.y) * _speed * Time.deltaTime;
        _playerRb.MovePosition(_playerRb.position + transform.TransformDirection(move));
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if(other.CompareTag("pick-obj") && _holdingObj == null)
        {
            _canPick = true;
            _objToPick = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("pick-obj") || other.CompareTag("stil-place"))
            _canPick = false;
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundDistance, _groundMask);
    }
}
}
