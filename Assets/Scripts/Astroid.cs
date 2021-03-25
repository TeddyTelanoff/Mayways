using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
	public float m_Speed;
	public Vector2 m_Direction;

	[HideInInspector]
	public Rigidbody2D rb;
	public GameObject m_Dust;

	public float m_DespawnDistFromPlayer;

	private void Awake() =>
		rb = GetComponent<Rigidbody2D>();

	private void FixedUpdate()
	{
		rb.AddForce(m_Direction * m_Speed * Time.deltaTime, ForceMode2D.Impulse);

		// Destroy if out of range from player
		if (Vector2.Distance(transform.position, Player.Active.transform.position) >= m_DespawnDistFromPlayer)
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Instantiate(m_Dust, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
