using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Bandeja _bandeja;
    [SerializeField]
    private GameObject _itens;
    
    [SerializeField]
    private TextMeshProUGUI _display;
    [SerializeField]
    private TextMeshProUGUI _buttonText;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Button _previousButton;
    [SerializeField]
    private Button _nextButton;

    private int _indexInstrucao = 0;

    private string[] _instrucoes =
    {
        "Bem-vindo ao nosso jogo de realidade virtual que ensina como dar banho em um rec�m-nascido! Voc� pode ler as regras controlando com os bot�es abaixo ou come�ar o jogo agora apertando em \"Jogar\".",
        "1 - <b>Itens Dispon�veis</b>:\n Voc� ver� v�rios itens ao seu redor, todos ao alcance. Estes itens podem ser �teis ou n�o para dar banho em um rec�m-nascido.\n" +
        "2 - <b>Bandeja Azul</b>:\n Em sua frente, voc� encontrar� uma bandeja azul. Esta � a �rea onde voc� deve colocar os itens que acredita serem necess�rios para dar banho no beb�.",

        "3 - <b>Escolha dos Itens</b>:\n Use os controladores VR para pegar os itens. Coloque na bandeja azul aqueles que voc� acha que s�o apropriados para o banho do rec�m-nascido.\n" +
        "4 - <b>Cores</b>:\n" +
            "<color=green>Verde</color>: Se voc� escolheu um item correto, ele ser� destacado em verde.\n" +
            "<color=red>Vermelho</color>: Se voc� escolheu um item incorreto, ele ser� destacado em vermelho.",

        "5 - <b>Finalizar Escolha</b>:\n Quando voc� achar que j� escolheu todos os itens necess�rios, clique no bot�o \"Finalizar\" localizado nessa caixa de texto.\n" +
        "6 - <b>Revis�o</b>:\n Ap�s clicar em \"Finalizar\", voc� receber� uma pontua��o sobre suas escolhas, com dicas sobre os itens corretos e incorretos.\n" +
        "Divirta-se e aprenda a dar banho em um rec�m-nascido de maneira segura e eficaz!",

    };

    private void Start()
    {
        _button.onClick.AddListener(MostrarInstrucoes);
        _nextButton.onClick.AddListener(Proximo);
        _previousButton.onClick.AddListener(Anterior);
    }

    public void SetText(string text)
    {
        _display.text = text;
    }

    private void MostrarInstrucoes()
    {
        _display.text = _instrucoes[0];
        _nextButton.gameObject.SetActive(true);
        _buttonText.text = "JOGAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ComecarJogo);
    }

    private void Proximo()
    {
        _indexInstrucao++;
        _display.text = _instrucoes[_indexInstrucao];
        if (_indexInstrucao == 1)
            _previousButton.gameObject.SetActive(true);
        if (_indexInstrucao == _instrucoes.Length - 1)
            _nextButton.gameObject.SetActive(false);
    }

    private void Anterior()
    {
        _indexInstrucao--;
        _display.text = _instrucoes[_indexInstrucao];
        if (_indexInstrucao == 0)
            _previousButton.gameObject.SetActive(false);
        if (_indexInstrucao == _instrucoes.Length - 2)
            _nextButton.gameObject.SetActive(true);
    }

    private void ComecarJogo()
    {
        _nextButton.gameObject.SetActive(false);
        _previousButton.gameObject.SetActive(false);

        _display.text = "Coloque os itens na bandeja!";
        _itens.gameObject.SetActive(true);

        _buttonText.text = "FINALIZAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(FinalizarJogo);
    }

    private void FinalizarJogo()
    {
        Debug.Log("Fim de Jogo");
    }
}
