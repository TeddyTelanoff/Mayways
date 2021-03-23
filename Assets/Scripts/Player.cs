using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private Dimension[] m_Dimensions;
	[SerializeField]
	private string m_PlatformLayer;
	[SerializeField]
	private float m_Speed;
	[SerializeField]
	private float m_JumpForce;

	private Rigidbody2D rb;
	private uint m_Grounds;
	public bool Grounded { get => m_Grounds > 0 || m_LastOnPlatform < m_LateJump; }
	public bool Playing { get; private set; }

	private int m_Dimension;
	private int m_PlatformLayerNum;

	[SerializeField]
	private float m_LateJump;
	private float m_LastOnPlatform;

	private void Awake()
	{
		Playing = true;
		rb = GetComponent<Rigidbody2D>();
		m_PlatformLayerNum = LayerMask.NameToLayer(m_PlatformLayer);
		StartCoroutine(DelayEnableDimension());
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
	}

	private void FixedUpdate()
	{
		if (!Playing)
			return;

		float jumpForce = 0;
		if (Grounded)
		{
			jumpForce = m_JumpForce * (Input.GetAxisRaw("Vertical") > 0 ? 1 : 0);
			if (jumpForce > 0)
				m_LastOnPlatform = m_LateJump; // No double jump
		}
		
		var accel = new Vector2(Input.GetAxis("Horizontal") * m_Speed, jumpForce);
		rb.AddForce(accel);

		if (m_Grounds == 0 && m_LastOnPlatform < m_LateJump) // <--- Optimus Prime
			m_LastOnPlatform += Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!Playing)
			return;

		if (other.CompareTag("Goal"))
			Level.Active.Complete();
		else if (other.gameObject.layer == m_PlatformLayerNum)
			m_Grounds++;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!Playing)
			return;

		if (other.gameObject.layer == m_PlatformLayerNum)
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

	public void Disable()
	{
		Playing = false;
		rb.constraints = RigidbodyConstraints2D.None;
		rb.AddTorque(Random.Range(-5, 5), ForceMode2D.Impulse);
		rb.AddForceAtPosition(Random.insideUnitCircle * 25, transform.position, ForceMode2D.Impulse);
	}
}
