using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MemoryCard : MonoBehaviour
{
    public event Action<MemoryCard> Revealed;

    [SerializeField] private GameObject Back;

    private GameplayController _gameplayController;
    private SpriteRenderer _renderer;

    public CardType Id { get; private set; }

    private void Awake() => _renderer = GetComponent<SpriteRenderer>();

    public void Initialize(CardType id, GameplayController gameplayController)
    {
        Id = id;
        _gameplayController = gameplayController;
    }

    public void SetImage(Sprite image) => _renderer.sprite = image;

    public void OnMouseDown()
    {
        if (!Back.activeSelf)
            return;
        if (!_gameplayController.CanReveal)
            return;

        Back.SetActive(false);
        Reveal(true);
    }

    public void Reveal(bool isRevealed)
    {
        Back.SetActive(!isRevealed);
        if (isRevealed)
            Revealed?.Invoke(this);
    }
}