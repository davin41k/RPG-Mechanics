using UnityEngine;
using UnityEngine.Playables;
using RPG.Resources;
using RPG.Control;

namespace RPG.Cinematics
{
	public class CinematicsControlRemover : MonoBehaviour
	{
        GameObject player;

		private void Start() {
            player = GameObject.FindWithTag("Player");
            GetComponent<PlayableDirector>().stopped += EnableControl;
            GetComponent<PlayableDirector>().played += DisableControl;
			
		}

		public void EnableControl(PlayableDirector pd) {
            player.GetComponent<PlayerController>().enabled = true;

        }

		public void DisableControl(PlayableDirector pd) {
			player.GetComponent<ActionScheduler>().CancelCurrentAction();
			player.GetComponent<PlayerController>().enabled = false;
		}
	}
}