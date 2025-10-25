using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int score;

    void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("スコア加算！現在：" + score);
    }
}
