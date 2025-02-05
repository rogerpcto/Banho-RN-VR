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

    #region Instru��es

    private readonly string[] _instrucoes1 =
    {
        "Bem-vindo ao nosso jogo de realidade virtual que ensina como dar banho em um rec�m-nascido! Voc� pode ler as regras controlando com os bot�es abaixo ou come�ar o jogo agora apertando em <b>Jogar</b>.",
            "1 - <b>Itens Dispon�veis</b>:\n Voc� ver� v�rios itens ao seu redor, todos ao alcance. Estes itens podem ser �teis ou n�o para dar banho em um rec�m-nascido.\n" +
            "2 - <b>Bandeja Azul</b>:\n Em sua frente, voc� encontrar� uma bandeja azul. Esta � a �rea onde voc� deve colocar os itens que acredita serem necess�rios para dar banho no beb�.",

        "3 - <b>Escolha dos Itens</b>:\n Use os controladores VR para pegar os itens. Coloque na bandeja azul aqueles que voc� acha que s�o apropriados para o banho do rec�m-nascido.\n" +
            "4 - <b>Cores</b>:\n" +
            "<color=green>Verde</color>: Se voc� escolheu um item correto, ele ser� destacado em verde.\n" +
            "<color=red>Vermelho</color>: Se voc� escolheu um item incorreto, ele ser� destacado em vermelho.",

        "5 - <b>Finalizar Escolha</b>:\n Quando voc� achar que j� escolheu todos os itens necess�rios, clique no bot�o <b>Finalizar</b> localizado nessa caixa de texto.\n" +
            "6 - <b>Revis�o</b>:\n Ap�s clicar em <b>Finalizar</b>, voc� receber� uma pontua��o baseada em suas escolhas, com dicas sobre os itens corretos e incorretos.",

        "A qualquer momento, voc� pode reiniciar a etapa atual no menu no canto superior direito nessa tela. Divirta-se e aprenda a dar banho em um rec�m-nascido de maneira segura e eficaz!"

    };

    private readonly string[] _instrucoes2 =
    {
        "Para come�ar o banho no beb�, � importante que a pessoa respons�vel retire an�is, pulseiras ou acess�rios, para evitar arranh�es. Al�m disso, a pessoa deve estar com unhas curtas" +
            " e com cabelo preso, caso esse seja longo.",

        "A �gua para banho deve estar na temperatura do corpo, cerca de 37�C, e � importante que o banho n�o ultrapasse a faixa de 5 a 10 minutos," +
            " porque os beb�s perdem calor com muita facilidade.",

        "Tamb�m � importante que o local do banho esteja com janelas e portas fechadas, para evitar correntes de ar no beb�, para que ele n�o sinta frio. O banho do beb� come�a pelo rosto do beb�.",

        "Nesse momento do banho, o beb� deve estar enrolado numa toalha para evitar perda de calor." +
            "Aqui no nosso jogo, voc� limpar� o rosto do beb� utilizando um algod�o, que se encontra na bandeja.",

        "Voc� pode pegar o beb� sempre apoiando sua cabe�a para evitar solavancos no seu pesco�o. O algod�o pode ser molhado na banheira � direita e voc� precisa pass�-lo na testa, nas duas " +
            "bochechas do beb� e no seu queixo.",

        "Essas �reas est�o manchadas, simulando uma sujeira, basta esfregar o algod�o molhado para retirar a sujeira. Caso voc� tenha �xito, o beb� ir�" +
            " responder animadamente �s suas a��es! Para come�ar, basta clicar em <b>Jogar</b>. A qualquer momento, voc� pode reiniciar a etapa no canto inferior direito" +
            " ou voltar para a etapa inicial clicando em 1.",
    };

    private readonly string[] _instrucoes3 =
    {
        "Agora, vamos lavar a cabe�a do beb�. Nesse momento, usaremos sabonete l�quido nos seus cabelos. Para enxaguar, utilizamos �gua e nossas m�os.",

        "� importante n�o passar sabonete l�quido nos rosto do beb�, para evitar que ele irrite os olhos do beb� ou entre em sua boca." +
            " Lembre-se de tampar os ouvidos do beb� para evitar a entrada de �guas nos seus ouvidos.",

        "Clique em <b>Jogar</b> para come�ar e a qualquer momento, voc� pode reiniciar esta etapa no canto inferior direito" +
            " ou voltar para a etapa inicial clicando em 1.",
    };

    private readonly string[] _instrucoesFinais =
    {
        "Parab�ns! Voc� chegou ao fim do nosso jogo! Mas o banho do beb� n�o acaba por aqui! Ao terminar de lavar a cabe�a, � importante secar bem o seu rostinho para que ele n�o sinta frio. A partir" +
            " da�, podemos retirar sua toalha, sua fralda e realizar um banho de submers�o na bacia.",

        "� importante colocar o beb� bem lentamente na banheira, para ele ir acostumando com a temperatura da �gua. Usando bem pouco sab�o, voc� limpa a parte da frente do beb�, seguido pela parte de tr�s." +
            " Virando-o sempre com muito cuidado, para que ele n�o escorregue na banheira.",

        "Uma boa dica � segur�-lo sempre para axila, promovendo sua seguran�a. Nunca deixe seu beb� sozinho na banheira, por nenhum momento! Seque bem seu beb� ao fim do banho e passe uma gaze com �lcool 70%" +
            " (ou clorexidina) no coto umbilical, realizando sua limpeza. Depois coloque a fralda e as roupas do seu beb�!",

        "Caso o coto n�o tenha ca�do ainda, mantenha para fora da fralda, mantendo-o limpo e seco. Se surgirem sinais de infec��o no coto umbilical como incha�o, vermelhid�o e secre��es, procure o atendimento" +
            " no Servi�o de Sa�de. Todo esse cuidado � importante para criar um la�o com seu beb�, aproveite bastante esse momento, cantarolando e conversando com ele. Obrigado por jogar e at� a pr�xima!"
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

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(0);
    }

    private void InstrucaoSegurarBebe()
    {
        _nextButton.gameObject.SetActive(false);
        _previousButton.gameObject.SetActive(false);

        _display.text =
            "Comece pegando o beb�! Segure o beb� de barriga para cima, apoiando bem a cabe�a e o pesco�o. Mantenha-o pr�ximo ao peito," +
            " com a cabe�a mais alta, e evite movimentos bruscos. Conforto e seguran�a s�o essenciais!";
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

    #region Fase 1

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
        _buttonText.text = "PR�XIMA ETAPA";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(IniciarFase2);
        MostrarMensagens();
    }

    #endregion

    #region Fase 2

    private void MostrarInstrucoes2()
    {
        _mensagens = _instrucoes2.Select(s => new Mensagem() { mensagem = s }).ToArray();
        _buttonText.text = "JOGAR";
        _button.gameObject.SetActive(true);
        _button.onClick.RemoveAllListeners();

        Eventos.InscreverComecarFase2(InstrucaoSegurarBebe);
        Eventos.InscreverSegurarBebe(SegurandoBebeFase2);
        Eventos.InscreverSoltarBebe(InstrucaoSegurarBebe);
        Eventos.InscreverLiberarFinalizarFase2(LigarBotaoFinalizarFase2);
        Eventos.InscreverGerarMensagemFase2(FinalizarFase2);

        _button.onClick.AddListener(() =>
        {
            Eventos.InvocarComecarFase2();
            _button.gameObject.SetActive(false);
        });
        MostrarMensagens();
    }

    private void SegurandoBebeFase2()
    {
        _display.text = "Pegue um algod�o do pote de algod�o na bandeija � esquerda e umede�a-o na banheira � sua direita." +
            " Depois passe no rosto do beb� at� que a mancha de sujeira seja removida.";
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
        _buttonText.text = "PR�XIMA ETAPA";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            string sceneName = "Segunda fase";
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName).completed += (operation) =>
                {
                    IniciarFase3();
                };
            }
        });
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
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (operation) =>
                {
                    PrepararFase2();
                };
            };
        }
        else
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (operation) =>
            {
                PrepararFase2();
            };
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

    #endregion

    #region Fase 3

    private void IniciarFase3()
    {
        string sceneName = "Terceira fase";
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName).completed += (operation) =>
            {
                _MenuExpandButton.ToggleMenu();
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (operation) =>
                {
                    PrepararFase3();
                };
            };
        }
        else
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (operation) =>
            {
                PrepararFase3();
            };
        }
    }

    private void PrepararFase3()
    {
        Eventos.LimparEventos();

        _RestartLevelMenuButton.onClick.RemoveAllListeners();
        _RestartLevelMenuButton.onClick.AddListener(IniciarFase3);

        MostrarInstrucoes3();
    }

    private void MostrarInstrucoes3()
    {
        _mensagens = _instrucoes3.Select(s => new Mensagem() { mensagem = s }).ToArray();
        _buttonText.text = "JOGAR";
        _button.gameObject.SetActive(true);
        _button.onClick.RemoveAllListeners();

        Eventos.InscreverComecarFase3(InstrucaoSegurarBebe);
        Eventos.InscreverSegurarBebe(SegurandoBebeFase3);
        Eventos.InscreverSoltarBebe(InstrucaoSegurarBebe);
        Eventos.InscreverComecarEnxague(ComecarEnxague);
        Eventos.InscreverLiberarFinalizarFase3(LigarBotaoFinalizarFase3);
        Eventos.InscreverGerarMensagemFase3(FinalizarFase3);

        _button.onClick.AddListener(() =>
        {
            Eventos.InvocarComecarFase3();
            _button.gameObject.SetActive(false);
        });
        MostrarMensagens();
    }

    private void SegurandoBebeFase3()
    {
        _display.text = "O sabonete l�quido se encontra � esquerda, onde voc� pode pegar espuma para passar na cabe�a do beb�." +
            " V� passando at� que o beb� te d� um sorrisinho!";
    }

    private void ComecarEnxague()
    {
        Eventos.LimparEventosSegurarBebe();
        Eventos.InscreverSoltarBebe(ComecarEnxague);
        _display.text = "Agora chegou a hora de enxaguar a cabe�a do beb�. Voc� pode pegar a �gua na bacia � direita e soltar para despej�-la" +
            " sobre a cabe�a do beb�, removendo assim a espuma. N�o demore muito, para o sab�o n�o escorrer nos olhos do beb�!";
    }

    private void LigarBotaoFinalizarFase3()
    {
        _button.gameObject.SetActive(true);
        _buttonText.text = "FINALIZAR";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(Eventos.InvocarTerminarFase3);
    }

    private void FinalizarFase3(string mensagem)
    {
        _display.text = mensagem;
        _buttonText.text = "PR�XIMA ETAPA";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(MostrarInstrucoesFinais);
    }

    private void MostrarInstrucoesFinais()
    {
        Eventos.LimparEventos();

        _buttonText.text = "JOGAR NOVAMENTE";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ReiniciarJogo);

        _mensagens = _instrucoesFinais.Select(s => new Mensagem() { mensagem = s }).ToArray();
        _button.gameObject.SetActive(false);
        MostrarMensagens();
    }

    #endregion
}

public struct Mensagem
{
    public string mensagem;
    public Sprite sprite;
}
