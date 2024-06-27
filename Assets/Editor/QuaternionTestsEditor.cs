using System;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using static QuaternionTests.QuaternionTestData;

namespace QuaternionTests
{
    // View
    [CustomEditor(typeof(QuaternionTests))]
    public class QuaternionTestsEditor : Editor
    {
        private QuaternionTests tests;
        private SynchedEnumField typeField;
        private SynchedEnumField axisField;
        private FloatField timeIntervalField;
        private FloatField rotationAngleField;
        private ObjectField lookTargetField;
        private ObjectField upTargetField;
        private Vector3Field axisDirectionField;
        private Button resetButton;

        public override VisualElement CreateInspectorGUI()
        {
            tests = (QuaternionTests)target;
            var root = new VisualElement();
        
            typeField = new SynchedEnumField(tests.data.rotationType, root);
            typeField.onChange += () => {
                tests.data.rotationType = (RotationType)typeField.value;
                EditorUtility.SetDirty(tests.data);
                AssetDatabase.SaveAssetIfDirty(tests.data);
                Refresh();
            };

            axisField = new SynchedEnumField(tests.data.axis, root);
            axisField.style.display = DisplayStyle.None;
            axisField.onChange += () => { 
                tests.data.axis = (Axis)axisField.value;
                EditorUtility.SetDirty(tests.data);
                AssetDatabase.SaveAssetIfDirty(tests.data);
                Refresh();
            };

            timeIntervalField = new FloatField("Time Interval: ");
            timeIntervalField.style.display = DisplayStyle.None;
            timeIntervalField.value = tests.data.timeInterval;
            timeIntervalField.RegisterValueChangedCallback((evt) => tests.data.timeInterval = evt.newValue);

            rotationAngleField = new FloatField("Rotation Angle: ");
            rotationAngleField.style.display = DisplayStyle.None;
            rotationAngleField.value = tests.data.rotationAngle;
            rotationAngleField.RegisterValueChangedCallback((evt) => tests.data.rotationAngle = evt.newValue);

            lookTargetField = new ObjectField("Look Target: ");
            lookTargetField.style.display = DisplayStyle.None;
            lookTargetField.value = tests.lookTarget;

            upTargetField = new ObjectField("Up Target: ");
            upTargetField.style.display = DisplayStyle.None;
            upTargetField.value = tests.upTarget;

            axisDirectionField = new Vector3Field("Axis Direction: ");
            axisDirectionField.style.display = DisplayStyle.None;
            axisDirectionField.value = tests.data.axisDirection;
            axisDirectionField.RegisterValueChangedCallback(evt => tests.data.axisDirection = evt.newValue);

            resetButton = new Button(() => tests.transform.rotation = Quaternion.identity);
            resetButton.text = "Reset";

            root.Add(typeField);
            root.Add(axisField);
            root.Add(timeIntervalField);
            root.Add(rotationAngleField);
            root.Add(lookTargetField);
            root.Add(upTargetField);
            root.Add(axisDirectionField);
            root.Add(resetButton);
            Refresh();
            return root;
        }


        private void Refresh()
        {
            if (((RotationType)typeField.value &
                    (RotationType.AngleAxis | RotationType.AngleAxisInverse)) != 0)
                axisDirectionField.style.display = DisplayStyle.Flex;
            else axisDirectionField.style.display = DisplayStyle.None;

            if (((RotationType)typeField.value &
                    (RotationType.AngleAxis | RotationType.AngleAxisInverse | RotationType.TransformRotate | RotationType.TransformLookAt |
                    RotationType.Euler | RotationType.EulerInverse | RotationType.LookRotation_with_HeadUp | RotationType.LookRotation_with_HeadCustom)) != 0)
                timeIntervalField.style.display = DisplayStyle.Flex;
            else timeIntervalField.style.display = DisplayStyle.None;

            if ((RotationType)typeField.value == RotationType.Inverse)
                timeIntervalField.style.display = DisplayStyle.Flex;

            if (((RotationType)typeField.value &
                (RotationType.AngleAxis | RotationType.AngleAxisInverse | RotationType.TransformRotate |
                RotationType.Euler | RotationType.EulerInverse)) != 0)
                rotationAngleField.style.display = DisplayStyle.Flex;
            else rotationAngleField.style.display = DisplayStyle.None;

            if (((RotationType)typeField.value & (RotationType.TransformRotate | RotationType.Euler | RotationType.EulerInverse)) != 0)
                axisField.style.display = DisplayStyle.Flex;
            else axisField.style.display = DisplayStyle.None;

            if (((RotationType)typeField.value & (RotationType.LookRotation_with_HeadCustom)) != 0)
                upTargetField.style.display = DisplayStyle.Flex;
            else upTargetField.style.display = DisplayStyle.None;

            if (((RotationType)typeField.value & (RotationType.TransformLookAt | RotationType.LookRotation_with_HeadUp | RotationType.LookRotation_with_HeadCustom)) != 0)
                lookTargetField.style.display = DisplayStyle.Flex;
            else lookTargetField.style.display = DisplayStyle.None;
        }

        public class SynchedEnumField : EnumField 
        {
            public event Action onChange;
            public SynchedEnumField(Enum source, VisualElement root)
            { 
                label = source.GetType().Name;
                Init(source);
                RegisterCallback<ChangeEvent<Enum>>((evt) => { 
                    source = evt.newValue;
                    onChange?.Invoke();
                    root.MarkDirtyRepaint();
                });
            }
        }
    }
}