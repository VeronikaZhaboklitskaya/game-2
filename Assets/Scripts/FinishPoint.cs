using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("FinishPointScript");
        if (collision.CompareTag("Player"))
        {
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.finishlevel);
            }// go to next level 
            GameManager.instance.NextLevel();
        }
    }
}
