using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;
using System;

[RequireComponent(typeof(AudioSource))]
public class pacman : MonoBehaviour
{
    private Rigidbody rbody;
    private float fuerza =200f;
    public GameObject miCamara;
    public GameObject  pacmanobjt;
    private Vector3 setCamara;
    private Vector3 setpacman;
    public AudioClip impact;
    public AudioClip impact2;
    public AudioSource audios;
    public AudioSource audio2;
    public Material colorinicial;
    public bool pacmanenojado = true;
    float timeLeft = 10.0f;
    float timeLeft2 = 10.0f;
    float timeLeftjuego = 60.0f;
    public Text txtpuntaje;
    public Text txtfrutas;
    public Text txtganador;
    public Text txttiempo;
    public Text txtvidas;
    private int score;
    private int frutas;
    public Button btnRestart;
    public Button btnmenu;
    private bool estadopacman= true;

    public MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
    string alias = "";
    string contraseña = "";
    string id = "";
    int puntaje;
    int vidas ;
    


    // Use this for initialization

    void Start()
    {
        
        gameObject.GetComponent<Renderer>().material = colorinicial;
        rbody = GetComponent<Rigidbody>();
        setCamara = miCamara.transform.position;
        
        score = 0;
        frutas = 0;
        txtpuntaje.text = "Puntaje: ";
        txtfrutas.text = "Frutas: ";
        txtganador.text = "";
        audios =gameObject.AddComponent<AudioSource>();
        audios.clip = Resources.Load(name) as AudioClip;
        audio2 = gameObject.AddComponent<AudioSource>();
        audio2.clip = Resources.Load(name) as AudioClip;
        btnRestart.gameObject.SetActive(false);
        btnmenu.gameObject.SetActive(false);
        builder.Server = "localhost";
        builder.UserID = "root";
        builder.Password = "";
        builder.Database = "pacman";
        sesionactiva();
        
        


    }

    // Update is called once per frame
    void Update()
    {
        timeLeftjuego -= Time.deltaTime;
        if (timeLeftjuego < 0)
        {
            txtganador.text = "Puntaje = " + score*(frutas+1);
            btnRestart.gameObject.SetActive(true);
            btnmenu.gameObject.SetActive(true);
        }
        else
        { 
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                pacmanenojado = false;
                colorinicial.color = new Color(255.0f, 255.0f, 0, 255.0f);
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v != 0.0f && h == 0.0f)
            {
                Vector3 vector = new Vector3(0, 0, v);
                rbody.AddForce(vector * fuerza * Time.deltaTime);
                miCamara.transform.position = transform.position + setCamara;
            }
            if (h != 0.0f && v == 0.0f)
            {
                Vector3 vector = new Vector3(h, 0, 0);
                rbody.AddForce(vector * fuerza * Time.deltaTime);
                miCamara.transform.position = transform.position + setCamara;
            }
            if (estadopacman)
            {
                txttiempo.text = " " + timeLeftjuego.ToString();
            }
            else
            {
                txttiempo.text = " ";
                timeLeftjuego = 0;
            }

            


        }

        txtvidas.text = vidas+"";
    }
 
    void OnTriggerEnter(Collider obj)
    {
        //Destroy(obj.gameObject);
        if (obj.gameObject.tag == "comidapequeña")
        {
            obj.gameObject.SetActive(false);
            audios.PlayOneShot(impact);
            audios.Play();
            score= score+10;
            txtpuntaje.text = "Puntaje: " + score.ToString();
            if (score >= 1200)
            {
               
                fuerza = 1000;
                txtganador.text = "Ganaste !!";
                btnRestart.gameObject.SetActive(true);
                btnmenu.gameObject.SetActive(true);

            }

        }
        else if (obj.gameObject.tag == "comidagrande")
        {
            obj.gameObject.SetActive(false);
            audio2.PlayOneShot(impact2);
            audio2.Play();
            colorinicial.color = new Color(1.0f,1.0f, 1.0f);
            pacmanenojado = true;
            timeLeft = 10.0f;
            fuerza = 500.0f;
            audios.Stop();
            frutas = frutas + 1;
           txtfrutas.text = "Frutas: " + frutas.ToString();
            


        }
        else if (obj.gameObject.tag == "fantasma" )
        {
            if (pacmanenojado)
            {
                obj.gameObject.SetActive(false);
            }
            else {
                if (vidas <= 0)
                {
                    txtganador.text = "Perdiste!!";

                    btnRestart.gameObject.SetActive(true);
                    btnmenu.gameObject.SetActive(true);
                    sesionpasiva();
                    estadopacman = false;
                }
                else
                {
                    controlvidas();
                  
                }
                   
                }
            }
        }
    void sesionactiva()
    {
        
        MySqlConnection conn = new MySqlConnection(builder.ToString());
        MySqlCommand command = new MySqlCommand(
          "SELECT id,usuario, password,puntaje,num_vidas FROM jugador where estado = 'true';", conn);
        conn.Open();
        MySqlDataReader reader = command.ExecuteReader();
        print("conexion");
        while (reader.Read())
        {
            id = reader["id"].ToString();
            alias = reader["usuario"].ToString();
            contraseña = reader["password"].ToString();
            puntaje = (int) reader["puntaje"];
            vidas = (int) reader["num_vidas"];

        }
        print(vidas);
        print(puntaje);
    }
    void sesionpasiva()
    {
        int puntajefinal = score + puntaje;
        MySqlConnection conn = new MySqlConnection(builder.ToString());
        MySqlCommand command = new MySqlCommand("UPDATE jugador SET estado = 'false' , puntaje = "+puntajefinal+"  WHERE id = " + id + ";", conn);
        conn.Open();
        MySqlDataReader reader = command.ExecuteReader();
        conn.Close();
    }
    void controlvidas()
    {
        vidas = vidas - 1;
        print("control de vidas");
        MySqlConnection conn = new MySqlConnection(builder.ToString());
        MySqlCommand command = new MySqlCommand("UPDATE jugador SET num_vidas = "+vidas+" WHERE id = " + id + ";", conn);
        conn.Open();
        MySqlDataReader reader = command.ExecuteReader();
        conn.Close();
        Vector3 vector = new Vector3(15.92f,0.5f,-13.42f);
        setpacman =  vector;
        pacmanobjt.transform.position =  setpacman;



    }
    public void menu()
    {
      Application.LoadLevel("menu");
        sesionpasiva();
    }
    public void reloadGame()
    {
      Application.LoadLevel(Application.loadedLevel.ToString());
    }
}

