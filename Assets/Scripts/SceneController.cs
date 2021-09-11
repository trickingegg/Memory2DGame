using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 2;  // quantity of rows 
    public const int gridCols = 4;  // and columns 
    public const float offsetX = 2f;    // regulate distance
    public const float offsetY = 2.5f;
    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;
    private int _score = 0;
    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;
    public bool canReveal
    {
        get
        {
            return _secondRevealed == null; //false if second is opened
        }
    }

    public void CardRevealed(MemoryCard card)
    {
        if (_firstRevealed == null) //saving cards in one of two variables
        {
            _firstRevealed = card;
        }
        else
        {
            _secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }
    
    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)
        {
            _score++;
            scoreLabel.text = "Score: " + _score;
        }
        else
        {
            yield return new WaitForSeconds(.5f);

            _firstRevealed.Unreveal();  //closing not matched cards
            _secondRevealed.Unreveal();
        }
        _firstRevealed = null;  //clearing variables
        _secondRevealed = null;
    }

    void Start()
    {
        Vector3 startPos = originalCard.transform.position; //position of 1st card, all next cards are based on this

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 }; //array of identifier pairs for all cards
        numbers = ShuffleArray(numbers); //shuffle the array

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MemoryCard card;    //container link for original card and copies
                if (i== 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }
                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
    }

    private int[] ShuffleArray(int[] numbers)   //Knuth shuffle algorithm
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Memory");
    }
}
