using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
	[HideInInspector]
	public ParticleSystem particle;

	private void Awake()
	{
		particle = GetComponent<ParticleSystem>();
		StartCoroutine(Blausgdh());
	}

	private IEnumerator Blausgdh()
	{
		yield return new WaitForSeconds(particle.main.duration);
		Destroy(gameObject);
		yield return null;
	}
}
