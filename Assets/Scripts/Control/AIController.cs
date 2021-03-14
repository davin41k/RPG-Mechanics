using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Resources;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
	public class AIController : MonoBehaviour
	{
		[SerializeField] float chaseDistance = 5f;
		[SerializeField] float suspicionTime = 5f;
		[SerializeField] PatrolPath patrolPath;
		[SerializeField] float waypointTolerance = 1f;
		[SerializeField] float waypointDwellTime = 3f;
		[Range(0,1)]
		[SerializeField] float patrolSpeedFraction = 0.4f;

		Fighter fighter;
		GameObject player;
		Health health;
		Mover mover;

		Vector3 guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		float timeSinceArrivedAtWaypoint = Mathf.Infinity;
		int currentPointIndex = 0;

		private void Start() {
			fighter = GetComponent<Fighter>();
			player = GameObject.FindWithTag("Player");
			health = GetComponent<Health>();
			guardPosition = transform.position;
			mover = GetComponent<Mover>();
		}
		
		private void Update()
		{
			if (health.IsDead()) return;

			if (InAttackRangeOfPlayer(player) && fighter.CanAttack(player))
			{

                AttackBehaviour();
			}
			else if (timeSinceLastSawPlayer < suspicionTime)
			{
				SuspicionBehaviour();
			}
			else
			{
				PatrolBehaviour();
			}
			UpdateTimers();

		}

		private void UpdateTimers()
		{
			timeSinceLastSawPlayer += Time.deltaTime;
			timeSinceArrivedAtWaypoint += Time.deltaTime;
		}

		private void PatrolBehaviour()
		{
			Vector3 nextPosition = guardPosition;

			if (patrolPath != null) {
				if (AtWaypoint()) {
					timeSinceArrivedAtWaypoint = 0;
					CycleWaypoint();
				}
				nextPosition = GetCurrentWaypoint();
			}
			if (timeSinceArrivedAtWaypoint > waypointDwellTime) {
				mover.StartMoveAction(nextPosition, patrolSpeedFraction);
			}
		}

		private bool AtWaypoint()
		{
			float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
			return distanceToWaypoint < waypointTolerance; 
		}

		private void CycleWaypoint()
		{
			currentPointIndex = patrolPath.GetNextIndex(currentPointIndex);
		}

		private Vector3 GetCurrentWaypoint()
		{
			return patrolPath.GetWaypoint(currentPointIndex);
		}

		private void SuspicionBehaviour()
		{
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

		private void AttackBehaviour()
		{
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
		}

		private bool InAttackRangeOfPlayer(GameObject player) {
			float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
			return distanceToPlayer <= chaseDistance;
		}

		// Called by unity
		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
		}
	}
}
