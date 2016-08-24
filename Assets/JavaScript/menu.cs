using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;
using System;

public class menu : MonoBehaviour {

    // Use this for initialization
    public Button btnJUGAR;
    public Button btnSALIR;
    public Button btnfacil;
    public Button btnmedio;
    public Button btndificil;
    public Button btnacceder;
    public Button btncancelar;
    public Button btncoins;
    private int puntaje;
    private string Nombre;
    private string Alias;
    public string id;
    private int vidas;
    private bool coins = false;
    [SerializeField]
    public Text resultado;
    [SerializeField]
    public Text txt_usuario;
    [SerializeField]
    public Text txt_contraseña;
    [SerializeField]
    private InputField alias_input = null;
    [SerializeField]
    private InputField pas_input = null;
    // Use this for initialization
    public MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
    void Start () {
        btnJUGAR.gameObject.SetActive(false);
        btnSALIR.gameObject.SetActive(false);
        btnfacil.gameObject.SetActive(false);
        btnmedio.gameObject.SetActive(false);
        btndificil.gameObject.SetActive(false);
        btnacceder.gameObject.SetActive(true);
        btncancelar.gameObject.SetActive(true);
        btncoins.gameObject.SetActive(false);
        builder.Server = "localhost";
        builder.UserID = "root";
        builder.Password = "";
        builder.Database = "pacman";

    }
	// Update is called once per frame
	void Update () {

	
	}
    public void login()
    {
        string nom_clave = alias_input.text;
        string pass = pas_input.text;
        string alias = "";
        string contraseña = "";
        MySqlConnection conn = new MySqlConnection(builder.ToString());
        MySqlCommand command = new MySqlCommand(
          "SELECT id,usuario, password ,num_vidas, puntaje FROM jugador where usuario = '"+ nom_clave+"';", conn);
        conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
        print("conexion");
        while (reader.Read())
        {
            id = reader["id"].ToString();
            alias = reader["usuario"].ToString();
            contraseña = reader["password"].ToString();
            vidas = (int) reader["num_vidas"];
            puntaje = (int)reader["puntaje"];

        }
        
        if (nom_clave == alias && pass == contraseña)
        {
            print("Cargada");
            actualizar(id);
            // nom_clave.text = "";
            // pas_input.text = "";
            if(vidas <= 0)
            {
                sinvidas();
                bloqueo();
                coins = true;
            }
            else
            {
                acceso();
                
            }
            

        }
        else { print("No Cargada"); }
    }
    void actualizar(string i)
    {
         MySqlConnection conn = new MySqlConnection(builder.ToString());
         MySqlCommand command = new MySqlCommand("UPDATE jugador SET estado = 'true' WHERE id = "+id+";", conn);
         conn.Open();
         MySqlDataReader reader = command.ExecuteReader();
        conn.Close();

    }
    void insertcoins()
    {
        MySqlConnection conn = new MySqlConnection(builder.ToString());
        MySqlCommand command = new MySqlCommand("UPDATE jugador SET num_vidas = 3 WHERE id = " + id + ";", conn);
        conn.Open();
        MySqlDataReader reader = command.ExecuteReader();
        conn.Close();
        


    }
    void sinvidas()
    {
        btncoins.gameObject.SetActive(true);
    }
    public void acceso()
    {
        btnJUGAR.gameObject.SetActive(true);
        btnSALIR.gameObject.SetActive(true);
        btnfacil.gameObject.SetActive(false);
        btnmedio.gameObject.SetActive(false);
        btndificil.gameObject.SetActive(false);
        btncoins.gameObject.SetActive(false);
        bloqueo();
        if (coins)
        {
            insertcoins();
        }
        
    }
    void bloqueo()
    {
        btnacceder.gameObject.SetActive(false);
        btncancelar.gameObject.SetActive(false);
        alias_input.gameObject.SetActive(false);
        pas_input.gameObject.SetActive(false);
        txt_contraseña.gameObject.SetActive(false);
        txt_usuario.gameObject.SetActive(false);
    }
    public void reloadGame()
    {
        btnJUGAR.gameObject.SetActive(false);

        btnmedio.gameObject.SetActive(true);

        if (puntaje > 1000)
        {
           
            btndificil.gameObject.SetActive(true);
        }
        if(puntaje > 2000){
            
            btnfacil.gameObject.SetActive(true);
        }
        
        
        
        
    }
    public void facil()
    {
        Application.LoadLevel("Pista");
       
    }
    public void medio()
    {
        Application.LoadLevel("Pista2");
        
    }
    public void dificil()
    {
        Application.LoadLevel("Pista3");
       
    }
}


