using UnityEngine;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction, ISaveable
	{
		[SerializeField] float timeBetweenAttacks = 1f;
		[SerializeField] Transform rightHandTransform;
		[SerializeField] Transform leftHandTransform;
		[SerializeField] Weapon defaultWeapon;


		Health target;
		Mover mover;
		float timeSinceLastAttack = Mathf.Infinity;
		Weapon currentWeapon;

		private void Start()
		{
			mover = GetComponent<Mover>();
			if (currentWeapon == null) {
                EquipWeapon(defaultWeapon);
            }
		}

		public void EquipWeapon(Weapon weapon)
		{
			currentWeapon = weapon;
			Animator animator = GetComponent<Animator>();
			weapon.Spawn(rightHandTransform, leftHandTransform, animator);
		}

		public Health GetTarget() {
			return target;
		}

		private void Update()
		{
			timeSinceLastAttack += Time.deltaTime;
			if (target == null) return;

			if (target.IsDead()) return;

			if (!GetIsInRange()) {
				mover.MoveTo(target.transform.position, 1f);
			} else
			{
				mover.Cancel();
				AttackBehaviour();
			}
		}

		private void AttackBehaviour()
		{
			transform.LookAt(target.transform);

			if (timeSinceLastAttack > timeBetweenAttacks) {
				// This will trigger the Hit() event
			  //  GetComponent<Animator>().ResetTrigger("stopAttack");
				GetComponent<Animator>().SetTrigger("attack");
				timeSinceLastAttack = 0;
			}
		}
		
		//Animation Event, call from animator
		void Hit()
		{
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            } else {
				if (target != null) 
				target.TakeDamage(gameObject, currentWeapon.GetDamage());
			}
		}

		void Shoot() {
			Hit();
		}

		bool GetIsInRange()
			{
				return Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetRange();
			}

		public void Attack(GameObject combatTarget) {

			GetComponent<ActionScheduler>().StartAction(this);
			target = combatTarget.GetComponent<Health>();
		}

		public bool CanAttack(GameObject target) {
			return (target != null && !target.GetComponent<Health>().IsDead());
		}

		public void Cancel()
		{
		   // GetComponent<Animator>().ResetTrigger("attack");
			GetComponent<Animator>().SetTrigger("stopAttack");
			target = null;
			mover.Cancel();
		}

        public object CaptureState()
        {
			return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
			string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
			EquipWeapon(weapon);
        }
    }
}