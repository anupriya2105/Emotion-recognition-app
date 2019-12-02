using System;
using MySql.Data.MySqlClient;

public class MusicController
{
    private static MySqlConnection connection;
    private static string server;
    private static string database;
    private static string uid;
    private static string password;

    public MusicController()
    {
        server = "localhost";
        database = "songs";
        uid = "root";
        password = "anuappu2105";
        string connectionString;
        connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);
        OpenConnection();
        CloseConnection();
    }

    //Insert statement
    public static bool Insert()
    {
        string query =
            "CREATE SCHEMA IF NOT EXISTS songs;" +
            "USE songs;" +
            "CREATE TABLE IF NOT EXISTS song(name VARCHAR(40), mood VARCHAR(10));" +
            "INSERT INTO song VALUES('AUD-20180710-WA0003.mp3', 'anger');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0004.mp3', 'anger');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0005.mp3', 'contempt');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0006.mp3', 'contempt');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0007.mp3', 'sadness');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0008.mp3', 'sadness');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0009.mp3', 'sadness');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0010.mp3', 'sadness');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0011.mp3', 'happiness');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0012.mp3', 'happiness');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0013.mp3', 'happiness');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0014.mp3', 'happiness');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0015.mp3', 'contempt');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0016.mp3', 'surprise');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0017.mp3', 'surprise');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0018.mp3', 'surprise');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0019.mp3', 'surprise');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0020.mp3', 'neutral');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0021.mp3', 'neutral');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0022.mp3', 'neutral');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0023.mp3', 'neutral');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0024.mp3', 'fear');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0025.mp3', 'fear');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0026.mp3', 'fear');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0027.mp3', 'disgust');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0028.mp3', 'disgust');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0029.mp3', 'disgust');" +
            "INSERT INTO song VALUES('AUD-20180710-WA0030.mp3', 'contempt');" +
            "";

        //open connection
        if (OpenConnection() == true)
        {
            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //Execute command
            cmd.ExecuteNonQuery();

            //close connection
            CloseConnection();
            return true;
        }

        return false;
    }

    public static string Select(string key)
    {
        string x = "";
        string query =
            "USE songs; SELECT name FROM song WHERE mood = '" + key + "' ORDER BY RAND() LIMIT 1";

        //open connection
        if (OpenConnection() == true)
        {
            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //Execute command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                x = (string)dataReader["name"];
            }
            //close connection
            CloseConnection();
        }

        return x;
    }

    //open connection to database
    private static bool OpenConnection()
    {
        try
        {

            connection.Open();
           
            return true;
        }
        catch (MySqlException ex)
        {
            //When handling errors, you can your application's response based 
            //on the error number.
            //The two most common error numbers when connecting are as follows:
            //0: Cannot connect to server.
            //1045: Invalid user name and/or password.
            switch (ex.Number)
            {
                case 0:
                    if (connection.Database == "")
                    {
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                    }
                    else
                    {
                        connection.ConnectionString = "SERVER=" + server + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                        Insert();
                    }

                    break;

                case 1045:
                    Console.WriteLine("Invalid username/password, please try again");
                    break;
                case 1042:
                    Console.WriteLine("Cannot connect to server");
                    break;
            }
            return false;
        }
    }

    //Close connection
    private static bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public string getMusicURI(string key)
    {

        return Select(key);
    }


}
