﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// State Conditionals are created to contain the state transition tests. 
/// </summary>
namespace artiMech
{
    public class editorDisplayToLoadConditional : stateConditionalBase
    {
        public editorDisplayToLoadConditional(string changeStateName) : base(changeStateName)
        {

        }

        /// <summary>
        /// Test conditionals are placed here.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>true or false depending if transition conditions are met.</returns>
        public override string UpdateConditionalTest(baseState state)
        {
            string strOut = null;

#if ARTIMECH_THIS_SHOULD_NEVER_BE_TRUE_BUT_IS_AN_EXAMPLE_OF_A_CONDITION_BEING_TRUE
            This is an example of setting a contition to true if the gameobject
            falls below a certain height ingame.
            if (state.m_GameObject.transform.position.y <= 1000)
                strOut = m_ChangeStateName;
#endif
            //if my state list is empty and a game object is populated then go to a load state
            //and see if the gameobject has a statemachine attached to it.
            if (stateEditorUtils.GameObject != stateEditorUtils.WasGameObject && stateEditorUtils.GameObject!=null)
                strOut = m_ChangeStateName;


            return strOut;
        }
    }
}