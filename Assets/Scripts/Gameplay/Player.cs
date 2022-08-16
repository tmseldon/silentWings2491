using Game.Stats;
using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] ProgressionsLevels _initialData;
        [SerializeField] float _invicibilityTimeDefault = 0.5f;
        [SerializeField] GameObject _explotionFX;
        [SerializeField] AudioClip _hitSoundFX;
        [SerializeField] AudioClip _explotionSoundFX;
        [SerializeField] Material _hitMaterial;
        
        public event Action OnPlayerDeath;

        //Player params
        private float _healthPlayer;
        private float _armorPoints;
        private float _currentHealthPlayer;
        private float _currentArmorPoints;
        private bool _isInvincible = false;
        private float _timeHitFX = 0.2f;

        //Components
        private AudioSource _playerAudioSource;
        private Renderer _playerRenderer;
        private Material _initialMaterial;
        private WaitForSeconds _timeHitWait;

        public bool IsInvincible
        {
            get { return _isInvincible; }
            set { _isInvincible = value; }
        }

        private PlayerMovement _playerMover;
        private PlayerHUD _playerHud;

        private void Awake()
        {
            _playerMover = GetComponent<PlayerMovement>();
            _playerHud = FindObjectOfType<PlayerHUD>();
            _playerAudioSource = GetComponent<AudioSource>();
            _playerRenderer = GetComponent<Renderer>();

            //Setting up material and params
            _initialMaterial = _playerRenderer.material;
            _timeHitWait = new WaitForSeconds(_timeHitFX);
            SetUpInitialData();
            RestartStats();
        }

        private void SetUpInitialData()
        {
            //initial data
            _healthPlayer = _initialData.GetStat(BasicStats.health, AirplanesTypes.player, 1);
            _armorPoints = _initialData.GetStat(BasicStats.armor, AirplanesTypes.player, 1);
            _playerMover.SpeedMovement = _initialData.GetStat(BasicStats.movementSpeed, AirplanesTypes.player, 1);

            _playerHud.UpdateTotalStats(BasicStats.armor, _armorPoints);
            _playerHud.UpdateUIData(BasicStats.health, _healthPlayer);
            _playerHud.UpdateAll();
        }

        //Perks/Buff based on percentage
        public void AddPercentage(BasicStats stat, float percentage)
        {
            switch (stat)
            {
                //case BasicStats.health:
                //    _healthPlayer *= (1 + percentage);
                //    break;

                case BasicStats.armor:
                    _armorPoints *= (1 + percentage);
                    _playerHud.UpdateTotalStats(BasicStats.armor, _armorPoints);
                    break;

                case BasicStats.movementSpeed:
                    _playerMover.SpeedMovement *= (1 + percentage);
                    _playerHud.UpdateTotalStats(BasicStats.movementSpeed, _playerMover.SpeedMovement);
                    break;

                default:
                    break;
            }

            RestartStats();
            _playerMover.RestartMoves();
            _playerHud.UpdateAll();
        }

        public void RestartStats()
        {
            _currentHealthPlayer = _healthPlayer;
            _currentArmorPoints = _armorPoints;
            _isInvincible = false;
        }

        public void TurnOnInvincibility(float duration)
        {
            StartCoroutine(StartTimerInvincibility(duration));
        }

        public void TurnOnInvincibility()
        {
            TurnOnInvincibility(_invicibilityTimeDefault);
        }

        private IEnumerator StartTimerInvincibility(float time)
        {
            _isInvincible = true;
            yield return new WaitForSeconds(time);
            _isInvincible = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_isInvincible) { return; }

            if (other.gameObject.layer == 7)
            {
                _playerAudioSource.clip = _hitSoundFX;
                _playerAudioSource.Play();

                StartCoroutine(HitChangeColor());

                //Check Armor
                if(_currentArmorPoints > 0)
                {
                    _currentArmorPoints--;
                    //Debug.Log($"armor: {_currentArmorPoints} / {_armorPoints}");
                    //Debug.Log($"health: {_currentHealthPlayer} / {_healthPlayer}");
                    _playerHud.UpdateUIData(BasicStats.armor, _currentArmorPoints);
                    return;
                }

                _currentHealthPlayer--;
                //Debug.Log($"health: {_currentHealthPlayer} / {_healthPlayer}");
                _playerHud.UpdateUIData(BasicStats.health, _currentHealthPlayer);
                if (_currentHealthPlayer <= 0)
                {
                    StartCoroutine(PlayerDie());
                }
            }
        }

        //FX when hit
        private IEnumerator HitChangeColor()
        {
            _playerRenderer.material = _hitMaterial;
            yield return _timeHitWait;
            _playerRenderer.material = _initialMaterial;
        }
    

        private IEnumerator PlayerDie()
        {
            //Game Over
            Instantiate(_explotionFX, transform.position, Quaternion.identity);
            _playerAudioSource.clip = _explotionSoundFX;
            _playerAudioSource.Play()
;
            yield return new WaitForSeconds(1.5f);
            //Call GameManager
            OnPlayerDeath();
        }
    }
}