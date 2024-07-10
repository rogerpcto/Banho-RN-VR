using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bandeja : MonoBehaviour
{
    private readonly List<Item> _itens = new();

    [SerializeField]
    private UIController _display;

    public void CadastrarItem(Item item)
    {
        _itens.Add(item);
        _display.SetText(item.GetInformacao());
    }

    public void RetirarItem(Item item)
    {
        _itens.Remove(item);
        if (_itens.Count > 0)
            _display.SetText(_itens[^1].GetInformacao());
    }
}
