using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum CardType
{
    First = 0,
    Second = 1,
    Third = 2,
    Fourth = 3,
}

public class GameplayController : MonoBehaviour
{
    private const int GridRows = 2; // quantity of rows 
    private const int GridCols = 4; // and columns 
    private const float OffsetX = 2f; // regulate distance
    private const float OffsetY = 2.5f;

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Sprite[] _images;
    [SerializeField] private Text _scoreLabel;

    private readonly List<MemoryCard> _cards = new List<MemoryCard>();
    private readonly List<MemoryCard> _revealedCards = new List<MemoryCard>();
    private readonly CardType[] _possibleCards = Enum.GetValues(typeof(CardType)).Cast<CardType>().ToArray();
    private CardType[] _cardPairs;

    private int _score;

    public bool CanReveal => _revealedCards.Count < 2;

    #region Initialize

    private void Awake()
    {
        var index = 0;
        _cardPairs = new CardType[_possibleCards.Length * 2];
        foreach (var card in _possibleCards)
        {
            _cardPairs[index++] = card;
            _cardPairs[index++] = card;
        }
    }

    private void Start()
    {
        var shuffledCards = ShuffleArray(_cardPairs);
        for (var i = 0; i < GridCols; i++)
        for (var j = 0; j < GridRows; j++)
        {
            var index = j * GridCols + i;
            var cardType = shuffledCards[index];

            var cardInstance = Instantiate(_cardPrefab).GetComponent<MemoryCard>();
            cardInstance.Initialize(cardType, this);
            cardInstance.SetImage(_images[(int)cardType]);
            cardInstance.Revealed += OnCardRevealed;
            _cards.Add(cardInstance);

            var posX = (OffsetX * i) + _startPosition.x;
            var posY = -(OffsetY * j) + _startPosition.y;
            cardInstance.transform.position = new Vector3(posX, posY, _startPosition.z);
        }
    }

    #endregion

    #region Card reveiling

    private void OnCardRevealed(MemoryCard card)
    {
        _revealedCards.Add(card);
        if (_revealedCards.Count < 2)
            return;

        StartCoroutine(CheckMatch());
    }

    private IEnumerator CheckMatch()
    {
        var matchedCardsCount = GetMatchedCardsCount();
        if (matchedCardsCount > 0)
        {
            _score++;
            _scoreLabel.text = "Score: " + _score;
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            foreach (var card in _revealedCards)
                card.Reveal(false);
        }

        _revealedCards.Clear();
    }

    private int GetMatchedCardsCount()
    {
        var count = 0;
        for (var i = 0; i < _revealedCards.Count; i++)
        for (var j = i; j < _revealedCards.Count; j++)
        {
            if (i == j)
                continue;

            if (_revealedCards[i].Id == _revealedCards[j].Id)
                count++;
        }

        return count;
    }

    #endregion

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    private static T[] ShuffleArray<T>(T[] numbers)
    {
        //Knuth shuffle algorithm
        var newArray = (T[])numbers.Clone();
        for (var i = 0; i < newArray.Length; i++)
        {
            var tmp = newArray[i];
            var r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }
}
