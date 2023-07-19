using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controle_Player : MonoBehaviour {
	/*
	 Aula Unity 134 a 139
	 */

	// variaveis de correndo e parado
	private Animator animacaoPlayer; //variavel para ter acesso ao Animator
	
	private Rigidbody2D fisicaPlayer2D; //variavel para ter acesso ao RigidBody2D para ter a fisica
	
	public Transform checandoChao; //variavel para detectar se o personagem esta pisando no chao ou nao
	
	public bool estaNoChao; //variavel false e true pra ve se esta no chao
	
	public bool olhandoDireita = true; //variavel false e true para ve se esta olhando para direita
	
	public float velocidadeJogador; //variavel de velocidade de personagem

	public float direcao; //variavel para ver se o personagem vai para direita ou esquerda

	//================================================================================================
	//variaveis de Pulo
	public bool pulo = false;
	public float forcaPulo;

	//variaveis para limitar o pulo
	public int numeroPulo = 0;
	public int maximoPulo = 2;

	//================================================================================================
	//variaveis de tiro
	public Transform bala; //variavel para detectar se o personagem esta atirando
	public float velocidadetiro; //variavel de velocidade do Tiro
	public GameObject tiroPrefab; //acessando um objeto do jogo 

	//tempo de disparo de tiro
	public float tempoTiro; //tempo do tiro
	private bool tiroDisparado; //variavel para ver se foi dispardo ou nao

	//================================================================================================
	//padrao Unity

	// Use this for initialization
	void Start () {

		animacaoPlayer = GetComponent<Animator>(); //variavel para ter o controle de todo Animator
		fisicaPlayer2D = GetComponent<Rigidbody2D>(); // variavel para ter o controle de todo Rigidbody2D

	}
	
	// Update is called once per frame
	void Update () {

		//-----------------------------------------------------------------------------------------------------------------
		//correndo e parado
		estaNoChao = Physics2D.Linecast(transform.position, checandoChao.position, 1 << LayerMask.NameToLayer("Ground")); //instruçao para analisar e ve se estou tocando no chao ou nao (True or False)
		//Debug.Log(estaNoChao);

		animacaoPlayer.SetBool("estouNoChao", estaNoChao); //acessando a variavel criada no Animator e setando ela

		direcao = Input.GetAxisRaw("Horizontal"); //pegando a direcao do eixo que estou

		ExecultaMovimentos(); //chamando a funcao de movimento

        //-----------------------------------------------------------------------------------------------------------------
        // pulo

		//verificando a tecla de pulo foi apertada (Space)
        if (Input.GetButtonDown("Jump") ) // adiciona no if caso seja só 1 pulo (&& estaNoChao)
        {
			pulo = true;
        }

        //-----------------------------------------------------------------------------------------------------------------
        // tiro

		//verficando a tecla do mouse esquerda foi apertada
        if (Input.GetMouseButton(0) && tiroDisparado == false)
        {
			if(fisicaPlayer2D.velocity.x != 0 && estaNoChao == true)
            {
				atirar();
            }
        }
	}

	// funçao para ter o melhor frame rate para independente da maquinas
	void FixedUpdate()
    {
		//-----------------------------------------------------------------------------------------------------------------
		//correndo e parado
		MovePlayer(direcao); //chamando a funçao

        //-----------------------------------------------------------------------------------------------------------------
        //pulo

		//verificando se o pulo esta true
        if (pulo)
        {
			puloPlayer();
        }

	}

    //=================================================================================================
    //funcao de correr e parado

    //funcao para fazer os movimentos (pulando, correndo, pisando no chao)
    void ExecultaMovimentos()
    {
		animacaoPlayer.SetFloat("velocidadeY", fisicaPlayer2D.velocity.y); //fazendo a animaçao blend Tree
		animacaoPlayer.SetBool("Pulando", !estaNoChao); //fazendo a animacao de pulo no Animator
		animacaoPlayer.SetBool("correndo", fisicaPlayer2D.velocity.x != 0f && estaNoChao); //fazendo a animacao de correr no Animator
    }

	//movimentaçao do player (esquerda e direita)
	void MovePlayer(float movimentoH)
    {
		fisicaPlayer2D.velocity = new Vector2(movimentoH * velocidadeJogador, fisicaPlayer2D.velocity.y); //setando a velocidade do jogador eixo x

		//codiçao para ver se esta olhando para direita e esquerda para virar o personagem
		if(movimentoH < 0 && olhandoDireita || (movimentoH > 0 && !olhandoDireita))
        {
			Flip();
        }
    }

	//funcao responsavel para vira o personagem para direita e esquerda
	void Flip()
	{
		olhandoDireita = !olhandoDireita;
		Vector3 theScale = transform.localScale;

		theScale.x *= -1;
		velocidadetiro *= -1; //velocidade
		transform.localScale = theScale;
	}

	//=================================================================================================
	//funcao de Pular
	 void puloPlayer()
	{

        if (estaNoChao)
        {
			numeroPulo = 0;
        }

		//maximo de pulo e pulando
		if(estaNoChao || numeroPulo < maximoPulo)
        {
			fisicaPlayer2D.AddForce(new Vector2(0f, forcaPulo)); // aplicando o pulo
			numeroPulo++;
			estaNoChao = false;
		}

		pulo = false;
	}

	//=================================================================================================
	//funcao de Tiro
	//aula Unity 142
	void atirar()
	{
		tiroDisparado = true;
		StartCoroutine("tempoDeTiro");

		GameObject tiroTemporario = Instantiate(tiroPrefab);
		tiroTemporario.transform.position = bala.position;

		if(olhandoDireita == false)
        {
			//virando a bala
			tiroTemporario.GetComponent<SpriteRenderer>().flipX = true;
        }

		tiroTemporario.GetComponent<Rigidbody2D>().velocity = new Vector2(velocidadetiro, 0);
	}

	IEnumerable tempoDeTiro()
    {
		yield return new WaitForSeconds(tempoTiro); //derlay de tiro
		tiroDisparado = false;
    }
}
