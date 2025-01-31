using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Button _RestartLevelMenuButton;
    [SerializeField]
    private Button _RestartGameMenuButton;
    [SerializeField]
    private MenuExpandButton _MenuExpandButton;
    [SerializeField]
    private Button _nextButton;
    [SerializeField]
    private Image _imagem;

    private int _indexMensagens = 0;
    private Mensagem[] _mensagens;

    #region Instruções

    private readonly string[] _instrucoes1 =
    {
        "Bem-vindo ao nosso jogo de realidade virtual que ensina como dar banho em um recém-nascido! Você pode ler as regras controlando com os botões abaixo ou começar o jogo agora apertando em <b>Jogar</b>.",
            "1 - <b>Itens Disponíveis</b>:\n Você verá vários itens ao seu redor, todos ao alcance. Estes itens podem ser úteis ou não para dar banho em um recém-nascido.\n" +
            "2 - <b>Bandeja Azul</b>:\n Em sua frente, você encontrará uma bandeja azul. Esta é a área onde você deve colocar os itens que acredita serem necessários para dar banho no bebê.",

        "3 - <b>Escolha dos Itens</b>:\n Use os controladores VR para pegar os itens. Coloque na bandeja azul aqueles que você acha que são apropriados para o banho do recém-nascido.\n" +
            "4 - <b>Cores</b>:\n" +
            "<color=green>Verde</color>: Se você escolheu um item correto, ele será destacado em verde.\n" +
            "<color=red>Vermelho</color>: Se você escolheu um item incorreto, ele será destacado em vermelho.",

        "5 - <b>Finalizar Escolha</b>:\n Quando você achar que já escolheu todos os itens necessários, clique no botão <b>Finalizar</b> localizado nessa caixa de texto.\n" +
            "6 - <b>Revisão</b>:\n Após clicar em <b>Finalizar</b>, você receberá uma pontuação baseada em suas escolhas, com dicas sobre os itens corretos e incorretos.\n" +
            "Divirta-se e aprenda a dar banho em um recém-nascido de maneira segura e eficaz!",

    };

    private readonly string[] _instrucoes2 =
    {
        "Para começar o banho no bebê, é importante que a pessoa responsável retire anéis, pulseiras ou acessórios, para evitar arranhões. Além disso, a pessoa deve estar com unhas curtas" +
            " e com cabelo preso, caso esse seja longo.",

        "A água para banho deve estar na temperatura do corpo, cerca de 37ºC, e é importante que o banho não ultrapasse a faixa de 5 a 10 minutos," +
            " porque os bebês perdem calor com muita facilidade.",

        "Também é importante que o local do banho esteja com janelas e portas fechadas, para evitar correntes de ar no bebê, para que ele não sinta frio. O banho do bebê começa pelo rosto do bebê.",

        "Evite passar sabonete no rosto do bebê, já que pode causar irritação nos seus olhos. Nesse momento do banho, o bebê deve estar enrolado numa toalha para evitar perda de calor." +
            "Aqui no nosso jogo, você limpará o rosto do bebê utilizando um algodão, que se encontra na bandeja.",

        "Você pode pegar o bebê sempre apoiando sua cabeça para evitar solavancos no seu pescoço. O algodão pode ser molhado na banheira à direita e você precisa passá-lo na testa, nas duas " +
            "bochechas do bebê e no seu queixo.",

        "Essas áreas estão manchadas, simulando uma sujeira, basta esfregar o algodão molhado para retirar a sujeira. Caso você tenha êxito, o bebê irá" +
            " responder animadamente às suas ações! Para começar, basta clicar em <b>Jogar</b>.",
    };

    #endregion

    private void Start()
    {
        _button.onClick.AddListener(MostrarInstrucoes);
        _nextButton.onClick.AddListener(Proximo);
        _previousButton.onClick.AddListener(Anterior);
        _RestartLevelMenuButton.onClick.AddListener(ReiniciarJogo);

        Item[] itensEmCena = _itens.GetComponentsInChildren<Item>();
        _bandeja.Iniciar(itensEmCena);
    }

    private void MostrarInstrucoes()
    {
        _mensagens = _instrucoes1.Select(s => new Mensagem() { mensagem = s }).ToArray();
        _buttonText.text = "JOGAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ComecarFase1);
        MostrarMensagens();
    }

    private void MostrarMensagens()
    {
        _indexMensagens = 0;
        SetMensagem(_mensagens[_indexMensagens]);
        _previousButton.gameObject.SetActive(false);
        _nextButton.gameObject.SetActive(_mensagens.Length > 1);
    }

    private void Proximo()
    {
        _indexMensagens++;
        SetMensagem(_mensagens[_indexMensagens]);
        if (_indexMensagens == 1)
            _previousButton.gameObject.SetActive(true);
        if (_indexMensagens == _mensagens.Length - 1)
            _nextButton.gameObject.SetActive(false);
    }

    private void Anterior()
    {
        _indexMensagens--;
        SetMensagem(_mensagens[_indexMensagens]);
        if (_indexMensagens == 0)
            _previousButton.gameObject.SetActive(false);
        if (_indexMensagens == _mensagens.Length - 2)
            _nextButton.gameObject.SetActive(true);
    }

    private void ComecarFase1()
    {
        _nextButton.gameObject.SetActive(false);
        _previousButton.gameObject.SetActive(false);

        _display.text = "Coloque os itens na bandeja!";
        _itens.SetActive(true);

        _buttonText.text = "FINALIZAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(FinalizarFase1);
        _button.gameObject.SetActive(false);
    }

    private void FinalizarFase1()
    {
        Mensagem[] checklist = _bandeja.Checklist();
        _indexMensagens = 0;
        _mensagens = checklist;
        _buttonText.text = "PRÓXIMA ETAPA";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(IniciarFase2);
        MostrarMensagens();
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(0);
    }

    private void MostrarInstrucoes2()
    {
        _mensagens = _instrucoes2.Select(s => new Mensagem() { mensagem = s }).ToArray();
        _buttonText.text = "JOGAR";
        _button.gameObject.SetActive(true);
        _button.onClick.RemoveAllListeners();

        Eventos.InscreverComecarFase2(ComecarFase2);
        Eventos.InscreverSegurarBebe(SegurandoBebeFase2);
        Eventos.InscreverSoltarBebe(ComecarFase2);
        Eventos.InscreverLiberarFinalizarFase2(LigarBotaoFinalizarFase2);
        Eventos.InscreverGerarMensagemFase2(FinalizarFase2);

        _button.onClick.AddListener(Eventos.InvocarComecarFase2);
        MostrarMensagens();
    }

    private void ComecarFase2()
    {
        _nextButton.gameObject.SetActive(false);
        _previousButton.gameObject.SetActive(false);

        _display.text =
            "Comece pegando o bebê! Segure o bebê de barriga para cima, apoiando bem a cabeça e o pescoço. Mantenha-o próximo ao peito," +
            " com a cabeça mais alta, e evite movimentos bruscos. Conforto e segurança são essenciais! ";

        _button.gameObject.SetActive(false);
    }

    private void SegurandoBebeFase2()
    {
        _display.text = "Pegue um algodão do pote de algodão na bandeija à esquerda e umedeça-o na banheira à sua direita." +
            " Depois passe no rosto do bebê até que a mancha de sujeira seja removida.";
    }

    private void LigarBotaoFinalizarFase2()
    {
        _button.gameObject.SetActive(true);
        _buttonText.text = "FINALIZAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(Eventos.InvocarTerminarFase2);
    }

    private void FinalizarFase2(string mensagem)
    {
        _display.text = mensagem;
        _buttonText.text = "PRÓXIMA ETAPA";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(IniciarFase3);
    }

    private void IniciarFase2()
    {
        string sceneName = "Segunda fase";
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName).completed += (operation) =>
            {
                _MenuExpandButton.ToggleMenu();
                PrepararFase2();
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            };
        }
        else
        {
            PrepararFase2();
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }

    private void PrepararFase2()
    {
        Eventos.LimparEventos();

        if (_itens != null)
            Destroy(_itens);

        if (_bandeja != null)
            _bandeja.Destruir();

        _MenuExpandButton.AddButton(_RestartGameMenuButton.gameObject.transform);

        _RestartLevelMenuButton.onClick.RemoveAllListeners();
        _RestartLevelMenuButton.onClick.AddListener(IniciarFase2);
        _imagem.enabled = false;

        MostrarInstrucoes2();
    }

    private void IniciarFase3()
    {
        throw new System.NotImplementedException();
    }

    public void SetMensagem(Mensagem mensagem)
    {
        _display.text = mensagem.mensagem;
        if (mensagem.sprite == null)
            _imagem.enabled = false;
        else
        {
            _imagem.enabled = true;
            _imagem.sprite = mensagem.sprite;
        }
    }

    public void LigarBotao()
    {
        _button.gameObject.SetActive(true);
    }
}

public struct Mensagem
{
    public string mensagem;
    public Sprite sprite;
}
