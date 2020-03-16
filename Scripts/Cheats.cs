using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public partial class Cheats : MonoBehaviour {
	
	public static Cheats Instance;

	public bool lifeCheat; //Setar como life = 100 se Cheats.Instance.lifeCheat == true no script do PlayerInfo no fim do update
	public bool furyCheat; //Setar como fury = 100 se Cheats.Instance.furyCheat == true no script do PlayerInfo no fim do update

	public string levelName; // Criar um input para cada level... deixarei um exemplo, basta multiplicar a variavel e o trecho do código que ocorre o input e replicar o efeito na outra cena

	public bool noClipCheat; // Colocar o if do summar abaixo no script do Controller e criar o input desse cheat no input master, com o mesmo nome do que está no awake ( ultima função de input chamada )

	/* summary ==== cola no update do player, ultima linha possivel
	'
	{
		GetComponent<RigidBody>().useGravity = false;
		GetComponent<CapsuleCollider>().enable = false;
	}

	em todos os ifs de movimentação do FixedUpdate, onde tem if(grounded) adicionar um ||Cheats.Instance.noClipCheat para que ele execute os movimentos, mesmo se não tiver chão em baixo

	no update onde tem a chamada da função GroundCheck ou algo do genero, colocar um if para chama-lo, sendo ele == if(!Cheats.Instance.noClipCheat)


	colocar um if(Cheats.Instance != null) antes de tudo, para que não ocorra erros depois... Garantir que o código execute se Cheats.Instance for nulo

	*/

	bool block;


	InputMaster inputMaster;

	void Awake()
	{
        Instance = this;
		inputMaster = new InputMaster();

		inputMaster.CheatControll.FullLifeInput.performed += act => // Criar o input FullLifeInput no InputMaster e configurar o botão por lá, quando esse botão for apertado, chamara essa action
		{
			if(block) return;
			block = true;
			if(!lifeCheat)
			{
				lifeCheat = true;
			}
		};

		inputMaster.CheatControll.FullFuryInput.performed += act => // Criar o input FullFuryInput no InputMaster e configurar o botão por lá, quando esse botão for apertado, chamara essa action
		{
			if(block) return;
			block = true;
			if(!furyCheat)
			{
				furyCheat = true;
			}
		};

		//Criar série de inputs para diferentes cenas... Aqui estou usando o Exemplo de input SceneOneInput, basta criar um com o mesmo nome no InputMaster ( para cada um input, uma nova função, fazer isso sempre
		// que for necessário fazer um novo cheat para outra cena)
		inputMaster.CheatControll.SceneOneInput.performed += act => // Replicar esse exemplo de input para cada level que tiver uma variavel levelName para ela... fazer um input para cada load cheat
		{
			if(block) return;
			block = true;
			
			SceneManager.LoadScene("Cemiterio v2");

		};
        inputMaster.CheatControll.SceneTwoInput.performed += act => // Replicar esse exemplo de input para cada level que tiver uma variavel levelName para ela... fazer um input para cada load cheat
        {
            if (block) return;
            block = true;

            SceneManager.LoadScene("Cidade v2");

        };

        inputMaster.CheatControll.SceneThreeInput.performed += act => // Replicar esse exemplo de input para cada level que tiver uma variavel levelName para ela... fazer um input para cada load cheat
        {
            if (block) return;
            block = true;

            SceneManager.LoadScene("BBv2");

        };


        //Não esquecer, NoClipInput é o nome do input que tem que estar dentro do mapa de controles CheatControll do InputMaster para que funcione essa chamada ( Seguir exemplos de qualquer outro mapa,
        // implementações de exemplo estão presentes em grande parte dos scripts do player)
        inputMaster.CheatControll.NoClipInput.performed += act =>
        {
            if (block) return;
            block = true;

            noClipCheat = !noClipCheat;

        };

        inputMaster.CheatControll.AllLores.performed += act =>
        {
            LoreControll.Instance.unlockedLores.Clear();
            foreach (Lore l in LoreControll.Instance.allLores)
            {
                LoreControll.Instance.unlockedLores.Add(l);
            }
        };

    }

	void OnEnable()
	{
		inputMaster.Enable();
	}

	void OnDisable()
	{
		inputMaster.Disable();
	}

	void FixedUpdate()
	{
		if(block) block = false;
	}

}