using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    public class LocomotionState : ControllerState {
        //public float currentMoveMagnitude;

        public override ControllerStateID GetControllerStateID() {
            return ControllerStateID.Locomotion;
        }

        public override void OnStateEnter(CharacterMotor motor) {

        }

        public override void OnStateUpdate(CharacterMotor motor, CharacterInput input, Character characterBase) {
            Transform root = motor.transform;

            Debug.DrawRay(motor.transform.position + Vector3.up, input.worldMotionDirection * 5.0f);

            float inputMoveMagnitude = input.worldMotionDirection.magnitude;
            Vector3 targetDir = input.worldMotionDirection;

            if (inputMoveMagnitude < CharacterMotor.INPUT_DEADZONE) {
                inputMoveMagnitude = 0.0f;
                targetDir = root.forward;
            }

            motor.MoveMagnitude = Mathf.Lerp(motor.MoveMagnitude, input.worldMotionDirection.magnitude, Time.deltaTime * motor.stats.moveSpeedAcceleration);
            root.rotation = Quaternion.Lerp(root.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * motor.stats.turnSpeed * motor.MoveMagnitude);

            float oldY = motor.CharacaterRigidbody.velocity.y;
            Vector3 newVelocity = root.forward * motor.MoveMagnitude * motor.stats.moveSpeed;
            newVelocity.y = oldY;

            motor.CharacaterRigidbody.velocity = newVelocity;

            if (input.firing) {
                GameManager.Instance.PushMotorState(motor, ControllerStateID.Firing);
            }
        }
    }
}
