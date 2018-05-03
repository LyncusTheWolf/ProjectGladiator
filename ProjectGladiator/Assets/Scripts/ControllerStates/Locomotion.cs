using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    public class LocomotionState : ControllerState {
        public float currentMoveMagnitude;

        public override void OnStateEnter() {
            currentMoveMagnitude = 0.0f;
        }

        public override void OnStateUpdate(CharacterInput input) {
            Transform root = parentMotor.transform;

            Debug.DrawRay(parentMotor.transform.position + Vector3.up, input.worldMotionDirection * 5.0f);

            float inputMoveMagnitude = input.worldMotionDirection.magnitude;
            Vector3 targetDir = input.worldMotionDirection;

            if (inputMoveMagnitude < CharacterMotor.INPUT_DEADZONE) {
                inputMoveMagnitude = 0.0f;
                targetDir = root.forward;
            }

            currentMoveMagnitude = Mathf.Lerp(currentMoveMagnitude, input.worldMotionDirection.magnitude, Time.deltaTime * parentMotor.stats.moveSpeedAcceleration);
            root.rotation = Quaternion.Lerp(root.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * parentMotor.stats.turnSpeed * currentMoveMagnitude);

            float oldY = parentMotor.CharacaterRigidbody.velocity.y;
            Vector3 newVelocity = root.forward * currentMoveMagnitude * parentMotor.stats.moveSpeed;
            newVelocity.y = oldY;

            parentMotor.CharacaterRigidbody.velocity = newVelocity;
        }
    }
}
