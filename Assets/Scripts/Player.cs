using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private GameObject[] m_Dimensions;
	[SerializeField]
	private string m_PlatformLayer;
	[SerializeField]
	private float m_Speed;
	[SerializeField]
	private float m_JumpForce;

	private Rigidbody2D rb;
	public bool Grounded { get => m_Grounds > 0; }
	private uint m_Grounds;

	private int m_Dimension;
	private int m_PlatformLayerNum;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		m_PlatformLayerNum = LayerMask.NameToLayer(m_PlatformLayer);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			m_Dimensions[m_Dimension].SetActive(false);
			m_Dimension = (m_Dimension + 1) % m_Dimensions.Length;
			m_Dimensions[m_Dimension].SetActive(true);
		}
	}

	private void FixedUpdate()
	{
		float jumpForce = m_JumpForce * (Input.GetAxisRaw("Vertical") > 0 && Grounded ? 1 : 0);
		var accel = new Vector2(Input.GetAxis("Horizontal") * m_Speed, jumpForce);
		rb.AddForce(accel);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == m_PlatformLayerNum)
			m_Grounds++;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.layer == m_PlatformLayerNum)
			m_Grounds--;
	}
}
