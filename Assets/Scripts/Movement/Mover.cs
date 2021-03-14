using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Resources;
using RPG.Saving;

namespace RPG.Movement
{
	public class Mover : MonoBehaviour, IAction, ISaveable
	{
		[SerializeField] Transform target;
		[SerializeField] float maxSpeed = 5.66f;

		NavMeshAgent navMeshAgent;
		Health health;
		Ray lastRay;

		// Start is called before the first frame update
		void Start()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			health = GetComponent<Health>();
		}

		// Update is called once per frame
		void Update()
		{
			navMeshAgent.enabled = !health.IsDead();
			UpdateAnimator();
		}

		private void UpdateAnimator()
		{
			Vector3 velocity = navMeshAgent.velocity;
			Vector3 localVelocity = transform.InverseTransformDirection(velocity);
			float speed = localVelocity.z;
			GetComponent<Animator>().SetFloat("forwardSpeed", speed);
		}

		public void MoveTo(Vector3 destination, float speedFraction)
		{
			navMeshAgent.destination = destination;
			navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
			navMeshAgent.isStopped = false;
		}

		public void StartMoveAction(Vector3 destination, float speedFraction) {
			GetComponent<ActionScheduler>().StartAction(this);
			MoveTo(destination, speedFraction);
		}

		public void Cancel() {
			navMeshAgent.isStopped = true;
		}

		public object CaptureState()
		{
			return new SerializableVector3(transform.position);
		}

		public void RestoreState(object state)
		{
			SerializableVector3 position = (SerializableVector3) state;
			GetComponent<NavMeshAgent>().enabled = false;
			transform.position = position.ToVector();
			GetComponent<NavMeshAgent>().enabled = true;
			//Cancel();
		}
	}
}
