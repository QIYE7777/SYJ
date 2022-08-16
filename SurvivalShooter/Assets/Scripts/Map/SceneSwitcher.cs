using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher instance;

    public LevelPrototype currentLevel;
    public RoomPrototype roomPrototype;

    int currentIndex;

    public GameObject playerPrefab;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentIndex = 0;
        SwitchToRoomOfLevel();
    }

    void SwitchToRoomOfLevel()
    {
        if (currentIndex >= currentLevel.rooms.Count)
        {
            Debug.LogError("no enough room! currentIndex is " + currentIndex);
            // Win();
            return;
        }

        var room = currentLevel.rooms[currentIndex];
        roomPrototype = room;
        SceneManager.LoadScene(room.sceneName, LoadSceneMode.Single);
    }

    public void SwitchToNextRoom()
    {
        currentIndex += 1;
        SwitchToRoomOfLevel();
    }

    public void RestartCurrentLevel()
    {
        SwitchToRoomOfLevel();
    }
}