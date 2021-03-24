using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
	public static Level Active { get; private set; }
	public static int ActiveIndex { get; private set; }

	public Player m_Player;
	public GameObject m_CompleteScreen;
	public int m_Level;
	public bool m_Active = true;

	private void Awake()
	{
		if (m_Active)
		{
			Active = this;
			ActiveIndex = m_Level;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
			Retry();
	}

	public void Complete()
	{
		m_Player.Disable();
		m_CompleteScreen.SetActive(true);
	}

	public void Retry()
	{
		m_CompleteScreen.SetActive(false);
		m_Player.Retry();
	}

	public void Next()
	{
		int next = m_Level;
		if (next < Build.LevelCount)
			SceneManager.LoadSceneAsync(next + Build.Level0);
	}
}