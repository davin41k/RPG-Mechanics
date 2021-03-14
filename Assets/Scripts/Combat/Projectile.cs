using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using UnityEngine;

namespace PRG.Combat
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] float speed = 2f;
		[SerializeField] bool isHoming = true;
		[SerializeField] GameObject hitEffect;
		[SerializeField] float maxLifeTime = 10f;
		[SerializeField] GameObject[] DestroyOnHit;
		[SerializeField] float lifeAfterImpact = 2f;

		float damage = 0f;
		Health target;
		GameObject instagator;

		// Start is called before the first frame update
		void Start()
		{
			if (target != null)
                transform.LookAt(GetAimLocation());
        }

		// Update is called once per frame
		void Update()
		{
			if (target == null) return;
			
			if (isHoming && !target.IsDead()) {
				transform.LookAt(GetAimLocation());
			}
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}

		public void SetTarget(Health target, GameObject instagator, float damage) {
			this.target = target;
			this.damage = damage;
			this.instagator = instagator;

			Destroy(gameObject, maxLifeTime);
		}

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
			if (targetCapsule == null) return target.transform.position;
			
			return target.transform.position + Vector3.up *  targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
			if (target.IsDead()) return;
			target.TakeDamage(instagator, damage);
			speed = 0;
			
			if (hitEffect != null)
				Instantiate(hitEffect, target.transform);

			foreach(GameObject toDeastroy in DestroyOnHit) {
				Destroy(toDeastroy);
			}
			Destroy(gameObject, lifeAfterImpact);
        }
    }
}