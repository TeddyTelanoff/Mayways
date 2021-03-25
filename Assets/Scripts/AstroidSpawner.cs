using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidSpawner : MonoBehaviour
{
	public GameObject m_Prefab;
	public Vector2 m_Bounds;
	public Vector2 m_AngleRange;
	public float m_SpawnDelay;

	private void Awake() =>
		StartCoroutine(Spawn());

	private IEnumerator Spawn()
	{
		while (true)
		{
			var pos = new Vector2(Random.Range(m_Bounds.x, m_Bounds.y), transform.position.y);
			var obj = Instantiate(m_Prefab, pos, Quaternion.identity);
			var astroid = obj.GetComponent<Astroid>();
			float ang = Random.Range(m_AngleRange.x, m_AngleRange.y);
			astroid.m_Direction = FromAngle(ang);
			yield return new WaitForSeconds(m_SpawnDelay);
		}
	}

	private static Vector2 FromAngle(float ang) =>
		new Vector2(Mathf.Sin(Mathf.Deg2Rad * ang), Mathf.Cos(Mathf.Deg2Rad * ang));
}
