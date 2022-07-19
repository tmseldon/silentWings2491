using Game.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class PerkManager : MonoBehaviour
    {
        [SerializeField] ProgressionPerks _perksData;
        [SerializeField] int _numberSkillsAtEnd = 3;
        public int NumberSkillsAtEnd { get { return _numberSkillsAtEnd; }}

        private Dictionary<BasicStats, Queue<float>> _perksAvailableList = new Dictionary<BasicStats, Queue<float>>();

        private void OnEnable()
        {
            _perksAvailableList = _perksData.PerksAvailableTable;
        }

        public BasicStats[] GetRandomAvailablePerks(out float[] values)
        {
            List<BasicStats> listOfBuffs = new List<BasicStats>();
            foreach (KeyValuePair<BasicStats, Queue<float>> pair in _perksAvailableList)
            {
                listOfBuffs.Add(pair.Key);
            }

            BasicStats[] listToReturn = new BasicStats[_numberSkillsAtEnd];
            float[] selectedValues = new float[_numberSkillsAtEnd];

            for(int index = 0; index < _numberSkillsAtEnd; index++)
            {
                int randomIndex = Random.Range(0, listOfBuffs.Count);
                listToReturn[index] = listOfBuffs[randomIndex];
                selectedValues[index] = _perksAvailableList[listOfBuffs[randomIndex]].Peek();
                
                listOfBuffs.RemoveAt(randomIndex);
            }

            values = selectedValues;
            return listToReturn;
        }

        public void RemoveSelectedPerk(BasicStats perkName, float value)
        {
            Debug.Log($"se va a remover del queue {perkName} con el valor {value}");
            
            float valueToRemove = _perksAvailableList[perkName].Dequeue();
            if(valueToRemove != value) 
            { Debug.Log($"hubo un error en el queue valor a remover {value} y valor removido {valueToRemove}"); }
        }
    }
}