using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//This attribute tells Unity that this class should be used to draw the inspector for our custom class
[CustomPropertyDrawer(typeof(FractalNoise))]
public class FractalNoisePropertyDrawer : PropertyDrawer
{
	//We use this if we need more than one row, or we need to dynamically determine how big the property should be in the inspector
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		//two lines, plus two lots of spacing
		return 3f * EditorGUIUtility.singleLineHeight + 2f * EditorGUIUtility.standardVerticalSpacing;
	}
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
	        //We must begin with this
            EditorGUI.BeginProperty(position, label, property);

            //This is just a nice way to draw a box around our content, we could add padding and stff if we wanted
            EditorGUI.HelpBox(position, "", MessageType.None);
            
            //Yep, dealing with rects is super fun hooray :/
            var subWidth = position.width * 0.5f;
            var x1 = position.x;
            var x2 = position.x + subWidth;
            var y1 = position.y;
            var y2 = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var y3 = position.y + EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing;
            var rects = new[]
            {
	            new Rect(x1, y1, position.width, EditorGUIUtility.singleLineHeight),
	            new Rect(x1, y2, subWidth, EditorGUIUtility.singleLineHeight),
	            new Rect(x2, y2, subWidth, EditorGUIUtility.singleLineHeight),
	            new Rect(x1, y3, position.width, EditorGUIUtility.singleLineHeight),
            };
            
            //This is the important part - we get the *relative* property on the serialized class and draw a control for it
            EditorGUI.IntSlider(rects[0], property.FindPropertyRelative("Octaves"), 1, 5, new GUIContent("Octaves"));
            EditorGUI.PropertyField(rects[1], property.FindPropertyRelative("Frequency"), new GUIContent("Frequency"));
            EditorGUI.PropertyField(rects[2], property.FindPropertyRelative("FrequencyMultiplier"), new GUIContent("Frequency Multiplier"));
            EditorGUI.Slider(rects[3], property.FindPropertyRelative("AmplitudeMultiplier"), 0f, 1f, new GUIContent("Amplitude Multiplier"));

            //We must end with this
            EditorGUI.EndProperty();
        }
}
