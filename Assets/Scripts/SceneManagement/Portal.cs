using System;
using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.Scenemanagement
{
	public class Portal : MonoBehaviour
	{
		enum DestinationIdentifire {
			A, B, C, D, E, F
		}

		[SerializeField] int sceneToLoad = -1;
		[SerializeField] Transform spawnPoint;
		[SerializeField] DestinationIdentifire destination;

		private void Awake() {
			DontDestroyOnLoad(gameObject);
		}

		private void OnTriggerEnter(Collider other) {
			if (other.tag == "Player") {
				StartCoroutine(Transition());
			}
		}

		private IEnumerator Transition() {
			if (sceneToLoad < 0) {
				Debug.LogError("Have not scene to load!");
				yield break;
			}
			DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut();
			SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
			wrapper.Save();
			yield return SceneManager.LoadSceneAsync(sceneToLoad);
			wrapper.Load();
			Portal otherPortal = GetOtherPortal();
			UpdatePlayer(otherPortal);
			wrapper.Save();
            yield return fader.FadeIn();
            Destroy(gameObject);


		}

		private void UpdatePlayer(Portal otherPortal)
		{
			var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
			//player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
		    player.transform.position = otherPortal.spawnPoint.position;
			player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

		private Portal GetOtherPortal()
		{
			Portal rePortal = null;

			foreach(var portal in FindObjectsOfType<Portal>()) {
				if (portal == this) continue;
				if (destination != portal.destination) continue;
					rePortal = portal;
				rePortal = portal;
			}
			return rePortal;
		}
	}
}