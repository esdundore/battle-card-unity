using UnityEngine;
using System.Collections;

public class LibraryLoad : MonoBehaviour
{
    public static LibraryLoad Instance;

    public static PlayersRequest playersRequest = new PlayersRequest("Player", "AI");
    public static string HOST_NAME = "http://localhost:8080";
    //public static string HOST_NAME = "http://battlecard-env.eba-syzyw3ur.us-east-2.elasticbeanstalk.com";
    public static string SKILL_CARDS = "/cards-library";

    // GLOBAL SETTINGS
    [Header("Prefabs")]
    public GameObject SkillCardPrefab;
    public GameObject MonsterCardPrefab;
    [Header("Game Visuals")]
    public CardAreaVisual cardLibrary;

    // Singleton
    private void Awake()
    {
        Instance = this;
    }

    // On object creation
    void Start()
    {
        StartCoroutine(LoadLibraryRoutine(SKILL_CARDS));
    }

    // Game library
    public IEnumerator LoadLibraryRoutine(string url)
    {
        // Start the match
        CoroutineWithData request = new CoroutineWithData(this, RESTTemplate.GETRequestRoutine(HOST_NAME + url));
        yield return request.coroutine;
        CardLibrary cards = JsonUtility.FromJson<CardLibrary>((string)request.result);
        for (int i = 0; i < cardLibrary.slots.Children.Length; i++)
        {
            cardLibrary.CreateSkillCard(i, cards.skillCards[i], cardLibrary.slots.Children[i].transform.position);
        }
        
    }

}