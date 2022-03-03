﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoR2;
using RoR2EditorKit.Core.Inspectors;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(CharacterBody))]
    public class CharacterBodyInspector : ComponentInspector<CharacterBody>
    {
        private VisualElement inspectorData;

        protected override VisualElement DrawInspectorGUI()
        {
            VisualElement element = new VisualElement();

            inspectorData = Find<VisualElement>("inspectorData");
            element.Add(inspectorData);

            var tokenSetter = Find<Button>(inspectorData, "tokenSetter");
            tokenSetter.clicked += SetTokens;

            var rootMotionInState = Find<Toggle>(inspectorData, "rootMotion");
            rootMotionInState.RegisterValueChangedCallback(OnRootMotionSet);
            OnRootMotionSet();

            return element;
        }

        private void SetTokens()
        {
            if (Settings.MainManifest && Settings.TokenPrefix.IsNullOrEmptyOrWhitespace())
            {
                throw ErrorShorthands.ThrowNullTokenPrefix();
            }

            GameObject go = TargetType.gameObject;
            if(go)
            {
                TargetType.baseNameToken = $"{Settings.TokenPrefix.ToUpperInvariant()}_{go.name.ToUpperInvariant()}_NAME";
                TargetType.subtitleNameToken = $"{Settings.TokenPrefix.ToUpperInvariant()}_{go.name.ToUpperInvariant()}_SUBTITLE";
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        private void OnRootMotionSet(ChangeEvent<bool> evt = null)
        {
            var rootSpeed = Find<FloatField>(inspectorData, "mainRootSpeed");
            bool value = evt == null ? Find<Toggle>(inspectorData, "rootMotion").value : evt.newValue;
            rootSpeed.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
