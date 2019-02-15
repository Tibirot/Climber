using UnityEngine;

namespace Lean.Pool
{
	/// <summary>This component allows you to reset a Rigidbody's velocity via Messages or via Poolable.</summary>
	[RequireComponent(typeof(Rigidbody))]
	[HelpURL(LeanPool.HelpUrlPrefix + "LeanPooledRigidbody")]
	public class LeanPooledRigidbody : MonoBehaviour
	{
		/// <summary>This method will reset the current Rigidbody velocity values.</summary>
		public void ResetVelocity()
		{
			var rigidbody = GetComponent<Rigidbody>();

			rigidbody.velocity        = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
		}

		protected virtual void OnDespawn()
		{
			ResetVelocity();
		}
	}
}