
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using Vexe.Editor.Windows;

namespace Bonsai.Designer
{
    /*
     * This property drawer uses a selection window to make
     * it easier to select a key. Unity's default enum popup creates a 
     * a very long list making it difficult to navigate and pick a key.
     * */
    [CustomPropertyDrawer(typeof(KeyCode))]
    public class KeyCodeDrawer : PropertyDrawer
    {
        private KeyCode _selectedKey = KeyCode.None;
        private bool _bChangeKey = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Display the property name.
            EditorGUI.LabelField(position, ObjectNames.NicifyVariableName(property.name));

            // Unity SerializedProperty has no direct way to set an enum, it uses an index value
            // which is unrelated to the KeyCode enumeration. The index value is the order of the enum
            // in the default pop-up list. KeyCode enumeration might follow something like ASCII (unsure if 100% true).
            //
            // To bypass this, we can get the names of the KeyCode enums as a string array which are in the same order
            // as the popup list order, this order is the SerializedProperty.enumValueIndex.

            // To get the true KeyCode from a serialized property we do:
            
            // Get access to the KeyCode enums as a string array of enum names.
            string[] enums = property.enumNames;

            // Get the current KeyCode name held by the serialized property by accessing the array value at enumValueIndex.
            string propEnumName = enums[property.enumValueIndex];

            // Get the KeyCode enum by converting the name to KeyCode.
            var propKey = (KeyCode)Enum.Parse(typeof(KeyCode), propEnumName);

            // Only change the keys if they differ from the input selection and make sure
            // not to override it with the default value.
            if (_selectedKey != propKey && _selectedKey != KeyCode.None) {
                _bChangeKey = true;
            }

            string keyName = Enum.GetName(typeof(KeyCode), propKey);

            // Offset the button from the label.
            position.x += 120f;
            position.width = 80f;

            // Display the button that activates the selection window.
            if (EditorGUI.DropdownButton(position, new GUIContent(keyName), FocusType.Keyboard)) {
                selectKey();
            }

            // Apply changes if necessary, this way, the default _selectedKey value
            // does not incorrectly override the property enum value.
            if (_bChangeKey) {

                // Since we cannot set the enum value of the serialized property directly,
                // we need to get the enumValueIndex associated to the KeyCode name.
                //
                // To do this, we convert the selected key code to its string name and
                // then find its index position in SerializedProperty.enumNames (variable string[] enums)
                string _selectedKeyName = Enum.GetName(typeof(KeyCode), _selectedKey);

                int index = 0;

                // Find the index of the selected key name.
                foreach (string enumName in enums) {

                    if (enumName == _selectedKeyName) {
                        break;
                    }

                    index++;
                }

                // Set the index which in turn sets the correct key code enum.
                property.enumValueIndex = index;

                _bChangeKey = false;

                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndProperty();
        }

        // Display a menu to select key codes.
        private void selectKey()
        {
            // Get all the keycodes
            var keyCodes = Enum.GetValues(typeof(KeyCode));
            var keys = new KeyCode[keyCodes.Length];

            // Set all the keycode values in the array in order to feed it into the selection window.
            int i = 0;
            foreach (KeyCode k in keyCodes) {
                keys[i++] = k;
            }

            // Display the selection window to pick a keycode.
            SelectionWindow.Show(new Tab<KeyCode>(

                getValues: () => keys,
                getCurrent: () => _selectedKey,
                setTarget: key => { _selectedKey = key; },

                getValueName: key => Enum.GetName(typeof(KeyCode), key),
                title: "Keys"
            ));
        }
    }
}