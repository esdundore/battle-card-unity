using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // player IDs
    public static string PLAYER = "Player";

    public static string AI = "AI";
    public static string TUTORIAL = "Tutorial";

    // game start modes
    public static string START_URL = "/start-match";
    public static string RANDOM_START_URL = "/start-random-match";
    public static string TUTORIAL_START_URL = "/start-tutorial-match";

    // Testing only
    public static string TEST_START_URL = "/start-test-match?card1=" + PLAYER_CARDS + "&monster1=" + PLAYER_MONSTER + "&card2=" + OPPONENT_CARDS + "&monster2=" + OPPONENT_MONSTER;
    public static string PLAYER_CARDS = "Dino_Bite";
    public static string PLAYER_MONSTER = "Mocchi";
    public static string OPPONENT_CARDS = "Dino_Bite";
    public static string OPPONENT_MONSTER = "Mocchi";



    public void PlayTutorial()
    {
        SceneParameters.SceneStartUrl = TUTORIAL_START_URL;
        SceneParameters.PlayersRequest = new PlayersRequest(PLAYER, TUTORIAL);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGame()
    {
        SceneParameters.SceneStartUrl = START_URL;
        SceneParameters.PlayersRequest = new PlayersRequest(PLAYER, AI);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayRandomGame()
    {
        SceneParameters.SceneStartUrl = RANDOM_START_URL;
        SceneParameters.PlayersRequest = new PlayersRequest(PLAYER, AI);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayTestGame()
    {
        SceneParameters.SceneStartUrl = TEST_START_URL;
        SceneParameters.PlayersRequest = new PlayersRequest(PLAYER, AI);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGameQueue()
    {
    }

    public void LoadLibrary()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

}
