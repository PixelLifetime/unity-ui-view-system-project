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

public class MainMenuView : InputView
{
	[SerializeField] private Transform _cameraLookAtTarget;

	[Header("Follow Pointer Settings")]

	[SerializeField] private Vector2 _followPointerMinRange = new Vector2(-75f, -75f);
	[SerializeField] private Vector2 _followPointerMaxRange = new Vector2(75f, 75f);

	private IEnumerator FollowPointerProcess(PointerEventData eventData)
	{
		while (true)
		{
			this._cameraLookAtTarget.position =
				new Vector3(
					Mathf.Lerp(this._followPointerMinRange.x, this._followPointerMaxRange.x, eventData.position.x / Screen.width),
					Mathf.Lerp(this._followPointerMinRange.y, this._followPointerMaxRange.y, eventData.position.y / Screen.height),
					this._cameraLookAtTarget.position.z
				);

			Debug.Log(this._cameraLookAtTarget.position);

			yield return null;
		}
	}

	private Coroutine _followPointerProcess;

	public override void OnPointerEnter(PointerEventData eventData)
	{
		this._followPointerProcess = this.StartCoroutine(this.FollowPointerProcess(eventData));
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (this._followPointerProcess != null)
			this.StopCoroutine(this._followPointerProcess);
	}

#if UNITY_EDITOR
	//protected override void OnDrawGizmos()
	//{
	//}
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(MainMenuView))]
[CanEditMultipleObjects]
public class MainMenuViewEditor : Editor
{
#pragma warning disable 0219, 414
	private MainMenuView _sMainMenuView;
#pragma warning restore 0219, 414

	private void OnEnable()
	{
		this._sMainMenuView = this.target as MainMenuView;
	}

	public override void OnInspectorGUI()
	{
		this.DrawDefaultInspector();
	}
}
#endif