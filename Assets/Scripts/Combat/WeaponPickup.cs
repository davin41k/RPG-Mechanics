using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Combat
{
	public class WeaponPickup : MonoBehaviour
	{
		[SerializeField] Weapon weapon;
		[SerializeField] float respawnTime = 5f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				Fighter figher = other.GetComponent<Fighter>();
				figher.EquipWeapon(weapon);
				StartCoroutine(HideForSeconds(respawnTime));
				//Destroy(gameObject);
			}
		}

		private IEnumerator HideForSeconds(float seconds) {
			ShowPickup(false);
			yield return new WaitForSeconds(seconds);
			ShowPickup(true);
		}

        private void ShowPickup(bool shoudShow)
        {
			GetComponent<Collider>().enabled = shoudShow;
			foreach (Transform child in transform) {
				child.gameObject.SetActive(shoudShow);
			}
        }
    }
}
