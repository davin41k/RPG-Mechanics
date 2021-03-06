using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;

namespace RPG.Control {
	public class PlayerController : MonoBehaviour
	{
		Mover mover;
		Health health;

		// Start is called before the first frame update
		void Start()
		{
			mover = gameObject.GetComponent<Mover>();
            health = GetComponent<Health>();
		}

		// Update is called once per frame
		void Update()
		{
			if (health.IsDead())		return;
			if (InteractWithCombat())	return;
			if (InteractWithMovement())	return;
			//print("Nothing to do");
		}

		private bool InteractWithCombat()
		{
			RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
			foreach (RaycastHit hit in hits)
			{
				CombatTarget target = hit.collider.gameObject.GetComponent<CombatTarget>();
				if (target == null) continue;
				if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue; 
				if (Input.GetMouseButton(0))
					GetComponent<Fighter>().Attack(target.gameObject);
				return true;
			}
			return false;
		}

		private bool InteractWithMovement()
		{
			RaycastHit hit;
			bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
			if (hasHit)
			{
				if (Input.GetMouseButton(0))
				{
					mover.StartMoveAction(hit.point, 1f);
				}
				return true;
			}
			return false;
		}

		private static Ray GetMouseRay()
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}
	}
}
