using UnityEngine;

namespace StatePattern {
    public class Rotation : State {
        public override void Enter() {
            base.Enter();
        }

        public override void Exit() {
            base.Exit();
        }

        public override void Update(Player context) {
            base.Update(context);
            context.transform.Rotation(23);
        }
    }
}