using System.Collections;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
	public static Player Active { get; private set; }

	public Dimension[] m_Dimensions;
	public float m_Speed;
	public float m_JumpForce;
	public float m_ExplosionForce;
	public float m_ExplosionRadius;

	[SerializeField]
	private ParticleSystem m_System;

	[HideInInspector]
	public Rigidbody2D rb;
	private RigidbodyConstraints2D rbConstraints;
	private uint m_Grounds;
	public bool Grounded { get => m_Grounds > 0 || m_LastOnPlatform < m_LateJump; }
	public bool Playing { get; private set; }

	public int m_Dimension;

	public float m_LateJump;
	private float m_LastOnPlatform;

	public Vector2 m_StartPos;

	[SerializeField]
	private TextMeshProUGUI m_Timer;
	public float m_Time;

	private void Awake()
	{
		Playing = true;
		rb = GetComponent<Rigidbody2D>();
		rbConstraints = rb.constraints;
		StartCoroutine(DelayEnableDimension());

		m_LastOnPlatform = m_LateJump;
		m_StartPos = transform.position;
		Active = this;
	}

	private void Update()
	{
		if (!Playing)
			return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			m_Dimensions[m_Dimension].Disable();
			m_Dimension = (m_Dimension + 1) % m_Dimensions.Length;
			m_Dimensions[m_Dimension].Enable();
		}

		if (Input.GetMouseButtonDown(0))
		{
			m_System.Play();
			var colliders = Physics2D.OverlapCircleAll(transform.position, 15);
			foreach (var obj in colliders)
			{
				if (obj == GetComponent<Collider2D>())
					continue;

				var rb = obj.GetComponent<Rigidbody2D>();
				if (rb != null)
				{
					var diff = obj.transform.position - transform.position;
					float dist = diff.magnitude;
					if (dist < m_ExplosionRadius && dist > 0)
						rb.AddForce(diff.normalized * (m_ExplosionForce * (m_ExplosionRadius - dist)), ForceMode2D.Impulse);
				}
			}
		}
	}

	private void FixedUpdate()
	{
		if (!Playing)
			return;

		if (Grounded && Input.GetAxisRaw("Vertical") > 0)
			Jump();
		
		var accel = new Vector2(Input.GetAxis("Horizontal") * m_Speed, 0);
		rb.AddForce(accel);

		m_Timer.text = m_Time.ToString("n2"); // <--- Untimus Primer
		m_Time += Time.deltaTime;
		if (m_Grounds == 0 && m_LastOnPlatform < m_LateJump) // <--- Optimus Prime
			m_LastOnPlatform += Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!Playing)
			return;

		if (other.CompareTag("Goal"))
			Level.Active.Complete();
		else if (other.HasComponent<Jumpable>())
			m_Grounds++;
		else if (other.HasComponent<Astroid>())
			Disable();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!Playing)
			return;

		if (other.TryGetComponent<Jumpable>(out var unused))
		{
			if (--m_Grounds == 0 && m_LastOnPlatform >= m_LateJump)
				m_LastOnPlatform = 0;
		}
	}

	private IEnumerator DelayEnableDimension()
	{
		yield return new WaitForFixedUpdate();
		m_Dimensions[m_Dimension].Enable();
		yield return null;
	}

	private void Jump()
	{
		float jumpForce = m_JumpForce;
		m_LastOnPlatform = m_LateJump; // No double jump
		rb.AddForce(new Vector2(0, jumpForce));
	}

	public void Retry()
	{
		transform.rotation = Quaternion.identity;
		transform.position = m_StartPos;
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0;
		m_Grounds = 0;

		m_LastOnPlatform = m_LateJump;
		m_Dimensions[m_Dimension].Disable();
		m_Dimension ^= m_Dimension; // Fancy shmancy
		m_Dimensions[m_Dimension].Enable();

		m_Time = 0;
		Enable();
	}

	public void Enable()
	{
		Playing = true;
		rb.constraints = rbConstraints;
	}

	public void Disable()
	{
		Playing = false;
		rb.constraints = RigidbodyConstraints2D.None;
		rb.AddTorque(Random.Range(-5, 5), ForceMode2D.Impulse);
		rb.AddForceAtPosition(Random.insideUnitCircle * 25, transform.position, ForceMode2D.Impulse);
	}
}
