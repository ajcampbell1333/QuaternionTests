using System;
using UnityEngine;

namespace QuaternionTests
{
    // Model
    //[CreateAssetMenu]
	public class QuaternionTestData : ScriptableObject
	{
        public RotationType rotationType;
        public float timeInterval;
        public Axis axis;
        public float rotationAngle;
        public Vector3 axisDirection;
        
        [Flags]
        public enum RotationType
        {
            Inverse = 0,
            TransformRotate = 1 << 0,
            Euler = 1 << 1,
            EulerInverse = 1 << 2,
            TransformLookAt = 1 << 3,
            LookRotation_with_HeadUp = 1 << 4,
            LookRotation_with_HeadCustom = 1 << 5,
            AngleAxis = 1 << 6,
            AngleAxisInverse = 1 << 7
        }

        public enum Axis { x, y, z }
    }
}
