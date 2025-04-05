// using UnityEngine;
// using UnityEditor;
// using System.Collections;

// [CustomPropertyDrawer(typeof(ArrayLayout))]
// public class CustPropertyDrawer : PropertyDrawer {
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        
//         EditorGUI.PrefixLabel(position, label);
        
        
// 		position.y += 18f;
//         Rect newposition = position;
    
        
        
//         SerializedProperty data = property.FindPropertyRelative("cols");
        
 
        
//         for(int j = 0; j < data.arraySize; j++) {
            
//             SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
            
//             newposition.height = 18f;
            
//             if(row.arraySize != 14)
//                 row.arraySize = 14;
                
//             newposition.width = position.width / 14;
            
//             for(int i = 0; i < row.arraySize; i++) {
                
//                 EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(i), GUIContent.none);
                
                
//                 newposition.y += 18f;
//             }
            
            
//             newposition.y = position.y;
            
            
//             newposition.x += newposition.width;
//         }
//     }
    
//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        
//         return 25f * 12;
//     }
// }