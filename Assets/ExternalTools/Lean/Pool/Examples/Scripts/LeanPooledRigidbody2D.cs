using UnityEngine;

namespace Lean.Pool
{
	/// <summary>This component will automatically reset a Rigidbody2D when it gets spawned/despawned.</summary>
	[RequireComponent(typeof(Rigidbody2D))]
	[HelpURL(LeanPool.HelpUrlPrefix + "LeanPooledRigidbody2D")]
	public class LeanPooledRigidbody2D : MonoBehaviour
	{
		/// <summary>This method will reset the current Rigidbody2D velocity values.</summary>
		public void ResetVelocity()
		{
			var rigidbody2D = GetComponent<Rigidbody2D>();

			rigidbody2D.velocity        = Vector2.zero;
			rigidbody2D.angularVelocity = 0.0f;
		}

		protected virtual void OnDespawn()
		{
			ResetVelocity();
		}
	}
}