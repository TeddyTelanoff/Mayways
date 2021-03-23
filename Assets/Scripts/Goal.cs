using UnityEngine;

public class Goal : MonoBehaviour
{
	[SerializeField]
	private Vector3 m_Direction;
	[SerializeField]
	private float m_Speed;

	private void FixedUpdate()
	{
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + m_Direction * m_Speed * Time.deltaTime);
	}
}
