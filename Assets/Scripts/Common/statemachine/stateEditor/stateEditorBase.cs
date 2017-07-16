﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace artiMech
{
    public class stateEditorBase : EditorWindow
    {
#region Variables 

        protected bool m_DisplayStates = false;
        protected string m_CurrentStateName = "";

        protected IList<baseState> m_StateList;
        protected baseState m_CurrentState = null;
        protected stateChanger m_StateChanger;

        //protected Vector2 m_MousePos;

#endregion


#region Accessors

        public baseState CurrentState
        {
            get
            {
                return m_CurrentState;
            }
        }

#endregion

        #region Member Functions
        public stateEditorBase()
        {
            m_StateChanger = new stateChanger();
            m_StateList = new List<baseState>();

            m_CurrentState = AddState(new editorStartState(null), "Start");
            AddState(new editorDisplayWindowsState(null), "DisplayWindows");
        }

        [MenuItem("Window/ArtiMech/State Editor")]
        static void ShowEditor()
        {
            EditorWindow.GetWindow<stateEditor>();
        }

        // Update is called once per frame
        protected void Update()
        {
            if (m_CurrentState == null)
            {
                Debug.LogWarning(this.name + " stateEditorBase doesn't have an m_CurrentState.");
                return;
            }

            baseState state = m_StateChanger.UpdateChangeStates(m_StateList, m_CurrentState, null, m_DisplayStates);
            if (state != null)
                m_CurrentState = state;

            m_CurrentState.Update();
            m_CurrentStateName = m_CurrentState.m_StateName;
        }

        protected void OnGUI()
        {

        }

        protected void FixedUpdate()
        {
            if (m_CurrentState == null)
                return;

            m_CurrentState.FixedUpdate();
        }

        public baseState AddState(baseState state, string statename)
        {
            state.m_StateName = statename;
            m_StateList.Add(state);
            return state;
        }

        public void ForceChangeState(string stateName)
        {
            m_CurrentState = m_StateChanger.ForceChangeState(m_StateList, m_CurrentState, stateName, null, m_DisplayStates);
        }

        #endregion
    }
}