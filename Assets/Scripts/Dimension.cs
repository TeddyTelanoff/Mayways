using UnityEngine;

public class Dimension : MonoBehaviour
{
	[SerializeField]
	private float m_Alpha;

	public bool m_Enabled { get; private set; }

	private void Awake() =>
		Disable();

	public void Enable()
	{
		m_Enabled = true;

		var renderers = GetComponentsInChildren<Renderer>();
		foreach (var renderer in renderers)
		{
			var color = renderer.material.color;
			renderer.material.color = new Color(color.r, color.g, color.b, 1);
		}

		var colliders = GetComponentsInChildren<Collider2D>();
		foreach (var collider in colliders)
			collider.enabled = true;
	}

	public void Disable()
	{
		m_Enabled = false;

		var renderers = GetComponentsInChildren<Renderer>();
		foreach (var renderer in renderers)
		{
			var color = renderer.material.color;
			renderer.material.color = new Color(color.r, color.g, color.b, m_Alpha);
		}

		var colliders = GetComponentsInChildren<Collider2D>();
		foreach (var collider in colliders)
			collider.enabled = false;
	}
}
