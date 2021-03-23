using UnityEngine;
using UnityEngine.SceneManagement;

public class Build : MonoBehaviour
{
	public static Scene[] Scenes { get; private set; }
	public static int Level0 { get; private set; }
	public static int LevelCount { get; private set; }

	private void Start()
	{
		int numScenes = SceneManager.sceneCountInBuildSettings;
		Scenes = new Scene[numScenes];
		for (int i = 0; i < numScenes; i++)
		{
			Scenes[i] = SceneManager.GetSceneByBuildIndex(i);
			if (Scenes[i].name.StartsWith("Level"))
			{
				if (Level0 == default)
					Level0 = i;
				LevelCount++;
			}
		}
	}
}
