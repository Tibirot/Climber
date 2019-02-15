using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombuDemoPanel : MonoBehaviour {

	public Animator initiallyOpen;
	
	private int m_OpenParameterId;
	private Animator m_Open;
	
	const string k_OpenTransitionName = "Open";
	const string k_ClosedStateName = "Closed";

	public Animator Current
	{
		get {
			return m_Open;
		}
	}
	
	public virtual void OnEnable()
	{
		m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);
		
		if (initiallyOpen == null)
			return;
		
		OpenPanel(initiallyOpen);
	}
	
	public virtual void OpenPanel (Animator anim)
	{
		if (m_Open == anim)
			return;
		
		anim.gameObject.SetActive(true);
		
		anim.transform.SetAsLastSibling();
		
		CloseCurrent();
		
		m_Open = anim;
		m_Open.SetBool(m_OpenParameterId, true);
	}
	
	static GameObject FindFirstEnabledSelectable (GameObject gameObject)
	{
		GameObject go = null;
		var selectables = gameObject.GetComponentsInChildren<Selectable> (true);
		foreach (var selectable in selectables) {
			if (selectable.IsActive () && selectable.IsInteractable ()) {
				go = selectable.gameObject;
				break;
			}
		}
		return go;
	}
	
	public virtual void CloseCurrent()
	{
		if (m_Open == null)
			return;
		
		m_Open.SetBool(m_OpenParameterId, false);
		StartCoroutine(DisablePanelDelayed(m_Open));
		m_Open = null;
	}
	
	IEnumerator DisablePanelDelayed(Animator anim)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		while (!closedStateReached && wantToClose)
		{
			if (!anim.IsInTransition(0))
				closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);
			
			wantToClose = !anim.GetBool(m_OpenParameterId);
			
			yield return new WaitForEndOfFrame();
		}
		
		if (wantToClose)
			anim.gameObject.SetActive(false);
	}
}
