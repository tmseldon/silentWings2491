using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Stats;
using UnityEngine.UI;

namespace Game.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        //[SerializeField] TextMeshProUGUI _textHealth;
        //[SerializeField] TextMeshProUGUI _textArmor;
        [SerializeField] TextMeshProUGUI _textStunts;
        [SerializeField] TextMeshProUGUI _textDashes;

        [SerializeField] RectTransform _barHealth;
        [SerializeField] RectTransform _barArmor;

        private float _totalArmorPoints, _totalStunts, _totalDashes, _totalSpeed;
        private float _currentArmorPoints, _currentStunts, _currentDashes, _currentHealth;

        private const string STUNTS_TEXT = " ROLL";
        private const string DASHES_TEXT = " DASH";

        public void UpdateUIData(BasicStats nameStat, float currentValue)
        {
            switch(nameStat)
            {
                case BasicStats.stunt:
                    _currentStunts = currentValue;
                    _textStunts.SetText(currentValue + $"/{_totalStunts}" + STUNTS_TEXT);
                    break;

                case BasicStats.dash:
                    _currentDashes = currentValue;
                    _textDashes.SetText(currentValue + $"/{_totalDashes}" + DASHES_TEXT);
                    break;

                case BasicStats.armor:
                    _currentArmorPoints = currentValue;
                    //updateBar
                    _barArmor.localScale = new Vector3(_currentArmorPoints / 25f, 1f, 1f);
                    
                    //_textArmor.SetText(_currentArmorPoints.ToString());
                    break;

                case BasicStats.health:
                    _currentHealth = currentValue;
                    //updateBar
                    _barHealth.localScale = new Vector3(_currentHealth / 25f, 1f, 1f);

                    //_textHealth.SetText("Health:" + _currentHealth);
                    break;

                default:
                    break;
            }
        }

        public void UpdateTotalStats(BasicStats basicStats, float totalValue)
        {
            switch (basicStats)
            {
                case BasicStats.stunt:
                    _totalStunts = totalValue;
                    break;

                case BasicStats.dash:
                    _totalDashes = totalValue;
                    break;

                case BasicStats.armor:
                    _totalArmorPoints = totalValue;
                    break;

                case BasicStats.movementSpeed:
                    _totalSpeed = totalValue;
                    break;

                default:
                    break;
            }
        }

        public void UpdateAll()
        {
            _currentDashes = _totalDashes;
            _currentStunts = _totalStunts;
            _currentArmorPoints = _totalArmorPoints;

            //_textHealth.SetText("Health:" + _currentHealth);
            //_textArmor.SetText(_currentArmorPoints.ToString());

            //Update Bars
            _barArmor.localScale = new Vector3(_currentArmorPoints / 25f, 1f, 1f);
            _barHealth.localScale = new Vector3(_currentHealth / 25f, 1f, 1f);

            _textStunts.SetText(_currentStunts + $"/{_totalStunts}" + STUNTS_TEXT);
            _textDashes.SetText(_currentDashes + $"/{_totalDashes}" + DASHES_TEXT);
        }


    }
}