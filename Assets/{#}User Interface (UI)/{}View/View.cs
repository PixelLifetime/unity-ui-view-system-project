/* Created by Pixel.Lifetime */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

using TMPro;

[RequireComponent(typeof(Animator), typeof(CanvasGroup))]
public class View : MonoBehaviour
{
	protected RectTransform rectTransform;
	public RectTransform _RectTransform { get { return this.rectTransform; } }

	[SerializeField] protected RectTransform content;

	public bool Active_ { get; private set; }

	public virtual void Open()
	{
		this.Active_ = true;

		this.gameObject.SetActive(true);
	}

	public virtual void Close()
	{
		this.Active_ = false;

		this.gameObject.SetActive(false);
	}

	public void ToggleActivity()
	{
		if (this.Active_)
			this.Close();
		else
			this.Open();
	}

	private Animator _animator;
	private CanvasGroup _canvasGroup;

	public bool Visible_ { get; private set; }

	[SerializeField] protected bool controlsCursor = false;

	public virtual void Show()
	{
		if (this.controlsCursor)
			Cursor.visible = true;

		this.Visible_ = true;

		this._animator.ResetTrigger("Hide");
		this._animator.SetTrigger("Show");
	}

	public virtual void Hide()
	{
		if (this.controlsCursor)
			Cursor.visible = false;

		this.Visible_ = false;

		this._animator.ResetTrigger("Show");
		this._animator.SetTrigger("Hide");
	}

	public void ToggleVisibility()
	{
		if (this.Visible_)
			this.Hide();
		else
			this.Show();
	}

	[SerializeField] private bool _initiallyActive = true;
	[SerializeField] private bool _useAnimatorForInitialization = true;
	[SerializeField] private bool _initiallyShown = true;

	protected virtual void Awake()
	{
		this._animator = this.GetComponent<Animator>();
		this._canvasGroup = this.GetComponent<CanvasGroup>();

		this.rectTransform = this.transform as RectTransform;
	}

	protected virtual void Start()
	{
		if (this._initiallyActive)
			this.Open();
		else
			this.Close();

		if (this._useAnimatorForInitialization)
		{
			if (this._initiallyShown)
				this.Show();
			else
				this.Hide();
		}
	}

#if UNITY_EDITOR
	private void Reset()
	{
		string typeName = this.GetType().Name;
		this.gameObject.name = (typeName.Equals("View") ? "[View]" : "[View] ") + typeName.Remove(typeName.Length - 4);

		this.content = this.transform.Find("[Content]") as RectTransform;

		if (this.content == null)
		{
			GameObject content = new GameObject("[Content]", typeof(RectTransform));

			content.transform.SetParent(this.transform);

			this.content = content.transform as RectTransform;

			this.content.anchorMin = new Vector2(0f, 0f);
			this.content.anchorMax = new Vector2(1f, 1f);
			
			this.content.offsetMin = Vector2.zero;
			this.content.offsetMax = Vector2.zero;
		}

		if (this.rectTransform == null)
			this.Awake();

		this.rectTransform.anchorMin = new Vector2(0f, 0f);
		this.rectTransform.anchorMax = new Vector2(1f, 1f);

		this.rectTransform.offsetMin = Vector2.zero;
		this.rectTransform.offsetMax = Vector2.zero;
	}

	//protected override void OnDrawGizmos()
	//{
	//}
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(View))]
[CanEditMultipleObjects]
public class ViewEditor : Editor
{
#pragma warning disable 0219, 414
	private View _sView;
#pragma warning restore 0219, 414

	protected Animator animator;
	protected CanvasGroup canvasGroup;

	protected virtual void DrawCreateAnimatorController(string gameObjectName)
	{
		if (GUILayout.Button("Create Animator Controller"))
		{
			DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.Combine("Assets/_Specific/#Animator Controllers", "#" + gameObjectName));

			string controllerPath = string.Format("Assets/_Specific/#Animator Controllers/#{0}/{0}.controller", gameObjectName);

			AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);

			if (controller == null)
			{
				controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

				// Add parameters
				controller.AddParameter("Show", AnimatorControllerParameterType.Trigger);
				controller.AddParameter("Hide", AnimatorControllerParameterType.Trigger);

				AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;

				// Create states
				AnimatorState stateEmpty = rootStateMachine.AddState("Empty");
				AnimatorState stateShown = rootStateMachine.AddState("Shown");
				AnimatorState stateHidden = rootStateMachine.AddState("Hidden");

				// Create and assign Show animation clip
				AnimationClip animationClipShow = AssetDatabase.LoadAssetAtPath<AnimationClip>(string.Format("Assets/_Specific/#Animator Controllers/#{0}/{1}.anim", gameObjectName, gameObjectName + " Show Animation"));

				if (animationClipShow == null)
				{
					AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 0.5f, 1f);

					animationClipShow = new AnimationClip
					{
						name = gameObjectName + " Show Animation"
					};

					animationClipShow.SetCurve(string.Empty, typeof(CanvasGroup), "m_Alpha", curve);
					animationClipShow.SetCurve(string.Empty, typeof(CanvasGroup), "m_Interactable", curve);
					animationClipShow.SetCurve(string.Empty, typeof(CanvasGroup), "m_BlocksRaycasts", curve);

					AssetDatabase.CreateAsset(animationClipShow, string.Format("Assets/_Specific/#Animator Controllers/#{0}/{1}.anim", gameObjectName, animationClipShow.name));
				}

				stateShown.motion = animationClipShow;

				// Create and assign Hidden animation clip
				AnimationClip animationClipHidden = AssetDatabase.LoadAssetAtPath<AnimationClip>(string.Format("Assets/_Specific/#Animator Controllers/#{0}/{1}.anim", gameObjectName, gameObjectName + " Hide Animation"));

				if (animationClipHidden == null)
				{
					AnimationCurve curve = AnimationCurve.Linear(0f, 1f, 0.5f, 0f);

					animationClipHidden = new AnimationClip
					{
						name = gameObjectName + " Hide Animation"
					};

					animationClipHidden.SetCurve(string.Empty, typeof(CanvasGroup), "m_Alpha", curve);
					animationClipHidden.SetCurve(string.Empty, typeof(CanvasGroup), "m_Interactable", curve);
					animationClipHidden.SetCurve(string.Empty, typeof(CanvasGroup), "m_BlocksRaycasts", curve);

					AssetDatabase.CreateAsset(animationClipHidden, string.Format("Assets/_Specific/#Animator Controllers/#{0}/{1}.anim", gameObjectName, animationClipHidden.name));
				}

				stateHidden.motion = animationClipHidden;

				// Create transition from empty to shown and hidden states
				AnimatorStateTransition emptyToShownStateTransition = stateEmpty.AddTransition(stateShown);
				emptyToShownStateTransition.hasFixedDuration = false;
				emptyToShownStateTransition.exitTime = 0f;
				emptyToShownStateTransition.duration = 0f;
				emptyToShownStateTransition.AddCondition(AnimatorConditionMode.If, 0f, "Show");

				AnimatorStateTransition emptyToHiddenStateTransition = stateEmpty.AddTransition(stateHidden);
				emptyToHiddenStateTransition.hasFixedDuration = false;
				emptyToHiddenStateTransition.exitTime = 0f;
				emptyToHiddenStateTransition.duration = 0f;
				emptyToHiddenStateTransition.AddCondition(AnimatorConditionMode.If, 0f, "Hide");

				// Create transition from shown to hidden state
				AnimatorStateTransition shownToHiddenStateTransition = stateShown.AddTransition(stateHidden);
				shownToHiddenStateTransition.hasFixedDuration = false;
				shownToHiddenStateTransition.exitTime = 0f;
				shownToHiddenStateTransition.duration = 0f;
				shownToHiddenStateTransition.AddCondition(AnimatorConditionMode.If, 0f, "Hide");

				// Create transition from hidden to shown state
				AnimatorStateTransition hiddenToShownStateTransition = stateHidden.AddTransition(stateShown);
				hiddenToShownStateTransition.hasFixedDuration = false;
				hiddenToShownStateTransition.exitTime = 0f;
				hiddenToShownStateTransition.duration = 0f;
				hiddenToShownStateTransition.AddCondition(AnimatorConditionMode.If, 0f, "Show");

				// Assign controller to Animator
				this.animator.runtimeAnimatorController = controller;
			}
		}
	}

	protected virtual void OnEnable()
	{
		this._sView = this.target as View;

		this.animator = this._sView.GetComponent<Animator>();
		this.canvasGroup = this._sView.GetComponent<CanvasGroup>();
	}

	public override void OnInspectorGUI()
	{
		this.DrawDefaultInspector();

		if (this.animator.runtimeAnimatorController == null)
			this.DrawCreateAnimatorController(this.target.name);
	}
}
#endif