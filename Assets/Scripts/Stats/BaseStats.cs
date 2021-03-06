using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 30)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;

        int currentLevel;

        private void Start() {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null) {
                experience.onExperienceGained += UpdateLevel;
            }
        }
        private void UpdateLevel() {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel) {
                currentLevel = newLevel;
                print("LEvel Up");
            }
        }

        public float GetStat(Stat stat) {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel() {
            if (currentLevel < 0) {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            float currentXP = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
           {
               float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP) {
                    return level;
                }
           }
           return penultimateLevel + 1;
        }
    }
}
