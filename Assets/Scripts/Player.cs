using System.Collections;
using System.Collections.Generic;
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
	public bool Grounded { get => m_Grounds > 0; }

	private int m_Dimension;
	private int m_PlatformLayerNum;

	[SerializeField]
	private float m_LateJump;
	private float m_LastOnPlatform;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		m_PlatformLayerNum = LayerMask.NameToLayer(m_PlatformLayer);
		StartCoroutine(DelayEnableDimension());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			m_Dimensions[m_Dimension].Disable();
			m_Dimension = (m_Dimension + 1) % m_Dimensions.Length;
			m_Dimensions[m_Dimension].Enable();
		}
	}

	private void FixedUpdate()
	{
		float jumpForce = 0;
		if (Grounded)
			jumpForce = m_JumpForce * (Input.GetAxisRaw("Vertical") > 0 ? 1 : 0);
		
		var accel = new Vector2(Input.GetAxis("Horizontal") * m_Speed, jumpForce);
		rb.AddForce(accel);

		if (m_LastOnPlatform < m_LateJump) // <--- Optimus Prime
			m_LastOnPlatform += Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Goal"))
			Debug.Log("Beat Level!");
		else if (other.gameObject.layer == m_PlatformLayerNum)
			m_Grounds++;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.layer == m_PlatformLayerNum)
			m_Grounds--;
	}

	private IEnumerator DelayEnableDimension()
	{
		yield return new WaitForFixedUpdate();
		m_Dimensions[m_Dimension].Enable();
		yield return null;
	}
}
