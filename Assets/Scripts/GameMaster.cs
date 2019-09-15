using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Text _restartText;

    private MapMaster _mapMaster;
    private bool _onLoseGame = false;
    private bool _onPause = true;


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
        FindObjectOfType<TilesManager>().onTilePassed += (t) => ScoresMaster.OnIncScore();

        UIEvents.onPausePressed += () =>
        {
            _menuWindow.Show();

            _player.Freeze();

            _onPause = true;
        };

        UIEvents.onPlayPressed += () =>
        {
            _menuWindow.HideImmediate();

            _player.Unfreeze();

            _onPause = false;
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

        _onLoseGame = false;
    }

    private void Start()
    {
        ScoresMaster.LoadBestScore();

        _mapMaster.Initialize();

        _menuWindow.Show();

        _camera.followPlayer = true;

        CreateEventsHandlers();
    }

    private void Update()
    {
        _scoreField.text = ScoresMaster.Score.ToString();
        _bestScoreField.text = ScoresMaster.BestScore.ToString();

        if (_onPause)
            return;

        if (_player.isFalling && !_onLoseGame)
        {
            _camera.followPlayer = false;

            _restartText.gameObject.SetActive(true);

            ScoresMaster.OnEndGame();

            _onLoseGame = true;
        }

        if (_onLoseGame && (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0)))
            StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        _restartText.gameObject.SetActive(false);
        _camera.followPlayer = true;

        _player.Reset();
        _player.Freeze();
        _player.transform.position = Vector3.zero;

        _mapMaster.Restart();

        _onLoseGame = false;

        yield return new WaitForSeconds(0.15f);

        _player.Unfreeze();

        ScoresMaster.OnStartGame();
    }
}
