using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAudioManager : MonoBehaviour
{
    public AudioSource menuMusicSource;
    public AudioClip menuMusicClip;

    void Awake()
    {
        menuMusicSource.clip = menuMusicClip;
        menuMusicSource.loop = true;
        menuMusicSource.Play();

        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Gameplay sahnelerinin ismi LEVEL ile baþlýyor veya özel gameplay sahneleri (örneðin LevelEditorScene)
        if (scene.name.StartsWith("LEVEL") || scene.name == "LevelEditorScene" || scene.name == "HellMode")
        {
            menuMusicSource.Stop();
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
