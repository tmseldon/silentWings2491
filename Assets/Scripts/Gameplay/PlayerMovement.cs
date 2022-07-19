using Game.Stats;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Gameplay
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] ProgressionsLevels _initialData;

        [Header("Movement Params")]
        [SerializeField] float _speedMovement = 2.5f;
        [SerializeField] float _angleTilt = 15f;
        [SerializeField] float _minX = -23f;
        [SerializeField] float _maxX = 23f;
        [SerializeField] float _minZ = 0f;
        [SerializeField] float _maxZ = 30f;

        [Header("Stunt Params")]
        [SerializeField] float _speedStunt = 85f;
        [SerializeField] float _offsetStunt = 5f;
        [SerializeField] AudioClip _stuntSoundFX;

        [Header("Dash Params")]
        [SerializeField] float _speedDash = 85f;
        [SerializeField] AudioClip _dashSoundFX;

        public float SpeedMovement 
        { 
            get { return _speedMovement; }
            set { _speedMovement = value; } 
        }

        private PlayerHUD _playerHud;
        //private GameManager _gameManager;
        private AudioSource _playerMovementAudio;

        //Movement related
        private Vector3 _movementInput;
        private Vector3 _initialPosition;
        private PlayerInput _playerAirplaneInput;
        private bool _isMoving = true;
        private Player _playerControl;
        public PlayerInput PlayerAirplaneInput { set { _playerAirplaneInput = value; } }

        //Rotation related
        private Quaternion _initialRotation;
        private float _initialYaw;

        //Stunt
        private Vector3 _offsetUp;
        private bool _isStunt = false;
        private float _angleStunt, _accumulatedAngle, _initialAltitude = 0f;
        private float _currentNumberStunts, _totalNumberStunts;

        //Dash
        private Rigidbody _rbPlayer;
        private bool _isDash = false;
        private float _dashTime = 0;
        private float _dashDuration = 0.2f;
        private float _currentNumberDashes, _totalNumberDashes;

        private void Awake()
        {
            _playerHud = FindObjectOfType<PlayerHUD>();
            _rbPlayer = GetComponent<Rigidbody>();
            _playerAirplaneInput = GetComponent<PlayerInput>();
            _playerControl = GetComponent<Player>();
            //_gameManager = FindObjectOfType<GameManager>();
            _playerMovementAudio = GetComponent<AudioSource>();

            _initialRotation = transform.rotation;
            _initialYaw = _initialRotation.eulerAngles.y;
            _initialAltitude = transform.position.y;
            _initialPosition = transform.position;

            //Set initial numbers of dash and stunt
            SetUpInitialData();
            RestartMoves();

            //HUD update
            _playerHud.UpdateTotalStats(BasicStats.dash, _totalNumberDashes);
            _playerHud.UpdateTotalStats(BasicStats.stunt, _totalNumberStunts);
            _playerHud.UpdateAll();
        }

        //private void OnEnable()
        //{
        //    _gameManager.StopGame += TurnOffPlayerInput;
        //    _gameManager.GoingToNextLevel += TurnOnPlayerInput;
        //}

        //private void OnDisable()
        //{
        //    _gameManager.StopGame -= TurnOffPlayerInput;
        //    _gameManager.GoingToNextLevel -= TurnOnPlayerInput;
        //}

        void Update()
        {
            //Stunt
            if (_isStunt)
            {
                _angleStunt = _speedStunt * Time.deltaTime;
                _accumulatedAngle += _angleStunt;

                transform.RotateAround(_offsetUp, Vector3.left, _angleStunt);

                if(_accumulatedAngle > 360f)
                {
                    _accumulatedAngle = 0f;
                    transform.position = new Vector3(transform.position.x, _initialAltitude, transform.position.z);
                    _currentNumberStunts--;
                    _playerHud.UpdateUIData(BasicStats.stunt, _currentNumberStunts);
                    SetBorderMoveConditions(false, out _isStunt);
                }
            }

            //Dash
            else if(_isDash)
            {
                _dashTime += Time.deltaTime;
                if(_dashTime >= _dashDuration)
                {
                    _rbPlayer.velocity = new Vector3(0f, 0f, 0f);
                    _dashTime = 0;
                    _currentNumberDashes--;
                    _playerHud.UpdateUIData(BasicStats.dash, _currentNumberDashes);
                    SetBorderMoveConditions(false, out _isDash);
                }
            }

            else
            {
                //Normal Movement
                Vector3 addDeltaMovement = transform.position + _movementInput * _speedMovement * Time.deltaTime;
                transform.position = new Vector3(Mathf.Clamp(addDeltaMovement.x, _minX, _maxX),
                                                 _initialAltitude,
                                                 Mathf.Clamp(addDeltaMovement.z, _minZ, _maxZ));
            }

        }

        private void SetUpInitialData()
        {
            //initial data
            _totalNumberStunts = _initialData.GetStat(BasicStats.stunt, AirplanesTypes.player, 1);
            _totalNumberDashes = _initialData.GetStat(BasicStats.dash, AirplanesTypes.player, 1);
        }

        public void AddMoves(BasicStats stat, float addNumberMoves)
        {
            switch (stat)
            {
                case BasicStats.stunt:
                    _totalNumberStunts += addNumberMoves;
                    //Debug.Log($"nuevos movimientos stunt: {_totalNumberStunts}");
                    _playerHud.UpdateTotalStats(BasicStats.stunt, _totalNumberStunts);
                    break;

                case BasicStats.dash:
                    _totalNumberDashes += addNumberMoves;
                    //Debug.Log($"nuevos movimientos dash: {_totalNumberDashes}");
                    _playerHud.UpdateTotalStats(BasicStats.dash, _totalNumberDashes);
                    break;

                default:
                    break;
            }

            RestartMoves();
            _playerControl.RestartStats();
            _playerHud.UpdateAll();
        }

        public void RestartMoves()
        {
            _currentNumberStunts = _totalNumberStunts;
            _currentNumberDashes = _totalNumberDashes;
            transform.position = _initialPosition;
        }

        private void OnMove(InputValue movementValue)
        {
            //Movement
            Vector2 movement2D = movementValue.Get<Vector2>();
            _movementInput = new Vector3(movement2D.x, 0f, movement2D.y);

            //Rotation
            if(movement2D.x == 0 && movement2D.y == 0)
            {
                transform.rotation = _initialRotation;
            }
            else
            {
                transform.rotation = Quaternion.Euler(movement2D.x * _angleTilt, _initialYaw, movement2D.y * _angleTilt);
            }
        }

        private void OnStunt()
        {
            //Check if there are available stunts
            if(_currentNumberStunts <= 0) { return; }

            _offsetUp = transform.position + Vector3.up * _offsetStunt;
            SetBorderMoveConditions(true, out _isStunt);

            _playerMovementAudio.clip = _stuntSoundFX;
            _playerMovementAudio.Play();
        }

        private void OnDash()
        {
            //Check if there are available dashes
            if(_currentNumberDashes <= 0) { return; }

            _rbPlayer.AddForce(Vector3.forward * _speedDash, ForceMode.Impulse);
            SetBorderMoveConditions(true, out _isDash);

            _playerMovementAudio.clip = _dashSoundFX;
            _playerMovementAudio.Play();
        }

        private void TurnOnPlayerInput()
        {
            _playerAirplaneInput.ActivateInput();
        }

        private void TurnOnPlayerInput(int nothing)
        {
            TurnOnPlayerInput();
        }

        private void TurnOffPlayerInput()
        {
            _playerAirplaneInput.DeactivateInput();
        }

        private void SetBorderMoveConditions(bool status, out bool moveState)
        {
            moveState = status;
            _playerControl.IsInvincible = status;
            if(status)
            {
                TurnOffPlayerInput();
            }
            else
            {
                TurnOnPlayerInput();
            }
        }
    }
}

