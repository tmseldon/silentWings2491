using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Game.Gameplay;
using Game.Core;
using Game.Stats;

namespace Game.UI
{
    public class PerksSelector : MonoBehaviour
    {
        [SerializeField] Button[] _selectionButtons;
        [SerializeField] AudioClip _audioMenu;

        private const string SPEED_TITLE = "Increase your speed by ";
        private const string ARMOR_TITLE = "Increase your armor by ";
        private const string STUNT_TITLE = " more evasive stunt!";
        private const string DASH_TITLE = " more dash!";

        private Player _playerPlane;
        private PlayerMovement _playerMovement;
        private PerkManager _perkManagerController;
        private GameManager _gameManager;

        private BasicStats[] _lookUpTablePerks;
        private float[] _lookUpTableValues;

        private void Awake()
        {
            _playerPlane = FindObjectOfType<Player>();
            _playerMovement = FindObjectOfType<PlayerMovement>();
            _perkManagerController = FindObjectOfType<PerkManager>();
            _gameManager = FindObjectOfType<GameManager>();

            _lookUpTablePerks = new BasicStats[_perkManagerController.NumberSkillsAtEnd];
            _lookUpTableValues = new float[_perkManagerController.NumberSkillsAtEnd];

            gameObject.SetActive(false);
        }

        public void TurnOnSkillSelector(bool status)
        {
            gameObject.SetActive(status);
            FillSelectionButtons();
            AudioSource.PlayClipAtPoint(_audioMenu, Camera.main.transform.position);
        }

        private void FillSelectionButtons()
        {
            float[] valuesBonus = new float[_perkManagerController.NumberSkillsAtEnd];
            BasicStats[] nameBonusList = new BasicStats[_perkManagerController.NumberSkillsAtEnd];

            nameBonusList = _perkManagerController.GetRandomAvailablePerks(out valuesBonus);

            for (int index = 0; index < _perkManagerController.NumberSkillsAtEnd; index++)
            {
                _lookUpTablePerks[index] = nameBonusList[index];
                _lookUpTableValues[index] = valuesBonus[index];
                
                AddButtonLabel(nameBonusList[index], valuesBonus[index], _selectionButtons[index]);
            }
        }

        public void ProcessButtonClick(int numberButton)
        {
            if (_lookUpTablePerks[numberButton] == BasicStats.stunt || _lookUpTablePerks[numberButton] == BasicStats.dash)
            {
                ProcessMovementsSelection(_lookUpTablePerks[numberButton], _lookUpTableValues[numberButton]);
            }
            else
            {
                ProcessStatsSelection(_lookUpTablePerks[numberButton], _lookUpTableValues[numberButton]);
            }

            //llamar seguir siguiente etapa
            _gameManager.StartNextLevel();
            TurnOnSkillSelector(false);
        }

        private void ProcessStatsSelection(BasicStats perkSelection, float valueSelection)
        {
            _playerPlane.AddPercentage(perkSelection, valueSelection);
            _perkManagerController.RemoveSelectedPerk(perkSelection, valueSelection);
        }

        private void ProcessMovementsSelection(BasicStats perkSelection, float valueSelection)
        {
            _playerMovement.AddMoves(perkSelection, valueSelection);
            _perkManagerController.RemoveSelectedPerk(perkSelection, valueSelection);
        }

        private void AddButtonLabel(BasicStats basicStats, float value, Button buttonSelector)
        {
            switch (basicStats)
            {
                //case BasicStats.health:
                //    break;

                case BasicStats.armor:
                    buttonSelector.GetComponentInChildren<TextMeshProUGUI>().SetText(ARMOR_TITLE + (value * 100) + "%");
                    break;

                case BasicStats.movementSpeed:
                    buttonSelector.GetComponentInChildren<TextMeshProUGUI>().SetText(SPEED_TITLE + (value * 100) + "%");
                    break;

                case BasicStats.stunt:
                    buttonSelector.GetComponentInChildren<TextMeshProUGUI>().SetText($"Add {value}" + STUNT_TITLE);
                    break;

                case BasicStats.dash:
                    buttonSelector.GetComponentInChildren<TextMeshProUGUI>().SetText($"Add {value}" + DASH_TITLE);
                    break;

                default:
                    break;
            }
        }
    }
}