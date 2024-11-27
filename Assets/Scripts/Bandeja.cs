using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bandeja : MonoBehaviour
{
    private int _totalItensCorretos;
    private int _totalItensErrados;
    private float _pontosPorItemCorreto;
    private float _penalidadePorItemErrado;
    private Action _onGameEnd;

    private readonly List<Item> _itens = new();

    [SerializeField]
    private UIController _display;

    public void Iniciar(Item[] itens)
    {
        _totalItensCorretos = itens.Count(i => i.Correto);
        _pontosPorItemCorreto = 100f / _totalItensCorretos;

        _totalItensErrados = itens.Count(i => !i.Correto);
        _penalidadePorItemErrado = 60f / _totalItensErrados;

        _onGameEnd = () =>
        {
            foreach (var item in itens)
            {
                item.DesativarInteratividade();
            }
        };
    }

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

    public string[] Checklist()
    {
        var itensCorretos = _itens.Count(i => i.Correto);
        var itensErrados = _itens.Count(i => !i.Correto);

        float pontuacao = itensCorretos * _pontosPorItemCorreto - itensErrados * _penalidadePorItemErrado;
        if (pontuacao < 0)
            pontuacao = 0;
        else
            pontuacao = Mathf.RoundToInt(pontuacao);

        var result = new List<string>
        {
            $"<color=green>Itens corretos</color>: {itensCorretos}/{_totalItensCorretos}\n" +
            $"<color=red>Itens errados</color>: {itensErrados}/{_totalItensErrados}\n" +
            $"Você escolheu {itensCorretos * 100 / _totalItensCorretos}% dos itens corretos e {itensErrados * 100 / _totalItensErrados}% dos itens errados.\n" +
            $"Sua pontuação foi de {pontuacao} pontos. Reveja informações sobre os itens usando o menu abaixo."
        };

        foreach (Item item in _itens)
        {
            result.Add(item.GetInformacao());
        }

        _onGameEnd.Invoke();

        return result.ToArray();
    }
}
