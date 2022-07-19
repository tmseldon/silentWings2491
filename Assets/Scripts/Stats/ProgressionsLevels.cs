using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stats
{
    [CreateAssetMenu(fileName = "ProgressionLevelData", menuName = "Stats/ProgressionLevels", order = 0)]
    public class ProgressionsLevels : ScriptableObject
    {
        [SerializeField] AirplaneStats[] _infoStatusAirplane;
        
        private Dictionary<AirplanesTypes, Dictionary<BasicStats, float[]>> _lookUpTable = 
                                            new Dictionary<AirplanesTypes, Dictionary<BasicStats, float[]>>();

        private void OnEnable()
        {
            BuildLookUpTable();
        }

        private void BuildLookUpTable()
        {
            if(_lookUpTable.Count != 0) { return; }

            foreach (AirplaneStats data in _infoStatusAirplane)
            {
                Dictionary<BasicStats, float[]> tempStatInfo = new Dictionary<BasicStats, float[]>();

                foreach (AttributesData statInfo in data.Attributes)
                {
                    tempStatInfo.Add(statInfo.Stats, statInfo.levels);
                }
                
                _lookUpTable.Add(data.Type, tempStatInfo);
            }
        }

        public float GetStat(BasicStats stat, AirplanesTypes airplaneClass, int level)
        {
            if (!_lookUpTable.ContainsKey(airplaneClass)) { return 0; }

            Dictionary<BasicStats, float[]> getStatAndLevels = _lookUpTable[airplaneClass];

            if (getStatAndLevels != null)
            {
                if (!getStatAndLevels.ContainsKey(stat)) { return 0; }

                float[] levelsInStat = getStatAndLevels[stat];
                if (levelsInStat != null && levelsInStat.Length >= level)
                {
                    return levelsInStat[level - 1];
                }
            }

            return 0;
        }
    }

    [System.Serializable]
    public class AirplaneStats
    {
        public AirplanesTypes Type;
        public AttributesData[] Attributes; 
    }

    [System.Serializable]
    public class AttributesData
    {
        public BasicStats Stats;
        public float[] levels;
    }
}