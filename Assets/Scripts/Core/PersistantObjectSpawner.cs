using UnityEngine;

namespace RPG.Resources
{
	public class PersistantObjectSpawner : MonoBehaviour
	{
		[SerializeField] GameObject persistentObjectPrefab;

		static bool hasSpawned = false;

		private void Awake() {
			if (hasSpawned) return;

			SpawnPersistantObjects();
			hasSpawned = true;
		}

		private void SpawnPersistantObjects() {
			GameObject persistentObject = Instantiate(persistentObjectPrefab);
			DontDestroyOnLoad(persistentObject);
		}
	}
}