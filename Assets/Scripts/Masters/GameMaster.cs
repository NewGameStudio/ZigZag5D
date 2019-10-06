using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MapCore;
using UICore;

[RequireComponent(typeof(MapMaster))]
public class GameMaster : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private CameraController _camera;

    [Space]

    [SerializeField] private UIWindow _menuWindow;
    [SerializeField] private Text _scoreField;
    [SerializeField] private Text _bestScoreField;
    [SerializeField] private Text _bestWorldScore;
    [SerializeField] private Text _restartText;

    private MapMaster _mapMaster;
    private bool _isGameLose = false;
    private bool _isPause = true;

    public event Action onLoseGame;


    private bool CheckComponents()
    {
        if (_player == null)
        {
            Debug.Log("GameMaster::player is null");
            return false;
        }

        if (_camera == null)
        {
            Debug.Log("GameMaster::CameraController is null");
            return false;
        }

        if (_menuWindow == null)
        {
            Debug.Log("GameMaster::MenuWindow is null");
            return false;
        }

        return true;
    }

    private void CreateEventsHandlers()
    {
        FindObjectOfType<TilesManager>().onTilePassed += (t) => ScoresMaster.IncScore();

        UIEvents.onPausePressed += () =>
        {
            _menuWindow.Show();

            _player.Freeze();

            _isPause = true;
        };

        UIEvents.onPlayPressed += () =>
        {
            _menuWindow.HideImmediate();

            _player.Unfreeze();

            _isPause = false;
        };

        UIEvents.onQuitPressed += () =>
        {
            ScoresMaster.OnEndGame();
            Application.Quit();
        };
    }

    private void Awake()
    {
        Application.targetFrameRate = 120;

        _player = _player ?? FindObjectOfType<PlayerController>();
        _camera = _camera ?? FindObjectOfType<CameraController>();

        if (!CheckComponents())
        {
            Destroy(this);
            return;
        }

        _mapMaster = GetComponent<MapMaster>();

        _player.Reset();
        _player.Freeze();

        _isGameLose = false;
    }

    private void Start()
    {
        LeaderboardMaster.Initialize();

        ScoresMaster.LoadBestScore();

        _mapMaster.Initialize();

        _camera.followPlayer = true;

        CreateEventsHandlers();
    }

    private void Update()
    {
        _scoreField.text = ScoresMaster.Score.ToString();
        _bestScoreField.text = ScoresMaster.BestScore.ToString();

        if (LeaderboardMaster.BestWorldScore < 0 && _bestWorldScore.transform.parent.gameObject.activeSelf)
            _bestWorldScore.transform.parent.gameObject.SetActive(false);

        else if (LeaderboardMaster.BestWorldScore > 0)
        {
            if (!_bestWorldScore.transform.parent.gameObject.activeSelf)
                _bestWorldScore.transform.parent.gameObject.SetActive(true);

            _bestWorldScore.text = LeaderboardMaster.BestWorldScore.ToString();
        }


        if (_isPause)
            return;

        if (_isGameLose)
        {
            if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0))
            {
                StartCoroutine(RestartGame());

                _isGameLose = false;
            }
        }
        else if (_player.isFalling)
        {
            OnGameLose();

            _isGameLose = true;
        }
    }

    private void OnGameLose()
    {
        _camera.followPlayer = false;

        _restartText.gameObject.SetActive(true);

        ScoresMaster.OnEndGame();

        onLoseGame?.Invoke();
    }

    private IEnumerator RestartGame()
    {
        _restartText.gameObject.SetActive(false);
        _camera.followPlayer = true;

        _player.Reset();
        _player.Freeze();
        _player.transform.position = Vector3.zero;

        _mapMaster.Restart();

        yield return new WaitForSeconds(0.1f);

        _player.Unfreeze();

        ScoresMaster.OnStartGame();
    }
}
