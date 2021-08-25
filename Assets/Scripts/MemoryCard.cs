using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private GameObject cardBack;
    public void OnMouseDown()
    {
        if (cardBack.activeSelf && controller.canReveal) //deactivate only if card back is visible and can be opened only 2 cards
        {
            cardBack.SetActive(false); //make card back invisible
            controller.CardRevealed(this);  //notify the controller about the opening of this card
        }
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }

    [SerializeField] private SceneController controller;

    private int _id;
    public int id
    {
        get { return _id; }
    }

    public void SetCard(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }    
        void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
