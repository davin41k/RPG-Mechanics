using System;
using PRG.Combat;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride;
		[SerializeField] GameObject equippedPrefab;
        [SerializeField] float weaponDamage = 7f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHend, Transform leftHand, Animator animator) {

            DestroyOldWeapon(leftHand, rightHend);

			if (equippedPrefab != null) {
				var weapon = Instantiate(equippedPrefab, isRightHanded ? rightHend : leftHand);
                weapon.name = weaponName;
			}
           
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null) {
				animator.runtimeAnimatorController = animatorOverride;
            } else if (overrideController != null) {
               animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
		}

        private void DestroyOldWeapon(Transform leftHand, Transform rightHend)
        {
            Transform oldWeapon = rightHend.Find(weaponName);
            if (oldWeapon == null) {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROING";
            Destroy(oldWeapon.gameObject);
        }

        public bool HasProjectile() {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instagator) {
            Projectile projectileInstance = Instantiate
            (projectile, isRightHanded ? rightHand.position : leftHand.position, Quaternion.identity);
            projectileInstance.SetTarget(target, instagator, weaponDamage);
        }

		public float GetDamage() {
			return weaponDamage;
		}

        public float GetRange()
        {
            return weaponRange;
        }
    }
}
