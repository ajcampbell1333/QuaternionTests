using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static QuaternionTests.QuaternionTestData;

namespace QuaternionTests
{
    // Controller
    public class QuaternionTests : MonoBehaviour
	{
        //public RotationType rotationType = RotationType.Inverse;
        public Transform lookTarget;
        public Transform upTarget;
        public IEnumerator coroutine;
        public QuaternionTestData data;

        public void Start() {
            if (coroutine != null)
                StopCoroutine(coroutine);
            SelectCoroutine();
            StartCoroutine(coroutine);
        }
        
        private IEnumerator Inverse()
        {
            while (data.rotationType == RotationType.Inverse)
            {
                Debug.Log("transform.rotation = transform.rotation.Inverse()");
                transform.rotation = transform.rotation.Inverse();
                yield return new WaitForSeconds(data.timeInterval);
            }
            Start();
        }

        private IEnumerator RotateWithTransformRotate()
        {
            while (data.rotationType == RotationType.TransformRotate)
            {
                Debug.Log("transform.Rotate()");
                transform.Rotate(Get(data.axis, data.rotationAngle));
                yield return new WaitForSeconds(data.timeInterval);
            }
            Start();
        }
       
        private IEnumerator RotateWithEuler()
        { 
            while (data.rotationType == RotationType.Euler)
            {
                Debug.Log("transform.rotation x Quaternion.Euler()");
                transform.rotation = transform.rotation * Quaternion.Euler(Get(data.axis, data.rotationAngle));
                yield return new WaitForSeconds(data.timeInterval);
            }
            Start();
        }

        private IEnumerator RotateWithEulerInverse()
        {
            while (data.rotationType == RotationType.EulerInverse)
            {
                Debug.Log("transform.rotation x Quaternion.Euler().Inverse()");
                transform.rotation = transform.rotation * Quaternion.Euler(Get(data.axis, data.rotationAngle)).Inverse();
                yield return new WaitForSeconds(data.timeInterval);
            }
            Start();
        }

        private IEnumerator RotateWithTransformLookAt()
        {
            while (data.rotationType == RotationType.TransformLookAt)
            {
                Debug.Log("transform.LookAt()");
                transform.LookAt(lookTarget);
                yield return new WaitForSeconds(data.timeInterval);
            }
            Start();
        }

        private IEnumerator RotateWithLookRotationWithHeadAlwaysUp()
        {
            while (data.rotationType == RotationType.LookRotation_with_HeadUp)
            {
                Debug.Log("transform.rotation * Quaternion.LookRotation(noseDirection,Vector3.up)");
                Vector3 direction = (lookTarget.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                yield return new WaitForSeconds(data.timeInterval);
            }
            Start();
        }

        private IEnumerator RotateWithLookRotationWithHeadCustomized()
        {
            while (data.rotationType == RotationType.LookRotation_with_HeadCustom)
            {
                Debug.Log("transform.rotation x Quaternion.LookRotation(noseDirection,headDirection)");
                Vector3 noseDirection = (lookTarget.position - transform.position).normalized;
                Vector3 headDirection = (upTarget.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(noseDirection, headDirection);
                yield return new WaitForSeconds(data.timeInterval);
            }
            Start();
        }

        private IEnumerator RotateWithAngleAxis()
        {
            while (data.rotationType == RotationType.AngleAxis)
            {
                Debug.Log("transform.rotation x Quaternion.AngleAxis()");
                transform.rotation = transform.rotation * Quaternion.AngleAxis(data.rotationAngle, data.axisDirection);
                yield return new WaitForSeconds(data.timeInterval);
            }
            Start();
        }

        private IEnumerator RotateWithAngleAxisInverse()
        {
            while (data.rotationType == RotationType.AngleAxisInverse)
            {
                Debug.Log("transform.rotation x Quaternion.AngleAxis().Inverse()");
                transform.rotation = transform.rotation * Quaternion.AngleAxis(data.rotationAngle, data.axisDirection).Inverse();
                yield return new WaitForSeconds(data.timeInterval);
            }
            Start();
        }

        private void Update()
        {
            if (data.rotationType == RotationType.AngleAxis || data.rotationType == RotationType.AngleAxisInverse)
                Debug.DrawLine(transform.position - data.axisDirection, transform.position + data.axisDirection, Color.green);
        }

        private void SelectCoroutine()
        { 
            switch (data.rotationType)
            {
                case RotationType.Inverse:                      coroutine = Inverse();
                    break;
                case RotationType.TransformRotate:              coroutine = RotateWithTransformRotate();
                    break;
                case RotationType.Euler:                        coroutine = RotateWithEuler();
                    break;
                case RotationType.EulerInverse:                 coroutine = RotateWithEulerInverse();
                    break;
                case RotationType.TransformLookAt:              coroutine = RotateWithTransformLookAt();
                    break;
                case RotationType.LookRotation_with_HeadUp:     coroutine = RotateWithLookRotationWithHeadAlwaysUp();
                    break;
                case RotationType.LookRotation_with_HeadCustom: coroutine = RotateWithLookRotationWithHeadCustomized();
                    break;
                case RotationType.AngleAxis:                    coroutine = RotateWithAngleAxis();
                    break;
                case RotationType.AngleAxisInverse:             coroutine = RotateWithAngleAxisInverse();
                    break;
            }
        }

        private Vector3 Get(Axis axis, float angle)
        { 
            switch (axis)
            {
                case Axis.x: return Vector3.right * angle;
                case Axis.y: return Vector3.up * angle;
                case Axis.z: return Vector3.forward * angle;
                default: return Vector3.zero;
            }
        }

    }
}
