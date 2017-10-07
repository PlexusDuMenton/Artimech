﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <name>nada</name>
    <posX>20</posX>
    <posY>40</posY>
    <sizeX>150</sizeX>
    <sizeY>80</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace artiMech
{
    public class editorAddConditionalState : baseState
    {

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        IList<stateConditionalBase> m_ConditionalList;
        static Texture2D m_BackGroundImage = null;
        stateWindowsNode m_WindowsSelectedNode = null;
        bool m_RightClickBool = false;
        bool m_CondtionCreatedBool = false;

        public bool RightClickBool { get { return m_RightClickBool; } }
        public bool ConditionCreatedBool { get { return m_CondtionCreatedBool; } }

        public editorAddConditionalState(GameObject gameobject)
        {
            m_GameObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editorAddConditionalToDisplay("Display Windows"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            for (int i = 0; i < m_ConditionalList.Count; i++)
            {
                string changeNameToThisState = null;
                changeNameToThisState = m_ConditionalList[i].UpdateConditionalTest(this);
                if (changeNameToThisState != null)
                {
                    m_ChangeStateName = changeNameToThisState;
                    m_ChangeBool = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Fixed Update for physics and such from the game object.
        /// </summary>
        public override void FixedUpdate()
        {

        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {

            Event ev = Event.current;
            stateEditorUtils.MousePos = ev.mousePosition;

            //check to see if 
            stateWindowsNode stateNode = GetWindowsNodeAtThisLocation(ev.mousePosition);
            if (Event.current.button == 0 && stateNode!=null && m_CondtionCreatedBool==false)
            {
                m_CondtionCreatedBool = true;
                stateEditorUtils.CreateConditionalAndAddToState(stateEditorUtils.SelectedNode.WindowTitle, stateNode.WindowTitle);
                
                //Debug.Log("window = " + stateNode.WindowTitle);
            }

            if (ev.button == 1)
            {
                if (ev.type == EventType.MouseDown)
                {
                    m_RightClickBool = true;
                }
            }

            DrawConditionCurve();

            for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
            {
                stateEditorUtils.StateList[i].Update(this);
            }


        }

        stateWindowsNode GetWindowsNodeAtThisLocation(Vector2 vect)
        {
            for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
            {
                if(stateEditorUtils.StateList[i].IsWithin(vect))
                {
                    return stateEditorUtils.StateList[i];
                }
            }

            return null;
        }

        void DrawConditionCurve()
        {
            Vector3 startPos = m_WindowsSelectedNode.GetPos();
            startPos.x += m_WindowsSelectedNode.WinRect.width * 0.5f;
            startPos.y += m_WindowsSelectedNode.WinRect.height * 0.5f;
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = stateEditorUtils.MousePos + Vector3.left * 50;
            Handles.DrawBezier(startPos, stateEditorUtils.MousePos, startTan, endTan, Color.black, null, 1);

            //draw shadow
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++) 
                Handles.DrawBezier(startPos, stateEditorUtils.MousePos, startTan, endTan, shadowCol, null, (i + 1) * 4);
        }

        void ContextCallback(object obj)
        {
            stateEditorUtils.ContextCallback(obj);
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_WindowsSelectedNode = stateEditorUtils.SelectedNode;
            m_RightClickBool = false;
            m_CondtionCreatedBool = false;
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

        }
    }
}