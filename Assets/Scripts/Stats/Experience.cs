using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiancePoints;

        public event Action onExperienceGained;

        public void GainExperience(float experience) {
            experiancePoints += experience;
            onExperienceGained();
        }

        public float GetPoints() {
            return experiancePoints;
        }

        public object CaptureState()
        {
            return experiancePoints;
        }

        public void RestoreState(object state)
        {
           experiancePoints = (float)state;
        }
    }
}