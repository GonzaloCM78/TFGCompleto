using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI roundText;
    public Image granada1, granada2, granada3, granada4;

    public int health = 100;

    public int round = 1;
    public int zombiesInRound = 10;
    public int maxSimultaneousZombies = 30;

    private int zombiesSpawnedInRound = 0;
    private int zombiesRemaining = 0;
    public int zombiesKilled = 0;

    private float zombieSpawnTimer = 0f;

    public Transform[] zombieSpawnPoints;
    public GameObject zombieEnemy;

    public int playerPoints = 500;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI interactionText;

    public Transform playerTransform;

    public bool isInstaKillActive = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip roundChangeClip;
    public AudioClip instaKillEnd;
    public AudioClip sonidoAmbiente;
    

    [Header("Game Over UI")]
    public GameObject deathPanel;
    public GameObject fadePanel;
    public TextMeshProUGUI muerteTexto;
    public TextMeshProUGUI puntosTexto;
    public TextMeshProUGUI bajasTexto;
    public TextMeshProUGUI rondaTexto;

    [Header("Marcador en juego")]
    public GameObject scorePanel; // Panel que se muestra al pulsar TAB
    public TextMeshProUGUI puntosTextoTab;
    public TextMeshProUGUI bajasTextoTab;
    public TextMeshProUGUI rondaTextoTab;

    void Start()
    {
        // Reproduce la canción si está asignada
        if (audioSource != null && sonidoAmbiente != null)
        {
            audioSource.clip = sonidoAmbiente;
            audioSource.loop = true;
            audioSource.Play();
        }
    }


    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        healthText.text = health.ToString();
        if (roundText != null) roundText.text = round.ToString();

        if (zombiesSpawnedInRound < zombiesInRound && zombiesRemaining < maxSimultaneousZombies)
        {
            zombieSpawnTimer += Time.fixedDeltaTime;

            if (zombieSpawnTimer >= 3f)
            {
                SpawnZombie();
                zombieSpawnTimer = 0f;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && health > 0)
        {
            scorePanel.SetActive(true);

            if (puntosTextoTab != null) puntosTextoTab.text = "PUNTOS: " + playerPoints.ToString("D5");
            if (bajasTextoTab != null) bajasTextoTab.text = "ZOMBIS ELIMINADOS: " + zombiesKilled;
            if (rondaTextoTab != null) rondaTextoTab.text = "RONDA: " + round;
        }

        if (Input.GetKeyUp(KeyCode.Tab) && health > 0)
        {
            scorePanel.SetActive(false);
        }
    }



    void SpawnZombie()
    {
        List<Transform> activeSpawnPoints = new List<Transform>();

        foreach (var sp in zombieSpawnPoints)
        {
            if (sp.gameObject.activeInHierarchy)
            {
                activeSpawnPoints.Add(sp);
            }
        }

        if (activeSpawnPoints.Count == 0)
            return;

        int spawnIndex = Random.Range(0, activeSpawnPoints.Count);
        Transform spawnPoint = activeSpawnPoints[spawnIndex];

        GameObject zombie = Instantiate(zombieEnemy, spawnPoint.position, Quaternion.identity);

        int baseLife = 5;
        int extraLifePerRound = 2;
        int totalLife = baseLife + ((round - 1) * extraLifePerRound);
        AI ai = zombie.GetComponent<AI>();
        if (ai != null)
        {
            ai.lifes = totalLife;
            ai.target = playerTransform;
        }

        zombiesSpawnedInRound++;
        zombiesRemaining++;
    }

    public void OnZombieKilled()
    {
        zombiesRemaining--;
        zombiesKilled++;

        if (zombiesSpawnedInRound >= zombiesInRound && zombiesRemaining <= 0)
        {
            if (audioSource != null && roundChangeClip != null)
            {
                audioSource.PlayOneShot(roundChangeClip);
            }

            StartCoroutine(StartNextRound());
        }
    }

    IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(12f);

        round++;
        zombiesInRound += 5;
        maxSimultaneousZombies = Mathf.Min(maxSimultaneousZombies + 2, 14);

        zombiesSpawnedInRound = 0;
        zombiesRemaining = 0;

        Debug.Log("Ronda " + round + " iniciada.");
    }

    public void LoseHealth(int amount)
    {
        health -= amount;
        if (healthText != null)
            healthText.text = health.ToString();

        if (health <= 0)
        {
            Debug.Log("Has muerto");
            ShowGameOverScreen();
        }
    }

    public void RegenHealth(int amount)
    {
        health += amount;
    }

    public void AddPoints(int amount)
    {
        playerPoints += amount;
        UpdatePointsUI();
    }

    public bool SpendPoints(int amount)
    {
        if (playerPoints >= amount)
        {
            playerPoints -= amount;
            UpdatePointsUI();
            return true;
        }
        return false;
    }

    private void UpdatePointsUI()
    {
        if (pointsText != null)
            pointsText.text = playerPoints.ToString();
    }

    public void ShowInteractionMessage(string message)
    {
        if (interactionText != null)
            interactionText.text = message;
    }

    public void ClearInteractionMessage()
    {
        if (interactionText != null)
            interactionText.text = "";
    }

    public void ShowTemporaryMessage(string message, float duration)
    {
        StartCoroutine(TemporaryMessageRoutine(message, duration));
    }

    IEnumerator TemporaryMessageRoutine(string msg, float delay)
    {
        ShowInteractionMessage(msg);
        yield return new WaitForSeconds(delay);
        ClearInteractionMessage();
    }

    public void ActivateInstaKill(float duration)
    {
        StartCoroutine(InstaKillRoutine(duration));
    }

    IEnumerator InstaKillRoutine(float time)
    {
        isInstaKillActive = true;
        ShowTemporaryMessage("¡Baja instantánea!", 2f);
        yield return new WaitForSeconds(time);
        audioSource.PlayOneShot(instaKillEnd);
        isInstaKillActive = false;
    }

    void ShowGameOverScreen()
    {
        // Enviar sesión al backend
        string username = PlayerPrefs.GetString("username", "");
        StartCoroutine(EnviarSesion.Enviar(username, round, playerPoints, "Shipment Z", "0:00"));

        Time.timeScale = 0f;

        if (deathPanel != null) deathPanel.SetActive(true);
        if (muerteTexto != null) muerteTexto.text = "HAS MUERTO";
        if (puntosTexto != null) puntosTexto.text = "PUNTOS: " + playerPoints.ToString("D5");
        if (bajasTexto != null) bajasTexto.text = "ZOMBIS ELIMINADOS: " + zombiesKilled;
        if (rondaTexto != null) rondaTexto.text = "RONDA: " + round;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }





    public void VolverAlMenu()
    {
        StartCoroutine(FadeAndLoad("Menu"));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        if (fadePanel != null)
            fadePanel.SetActive(true);

        yield return new WaitForSecondsRealtime(2f); //  Usamos WaitForSecondsRealtime

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(sceneName);
    }


    public void Reiniciar()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }




}
