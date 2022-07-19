using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stats
{
    [CreateAssetMenu(fileName = "PerkLevels", menuName = "Stats/PerkLevels", order = 1)]
    public class ProgressionPerks : ScriptableObject
    {
        [SerializeField] Buffs[] _listOfBuffs;

        Dictionary<BasicStats, Queue<float>> _perksAvailableTable = new Dictionary<BasicStats, Queue<float>>();
        public Dictionary<BasicStats, Queue<float>> PerksAvailableTable
        {
            get { return _perksAvailableTable; }
        }

        private void OnEnable()
        {
            BuildTableOfPerks();
        }

        private void BuildTableOfPerks()
        {
            if(_perksAvailableTable.Count != 0) { return; }

            foreach (Buffs buff in _listOfBuffs)
            {
                Queue<float> valueQueue = new Queue<float>(buff.AddedValuePerLevel);
                _perksAvailableTable.Add(buff.PerksName, valueQueue);
            }
        }
    }

    [System.Serializable]
    public class Buffs
    {
        public BasicStats PerksName;
        public float[] AddedValuePerLevel;
    }
}