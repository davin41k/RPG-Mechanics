using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Scenemanagement
{
	public class SavingWrapper : MonoBehaviour
	{
		const string defalultSaveFile = "save";

		private void Awake() {
			StartCoroutine(LoadLastScene());
		}

		private IEnumerator LoadLastScene() {
            Fader fader = FindObjectOfType<Fader>();
			fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defalultSaveFile);
            yield return fader.FadeIn();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.L)) {
				Load();
			} else if (Input.GetKeyDown(KeyCode.S)) {
				Save();
			} else if (Input.GetKeyDown(KeyCode.D)) {
				Delete();
			}
		}

		public void Load()
		{
			GetComponent<SavingSystem>().Load(defalultSaveFile);
		}

		public void Save() {
			GetComponent<SavingSystem>().Save(defalultSaveFile);
		}

		public void Delete() {
			GetComponent<SavingSystem>().Delete(defalultSaveFile);
		}
	}
}
