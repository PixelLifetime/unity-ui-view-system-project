/* Created by Pixel.Lifetime */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

using TMPro;

public abstract class InputView : View, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	public virtual void OnPointerEnter(PointerEventData eventData) { }
	public virtual void OnPointerExit(PointerEventData eventData) { }

	public virtual void OnPointerDown(PointerEventData eventData) { }
	public virtual void OnPointerUp(PointerEventData eventData) { }

	public virtual void OnPointerClick(PointerEventData eventData) { }

#if UNITY_EDITOR
	//protected override void OnDrawGizmos()
	//{
	//}
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(InputView))]
[CanEditMultipleObjects]
public class InputViewEditor : Editor
{
#pragma warning disable 0219, 414
	private InputView _sInputView;
#pragma warning restore 0219, 414

	private void OnEnable()
	{
		this._sInputView = this.target as InputView;
	}

	public override void OnInspectorGUI()
	{
		this.DrawDefaultInspector();
	}
}
#endif