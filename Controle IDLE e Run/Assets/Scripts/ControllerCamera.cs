using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCamera : MonoBehaviour {

	//camera
	private Transform player;
	private Vector2 velocidade;
	public float smoothTimeX;


	// Use this for initialization
	void Start ()
    {

        player = GameObject.Find("Player").GetComponent<Transform>(); //pegando o Player e pegando o transform
    }

    // Update is called once per frame
    void Update () {
		
	}

	void FixedUpdate()
    {
		//aqui que a camera segue o jogador
		float posX = Mathf.SmoothDamp(transform.position.x, player.position.x, ref velocidade.x, smoothTimeX);
		transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }
}
