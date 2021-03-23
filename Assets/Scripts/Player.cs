using System.Collections;
using UnityEngine;
using TMPro;

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

	private RigidbodyConstraints2D rbConstraints;
	private Rigidbody2D rb;
	private uint m_Grounds;
	public bool Grounded { get => m_Grounds > 0 || m_LastOnPlatform < m_LateJump; }
	public bool Playing { get; private set; }

	private int m_Dimension;
	private int m_PlatformLayerNum;

	[SerializeField]
	private float m_LateJump;
	private float m_LastOnPlatform;

	private Vector2 m_StartPos;

	[SerializeField]
	private TextMeshProUGUI m_Timer;
	private float m_Time;

	private void Awake()
	{
		Playing = true;
		rb = GetComponent<Rigidbody2D>();
		rbConstraints = rb.constraints;
		m_PlatformLayerNum = LayerMask.NameToLayer(m_PlatformLayer);
		StartCoroutine(DelayEnableDimension());

		m_LastOnPlatform = m_LateJump;
		m_StartPos = transform.position;
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
