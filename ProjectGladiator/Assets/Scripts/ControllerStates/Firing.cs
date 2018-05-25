using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    public class FiringState : ControllerState {
        public const float FIRING_TURN_SPEED = 50.0f;

        public override ControllerStateID GetControllerStateID() {
            return ControllerStateID.Firing;
        }

        public override void OnStateUpdate(CharacterMotor motor, CharacterInput input, Character characterBase) {
            Transform root = motor.transform;

            float inputMoveMagnitude = input.worldMotionDirection.magnitude;

            if (inputMoveMagnitude < CharacterMotor.INPUT_DEADZONE) {
                inputMoveMagnitude = 0.0f;
            }

            root.rotation = Quaternion.Lerp(root.rotation, Quaternion.LookRotation(CameraRig.Instance.FlatSpaceDirection(), Vector3.up), Time.deltaTime * FIRING_TURN_SPEED);

            motor.MoveMagnitude = Mathf.Lerp(motor.MoveMagnitude, input.worldMotionDirection.magnitude, Time.deltaTime * motor.stats.moveSpeedAcceleration);

            float oldY = motor.CharacaterRigidbody.velocity.y;
            Vector3 newVelocity = input.worldMotionDirection.normalized * motor.MoveMagnitude * motor.stats.strafeSpeed;
            newVelocity.y = oldY;

            motor.CharacaterRigidbody.velocity = newVelocity;

            characterBase.RequestQuickFireCalculation();

            if (!input.firing) {
                GameManager.Instance.PushMotorState(motor, ControllerStateID.Locomotion);
            }
        }
    }
}
