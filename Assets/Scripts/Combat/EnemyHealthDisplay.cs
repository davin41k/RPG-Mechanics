using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        Text text;

        private void Awake() {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            text = GetComponent<Text>();
        }

        private void Update() {
            if (fighter.GetTarget() == null) {
                text.text = "Empty";
            } else
                text.text = String.Format("{0:0.0}%", fighter.GetTarget().GetPercentage());
        }
    }
}