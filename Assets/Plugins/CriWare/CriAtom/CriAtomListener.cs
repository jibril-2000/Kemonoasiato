/****************************************************************************
 *
 * Copyright (c) 2011 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

using UnityEngine;
using System;
using System.Collections;

/// \addtogroup CRIATOM_UNITY_COMPONENT
/// @{

/**
 * <summary>3Dリスナーを表すコンポーネントです。</summary>
 * <remarks>
 * <para header='説明'>
 * 通常、カメラやメインキャラクタのGameObjectに付与して使用します。
 * 現在位置の更新は自動的に行われるため、特に操作や設定を行う必要はありません。
 * </para>
 * </remarks>
 */
[AddComponentMenu("CRIWARE/CRI Atom Listener")]
public class CriAtomListener : CriMonoBehaviour
{
	#region CRIWARE internals
	public static CriAtomListener activeListener {
		get; private set;
	}

	public static CriAtomEx3dListener sharedNativeListener {
		get; private set;
	}

	public static void CreateSharedNativeListener()
	{
		if (sharedNativeListener == null) {
			sharedNativeListener = new CriAtomEx3dListener();
		}
	}

	public static void DestroySharedNativeListener()
	{
		if (sharedNativeListener != null) {
			sharedNativeListener.Dispose();
			sharedNativeListener = null;
		}
	}
	#endregion

	#region Fields & Properties
	[SerializeField] CriAtomRegion regionOnStart = null;

	/**
	 * <summary>OnEnable 時に常にアクティブリスナーにするか</summary>
	 * <remarks>
	 * <para header='説明'>
	 * true の場合、 OnEnable 時に他のリスナーがアクティブな場合でもアクティブリスナーになります。
	 * false の場合、アクティブリスナーが存在しない場合のみアクティブリスナーになります。
	 * </para>
	 * </remarks>
	 */
	public bool activateListenerOnEnable = false;

	/**
	 * <summary>音源の3Dリージョンの設定及び取得</summary>
	 */
	public CriAtomRegion region3d
	{
		get { return currentRegion; }
		set {
			CriAtomEx3dRegion regionHandle = (value == null) ? null : value.region3dHn;
			if (sharedNativeListener != null) {
				sharedNativeListener.Set3dRegion(regionHandle);
				sharedNativeListener.Update();
				this.currentRegion = value;
			} else {
				Debug.LogError("[CRIWARE] Internal: CriAtomListener is not initialized correctly.");
				this.currentRegion = null;
			}
		}
	}
	#endregion

	#region Internal Variables
	private Vector3 lastPosition;
	private CriAtomRegion currentRegion = null;
	#endregion

	#region Functions
	private void Start()
	{
		if (regionOnStart != null) {
			region3d = this.regionOnStart;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if ((activeListener == null) || activateListenerOnEnable) {
			ActivateListener();
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (activeListener == this) {
			if (sharedNativeListener != null) {
				sharedNativeListener.ResetParameters();
				sharedNativeListener.Update();
			}
			activeListener = null;
		}
	}

	private void OnDrawGizmos()
	{
		if (this.enabled == false) { return; }
		var criWareLightBlue = new Color(0.332f, 0.661f, 0.991f);

		Gizmos.color = criWareLightBlue;
		Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward);
		Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.up);
#if UNITY_EDITOR
		UnityEditor.Handles.color = criWareLightBlue;
		UnityEditor.Handles.ArrowHandleCap(1, this.transform.position + this.transform.forward, this.transform.rotation, 1f, EventType.Repaint);
		UnityEditor.Handles.RectangleHandleCap(1, this.transform.position, this.transform.rotation * Quaternion.LookRotation(Vector3.up), 1f, EventType.Repaint);
#endif
	}

	public override void CriInternalUpdate() { }

	public override void CriInternalLateUpdate()
	{
		if (activeListener != this) {
			return;
		}
		Vector3 position = this.transform.position;
		Vector3 velocity = (position - this.lastPosition) / Time.deltaTime;
		Vector3 front    = this.transform.forward;
		Vector3 up       = this.transform.up;
		this.lastPosition = position;
		if (sharedNativeListener != null) {
			sharedNativeListener.SetPosition(position.x, position.y, position.z);
			sharedNativeListener.SetVelocity(velocity.x, velocity.y, velocity.z);
			sharedNativeListener.SetOrientation(front.x, front.y, front.z, up.x, up.y, up.z);
			sharedNativeListener.Update();
		}
	}
	#endregion

	/**
	 * <summary>アクティブリスナーにする</summary>
	 * <remarks>
	 * <para header='説明'>
	 * アクティブリスナーになると、 ::CriAtomSource の3Dリスナーとして動作します。
	 * </para>
	 * </remarks>
	 */
	public void ActivateListener()
	{
		activeListener = this;

		Vector3 position = this.transform.position;
		Vector3 front    = this.transform.forward;
		Vector3 up       = this.transform.up;
		this.lastPosition = position;
		if (sharedNativeListener != null) {
			sharedNativeListener.SetPosition(position.x, position.y, position.z);
			sharedNativeListener.SetVelocity(0.0f, 0.0f, 0.0f);
			sharedNativeListener.SetOrientation(front.x, front.y, front.z, up.x, up.y, up.z);
			sharedNativeListener.Update();
		}

		this.region3d = this.currentRegion;
	}
} // end of class

/// @}
/* end of file */
