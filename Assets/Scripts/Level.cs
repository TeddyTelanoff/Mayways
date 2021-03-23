using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	public static List<Level> Loaded { get => s_Loaded; }
	public static Level Active { get; private set; }
	private static List<Level> s_Loaded = new List<Level>();

	[SerializeField]
	private Player m_Player;
	[SerializeField]
	private GameObject m_CompleteScreen;
	[SerializeField]
	private bool m_Active;

	private void Awake()
	{
		s_Loaded.Add(this);
		if (m_Active)
			Active = this;
	}

	public void Complete()
	{
		m_Player.Disable();
		m_CompleteScreen.SetActive(true);
	}
}
