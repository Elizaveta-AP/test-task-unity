using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LineRenderer _trajectoy; 
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private GameController _gameController;
    [SerializeField] private AudioSource _coin, _pop;
    private PlayerController _playerController;
    private Vector3[] Points;
    private List<Vector3> Clicks = new List<Vector3>();
    private Vector3 _currentPosition, _currentTouch;
    private Transform _transform;
    private Touch _newTouch;
    private Animator _animator;
    private int _speed = 3, _score;
    
    
    void Start()
    {
        _score = 0;
        _transform = gameObject.transform;
        _animator = GetComponent<Animator>();
        _currentPosition = _transform.position;
        _currentTouch = _transform.position;
        _playerController = GetComponent<PlayerController>();
    }

    
    void Update()
    {
        if (Input.touchCount >0){
            _newTouch = Input.touches[0];
            if ((Input.touches[0].phase == TouchPhase.Began)) {
                _currentTouch = _camera.ScreenToWorldPoint(new Vector3(_newTouch.position.x, _newTouch.position.y, _camera.nearClipPlane));
                }
            if (Input.touches[0].phase == TouchPhase.Ended) GetNewPoint();
        }
    }

    void FixedUpdate(){
        if (Input.touchCount >0){
            if ((Input.touches[0].phase == TouchPhase.Moved)) {
                _currentTouch = _camera.ScreenToWorldPoint(new Vector3(_newTouch.position.x, _newTouch.position.y, _camera.nearClipPlane));
                }
        }
        if (Clicks.Count > 0) Move();
        Points = new Vector3[Clicks.Count + 2];
        Points[0] = _currentPosition;
        Points[Points.Length - 1] = _currentTouch;
        for (int i = 0; i < Clicks.Count; i++){
            Points[i+1] = Clicks[i];
        }
        _trajectoy.positionCount = Points.Length;
        _trajectoy.SetPositions(Points);
    }

    private void GetNewPoint(){
        Vector3 position = _camera.ScreenToWorldPoint(new Vector3(_newTouch.position.x, _newTouch.position.y, _camera.nearClipPlane));
        Clicks.Add(position);
    }

    private void Move(){
        Vector3 nextPoint = Clicks[0];
        _transform.position = Vector3.MoveTowards(_currentPosition, nextPoint, Time.deltaTime * _speed);
        _currentPosition = _transform.position;
        if (_currentPosition == Clicks[0]) Clicks.RemoveAt(0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Spine"){
            _animator.SetTrigger("Pop");
            _pop.Play();
            _trajectoy.positionCount = 0;
            StartCoroutine(Dead());
            _playerController.enabled = false;
        }
        else if (other.gameObject.tag == "Coin"){
            Destroy(other.gameObject);
            _coin.Play();
            _score += 1;
            _scoreText.text = $"{_score}";
            if (_score == 15) {
                _gameController.GameOver("win");
                _playerController.enabled = false;
            }
        }
    }

    IEnumerator Dead(){
        yield return new WaitForSeconds(1);
        _gameController.GameOver("loss");
        Destroy(gameObject);
    }

}
