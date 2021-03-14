using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
	public class Health : MonoBehaviour, ISaveable
	{
		float healthPoints = -1f;
		bool isDead;

		private void Start() {
			if (healthPoints < 0)
				healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);

		}

		public bool IsDead()
		{
			return isDead;
		}

		public void TakeDamage(GameObject instagator, float damage) {
			if (healthPoints == 0) return;
			healthPoints = Mathf.Max(healthPoints - damage, 0);
			if (healthPoints == 0) {
				Die();
				AwardExperiance(instagator);
			}
		}


        public float GetPercentage() {
			return healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health) * 100;
		}

		private void Die()
		{
			if (isDead) return;
			
			GetComponent<Animator>().SetTrigger("die");
			isDead = true;
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

        private void AwardExperiance(GameObject instagator) {
			Experience experience = instagator.GetComponent<Experience>();
			if (experience == null) return;

			experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

		public object CaptureState()
		{
			return  healthPoints;
		}

		public void RestoreState(object state)
		{
			healthPoints = (float) state;
            if (healthPoints == 0)
            {
                Die();
            }
		}
	}
}